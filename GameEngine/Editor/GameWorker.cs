using OxLibrary.Controls;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Editor;
using PlayStationGames.AccountEngine.ControlFactory.Accessors;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.GameEngine.ControlFactory.Controls;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;
using PlayStationGames.GameEngine.ControlFactory.Controls.Trophies;
using OxDAOEngine.ControlFactory.Accessors;

namespace PlayStationGames.GameEngine.Editor
{
    public class GameWorker : DAOWorker<GameField, Game, GameFieldGroup>
    {
        public GameWorker() : base() { }

        protected override void AfterColorizeControls()
        {
            base.AfterColorizeControls();
            ((OxPictureContainer)Builder[GameField.Image].Control).BaseColor = Editor.MainPanel.Colors.Lighter();
            ((TrophysetPanel)Builder[GameField.Trophyset].Control).BaseColor = Editor.MainPanel.Colors.Lighter();
        }

        protected override void AfterFillControlsAndSetHandlers()
        {
            base.AfterFillControlsAndSetHandlers();
            SetRelatedGamesFilter();
        }

        protected override void AfterLayoutControls()
        {
            base.AfterLayoutControls();
            SetMaximumPlayersConstraints();
        }

        private void SetMaximumPlayersConstraints()
        {
            Builder[GameField.MaximumPlayers].MinimumValue = 2;
            Builder[GameField.MaximumPlayers].MaximumValue =
                Builder[GameField.Platform].EnumValue<PlatformType>() switch
                {
                    PlatformType.PS3 =>
                        7,
                    PlatformType.PS2 =>
                        8,
                    _ =>
                        4
                };
        }

        protected override FieldGroupFrames<GameField, GameFieldGroup> GetFieldGroupFrames() =>
            new()
            {
                Editor.Groups,
                { GameFieldGroup.System, Editor.MainPanel.Footer }
            };

        protected override void AfterAlignLabels()
        {
            base.AfterAlignLabels();
            Control licenseCheckBox = Layouter.PlacedControl(GameField.Licensed)!.Control;
            Control nameControl = Layouter.PlacedControl(GameField.Name)!.Control;
            licenseCheckBox.Left =
                (Layouter.PlacedControl(GameField.Source)!.Control.Right +
                 nameControl.Right -
                 licenseCheckBox.Width) / 2;
            licenseCheckBox.Top = nameControl.Top + (nameControl.Height - licenseCheckBox.Height) / 2;
            ((OxPictureContainer)Layouter.PlacedControl(GameField.Image)!.Control).HiddenBorder = true;
            Builder[GameField.AvailablePlatinum].Text = "";
        }

        private void SetRelatedGamesFilter()
        {
            if (Item == null)
                return;

            Filter<GameField, Game> relatedGameFilter = new();
            relatedGameFilter.AddFilter(GameField.Id, FilterOperation.NotEquals, Item.Id, FilterConcat.AND);

            foreach (RelatedGame relatedGame in Builder.Value<RelatedGames>(GameField.RelatedGames)!)
                relatedGameFilter.AddFilter(
                    GameField.Id, FilterOperation.NotEquals, relatedGame.GameId, FilterConcat.AND
                );

            Builder.Control<RelatedGamesControl>(GameField.RelatedGames).Filter = relatedGameFilter;
        }

        protected override bool SyncFieldValue(GameField field, bool byUser)
        {
            switch (field)
            {
                case GameField.RelatedGames:
                    FillControls();
                    break;
                case GameField.Platform:
                    if (byUser)
                        SyncFormatWithPlatform();

                    SyncOwnerWithControls(byUser);
                    SetCoachMultiplayerVisible();
                    ShowMaximumPlayersControl();
                    SetMaximumPlayersConstraints();
                    break;
                case GameField.Licensed:
                    SyncOwnerWithControls(byUser);

                    if (!AvailableTrophyset)
                        ((TrophysetPanel)Builder[GameField.Trophyset].Control).Type = TrophysetType.NoSet;
                    break;
                case GameField.Source:
                    SyncOwnerWithControls(byUser);
                    break;
                case GameField.CoachMultiplayer:
                    ShowMaximumPlayersControl();
                    break;
            }

            SetRelatedGamesFilter();

            return field is 
                GameField.Licensed or
                GameField.Platform or
                GameField.Format or
                GameField.Trophyset or
                GameField.Source or
                GameField.Verified or
                GameField.CoachMultiplayer;
        }

        private bool SupportCoathMultiplayer =>
            TypeHelper.Helper<PlatformTypeHelper>().SupportCoachMultiplayer(
                Builder[GameField.Platform].EnumValue<PlatformType>());

        private void SetCoachMultiplayerVisible()
        {
            Builder[GameField.CoachMultiplayer].Visible = SupportCoathMultiplayer;

            if (!SupportCoathMultiplayer)
                Builder[GameField.CoachMultiplayer].Value = false;
        }

        private void ShowMaximumPlayersControl()
        {
            OxLabel label = Layouter.PlacedControl(GameField.MaximumPlayers)!.Label!;
            IControlAccessor coachMultiplayer = Builder[GameField.CoachMultiplayer];
            IControlAccessor onlineMultiplayer = Builder[GameField.OnlineMultiplayer];
            IControlAccessor maximumPlayers = Builder[GameField.MaximumPlayers];

            if (coachMultiplayer.BoolValue)
            {
                maximumPlayers.Visible = true;
                label.Visible = true;
                onlineMultiplayer.Top = maximumPlayers.Bottom + 2;
            }
            else
            {
                maximumPlayers.Visible = false;
                label.Visible = false;
                onlineMultiplayer.Top = 
                    SupportCoathMultiplayer 
                    ? label.Top - 4
                    : coachMultiplayer.Top;
            }
        }

        private readonly SourceHelper sourceHelper = TypeHelper.Helper<SourceHelper>();
        private readonly GameFieldGroupHelper groupHelper = TypeHelper.Helper<GameFieldGroupHelper>();
        private readonly GameFormatHelper formatHelper = TypeHelper.Helper<GameFormatHelper>();

        protected override bool SetGroupsAvailability(bool afterSyncValues = false)
        {
            Editor.Groups[GameFieldGroup.Trophyset].Visible = AvailableTrophyset;
            Editor.Groups[GameFieldGroup.Installations].Visible =
                sourceHelper.InstallationsSupport(Builder.Value<Source>(GameField.Source));

            bool isEmulator = Builder.Value<GameFormat>(GameField.Format) == GameFormat.Emulator;
            Editor.Groups[GameFieldGroup.Emulator].Visible = isEmulator;
            Editor.Groups[GameFieldGroup.Genre].Visible = !isEmulator;
            Editor.Groups[GameFieldGroup.RelatedGames].Visible = !isEmulator;
            Editor.Groups[GameFieldGroup.ReleaseBase].Visible = !isEmulator;

            Builder.SetVisible(GameField.EmulatorType, isEmulator);

            Editor.Groups[GameFieldGroup.Devices].Visible = TypeHelper.Helper<DeviceTypeHelper>().
                Available(Builder.Value<PlatformType>(GameField.Platform)).Count > 0;

            bool withoutTrophyset = 
                ((TrophysetPanel)Builder[GameField.Trophyset].Control).Type == TrophysetType.NoSet;
            Builder.SetVisible(GameField.Difficult, !withoutTrophyset);
            Builder.SetVisible(GameField.CompleteTime, !withoutTrophyset);

            bool verified = Builder.Value<bool>(GameField.Verified);
            List<GameField> unverifiedFields = GameFieldHelper.UnverifiedFields();

            if (!verified)
            {
                foreach (GameFieldGroup group in groupHelper.VerifiedGroups)
                    foreach (GameField field in groupHelper.Fields(group))
                        Builder[field].ReadOnly = false;

                //TrophysetPanel.ReadOnly = false;
            }

            bool editionVisible =
                !isEmulator
                && (!verified || !Builder[GameField.Edition].IsEmpty);
            Builder.SetVisible(GameField.Edition, editionVisible);
            Builder.SetVisible(GameField.Serieses, 
                !isEmulator 
                && (!verified || !Builder[GameField.Serieses].IsEmpty)
            );

            Builder[GameField.Serieses].Top = (editionVisible
                ? Builder[GameField.Edition].Bottom
                : Builder[GameField.Edition].Top) 
                + Generator!.Offset(GameField.Serieses);

            if (verified)
            {
                //TrophysetPanel.ReadOnly = true;

                foreach (GameFieldGroup group in groupHelper.VerifiedGroups)
                    foreach (GameField field in groupHelper.Fields(group))
                        Builder[field].ReadOnly = !unverifiedFields.Contains(field);
            }

            if (!verified)
                Builder[GameField.Owner].Enabled = AccountAvailable();

            if (afterSyncValues && ownerReadOnlyChanged)
                Builder[GameField.Owner].SetDefaultValue();

            Editor.Groups[GameFieldGroup.DLC].Visible = Builder.Value<bool>(GameField.Licensed) 
                && (!verified || !Builder[GameField.Dlcs].IsEmpty);
            return true;
        }

        private bool AvailableTrophyset =>
            new Game()
            {
                PlatformType = Builder[GameField.Platform].EnumValue<PlatformType>(),
                Format = Builder[GameField.Format].EnumValue<GameFormat>(),

            }.TrophysetAvailable;

        private void SyncFormatWithPlatform() =>
            Builder[GameField.Format].Value = TypeHelper.TypeObject<GameFormat>(
                formatHelper.DefaultFormat(
                    Builder.Value<PlatformType>(GameField.Platform)
                )
            );
        private bool ownerReadOnlyChanged = false;

        private void SyncOwnerWithControls(bool byUser)
        {
            bool accountAvailable = AccountAvailable();
            ownerReadOnlyChanged = accountAvailable != Builder[GameField.Owner].Enabled;

            if (byUser && !ownerReadOnlyChanged)
                return;

            if (Builder[GameField.Owner].Context.AdditionalContext is AccountAccessorParameters parameters)
            {
                parameters.UseNullable = !accountAvailable;
                parameters.OnlyNullable = !accountAvailable;
            }
            
            Builder[GameField.Owner].RenewControl(true);

            if (!accountAvailable )
                Builder[GameField.Owner].Value = null;
            else
                if (!byUser)
                    Builder[GameField.Owner].Value = Item!.Owner;
        }

        private bool AccountAvailable() => 
            new Game()
            {
                Licensed = Builder[GameField.Licensed].BoolValue,
                PlatformType = Builder[GameField.Platform].EnumValue<PlatformType>(),
                SourceType = Builder[GameField.Source].EnumValue<Source>()

            }.AccountAvailable;

        protected override List<List<GameField>> LabelGroups => new()
        {
            new List<GameField> {
                GameField.Owner,
                GameField.Source,
                GameField.Platform,
                GameField.Format
            },
            new List<GameField> {
                GameField.Region,
                GameField.Name,
                GameField.Edition,
                GameField.Serieses,
                GameField.EmulatorType
            },
            new List<GameField> {
                GameField.Developer,
                GameField.Publisher,
                GameField.Year,
                GameField.Pegi,
                GameField.CriticScore
            },
            new List<GameField> {
                GameField.Genre,
                GameField.ScreenView
            }
        };

        protected override void BeforeGrabControls()
        {
            base.BeforeGrabControls();

            if (Item == null)
                return;

            if (!AvailableTrophyset)
                Builder[GameField.Trophyset].Clear();

            List<GameFieldGroup> invisibleGroups = new();

            if (!Builder.Value<bool>(GameField.Licensed))
                invisibleGroups.Add(GameFieldGroup.DLC);

            if (Builder.Value<GameFormat>(GameField.Format) == GameFormat.Emulator)
            {
                invisibleGroups.Add(GameFieldGroup.Genre);
                invisibleGroups.Add(GameFieldGroup.RelatedGames);
                invisibleGroups.Add(GameFieldGroup.ReleaseBase);
                invisibleGroups.Add(GameFieldGroup.ReleasePlatforms);
                Builder[GameField.Edition].Clear();
                Builder[GameField.Serieses].Clear();
            }
            else
            {
                invisibleGroups.Add(GameFieldGroup.Emulator);
                Builder[GameField.EmulatorType].Clear();
            }

            if (!sourceHelper.InstallationsSupport(Builder.Value<Source>(GameField.Source)))
                invisibleGroups.Add(GameFieldGroup.Installations);
            else SyncInstallationsWithPlatform();

            foreach (GameFieldGroup group in invisibleGroups)
                foreach (GameField field in groupHelper.Fields(group))
                    Builder[field].Clear();

            Item[GameField.RelatedGames] = Builder.Value<RelatedGames>(GameField.RelatedGames);
        }

        private void SyncInstallationsWithPlatform()
        {
            ListDAO<Installation>? installations = Builder.Value<ListDAO<Installation>>(GameField.Installations);

            if (installations == null)
                return;

            RootListDAO<ConsoleField, PSConsole> availableConsoles = 
                DataManager.FullItemsList<ConsoleField, PSConsole>().FilteredList(Game.AvailableConsoleFilter(Builder));

            installations.RemoveAll(i => !availableConsoles.Contains(c => c.Id == i.ConsoleId));
            Builder[GameField.Installations].Value = installations;
        }

        protected override void AfterGrabControls() => 
            Item?.ReleasePlatforms.Add(Item.PlatformType);

        protected override EditorLayoutsGenerator<GameField, Game, GameFieldGroup> CreateLayoutsGenerator(
            FieldGroupFrames<GameField, GameFieldGroup> frames, ControlLayouter<GameField, Game> layouter
            ) =>
            new GameEditorLayoutsGenerator(frames, layouter);
    }
}
using OxLibrary;
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
using OxDAOEngine.ControlFactory.Accessors;

namespace PlayStationGames.GameEngine.Editor
{
    public class GameWorker : DAOWorker<GameField, Game, GameFieldGroup>
    {
        public GameWorker() : base() =>
            trophiesControlsHelper = new TrophiesControlsHelper(Builder, (GameEditorLayoutsGenerator)Generator!);

        protected override void PrepareStyles() =>
            Editor.MainPanel.BaseColor = new OxColorHelper(
                TypeHelper.BackColor(Builder.Value<Source>(GameField.Source))
            ).Darker(7);

        protected override void AfterColorizeControls()
        {
            base.AfterColorizeControls();
            ((OxPictureContainer)Layouter.PlacedControl(GameField.Image)!.Control).BaseColor = 
                new OxColorHelper(Editor.MainPanel.BaseColor).Lighter();
        }

        protected override void BeforeFillControls()
        {
            base.BeforeFillControls();
            trophiesControlsHelper.ClearTrophiesControlsConstraints();
        }

        private OxLabel? earnedTrophiesLabel;
        private OxLabel? availableTrophiesLabel;

        protected override void BeforeGenerateLayouts()
        {
            base.BeforeGenerateLayouts();
            earnedTrophiesLabel ??= CreateTrophiesLabel("Earned", 76);
            availableTrophiesLabel ??= CreateTrophiesLabel("Available", 148);
        }

        private OxLabel CreateTrophiesLabel(string text, int left) =>
            new()
            {
                Text = text,
                Left = left,
                Top = 112,
                Parent = Editor.Groups[GameFieldGroup.Trophyset],
                AutoSize = true,
                Font = new Font(Styles.FontFamily, Styles.DefaultFontSize,
                        FontStyle.Bold | FontStyle.Underline | FontStyle.Italic)
            };

        protected override void AfterFillControls()
        {
            base.AfterFillControls();
            trophiesControlsHelper.CalcTrophiesControls();
            trophiesControlsHelper.ClearUnusedCaptions();
        }

        protected override void AfterLayoutControls()
        {
            base.AfterLayoutControls();

            foreach (GameField field in fieldHelper.TrophiesFields)
            {
                if (field is GameField.AvailablePlatinum or GameField.EarnedPlatinum)
                    continue;

                ((NumericAccessor<GameField, Game>)Builder[field]).CenteredReadonlyText = true;
            }
        }

        protected override void AfterFillControlsAndSetHandlers()
        {
            base.AfterFillControlsAndSetHandlers();
            SetRelatedGamesFilter();
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
            trophiesControlsHelper.AlignLabels();

            Control licenseCheckBox = Layouter.PlacedControl(GameField.Licensed)!.Control;
            Control nameControl = Layouter.PlacedControl(GameField.Name)!.Control;
            licenseCheckBox.Left =
                (Layouter.PlacedControl(GameField.Source)!.Control.Right +
                 nameControl.Right -
                 licenseCheckBox.Width) / 2;
            licenseCheckBox.Top = nameControl.Top + (nameControl.Height - licenseCheckBox.Height) / 2;
            ((OxPictureContainer)Layouter.PlacedControl(GameField.Image)!.Control).HiddenBorder = true;
        }

        private void SetRelatedGamesFilter()
        {
            if (Item == null)
                return;

            Builder.Control<RelatedGamesControl>(GameField.RelatedGames).ParentItem = Item;
            Builder.Control<RelatedGamesControl>(GameField.RelatedGames).AvailableTrophyset = 
                AvailableTrophyset 
                && Builder.Value<TrophysetAccess>(GameField.TrophysetAccess) != TrophysetAccess.NoSet;

            Filter<GameField, Game> relatedGameFilter = new();
            relatedGameFilter.AddFilter(GameField.Id, FilterOperation.NotEquals, Item.Id, FilterConcat.AND);

            foreach (RelatedGame relatedGame in Builder.Value<RelatedGames>(GameField.RelatedGames)!)
                relatedGameFilter.AddFilter(
                    GameField.Id, FilterOperation.NotEquals, relatedGame.GameId, FilterConcat.AND
                );

            Builder.Control<RelatedGamesControl>(GameField.RelatedGames).Filter = relatedGameFilter;
        }

        protected override bool SyncFieldValues(GameField field, bool byUser)
        {
            if (new List<GameField>{
                    GameField.Name,
                    GameField.Platform,
                    GameField.Source}
                .Contains(field))
                FillFormCaptionFromControls();

            switch (field)
            {
                case GameField.Platform:
                    if (byUser)
                        SyncFormatWithPlatform();

                    SyncOwnerWithControls(byUser);
                    break;
                case GameField.TrophysetAccess:
                case GameField.AvailablePlatinum:
                case GameField.AvailableGold:
                case GameField.AvailableSilver:
                case GameField.AvailableBronze:
                case GameField.AvailableFromDLC:
                case GameField.AvailableNet:
                case GameField.EarnedGold:
                case GameField.EarnedSilver:
                case GameField.EarnedBronze:
                case GameField.EarnedFromDLC:
                case GameField.EarnedNet:
                    trophiesControlsHelper.CalcTrophiesControls();
                    break;
                case GameField.Licensed:
                    SyncOwnerWithControls(byUser);
                    trophiesControlsHelper.CalcTrophiesControls();
                    break;
                case GameField.Source:
                    SyncOwnerWithControls(byUser);
                    break;
            }

            SetRelatedGamesFilter();

            return field switch
            {
                GameField.Licensed or
                GameField.Platform or 
                GameField.Format or
                GameField.TrophysetAccess or
                GameField.Source or 
                GameField.Verified => 
                    true,
                _ =>
                    false,
            };
        }

        private readonly SourceHelper sourceHelper = TypeHelper.Helper<SourceHelper>();
        private readonly GameFieldGroupHelper groupHelper = TypeHelper.Helper<GameFieldGroupHelper>();
        private readonly GameFormatHelper formatHelper = TypeHelper.Helper<GameFormatHelper>();
        private readonly GameFieldHelper fieldHelper = TypeHelper.Helper<GameFieldHelper>();

        protected override bool SetGroupsAvailability(bool afterSyncValues = false)
        {
            Editor.Groups[GameFieldGroup.Trophyset].Visible = Editor.Groups[GameFieldGroup.Trophyset].Visible = AvailableTrophyset;
            Editor.Groups[GameFieldGroup.Installations].Visible =
                sourceHelper.InstallationsSupport(Builder.Value<Source>(GameField.Source));

            bool isEmulator = Builder.Value<GameFormat>(GameField.Format) == GameFormat.Emulator;
            Editor.Groups[GameFieldGroup.Emulator].Visible = isEmulator;
            Editor.Groups[GameFieldGroup.Genre].Visible = !isEmulator;
            Editor.Groups[GameFieldGroup.RelatedGames].Visible = !isEmulator;
            Editor.Groups[GameFieldGroup.ReleaseBase].Visible = !isEmulator;

            Builder.SetVisible(GameField.EmulatorType, isEmulator);

            bool withoutTrophyset = Builder.Value<TrophysetAccess>(GameField.TrophysetAccess) == TrophysetAccess.NoSet;

            foreach (GameField field in fieldHelper.TrophiesFields)
                Builder.SetVisible(field, !withoutTrophyset);

            Builder.SetVisible(GameField.Difficult, !withoutTrophyset);
            Builder.SetVisible(GameField.CompleteTime, !withoutTrophyset);

            if (earnedTrophiesLabel != null)
                earnedTrophiesLabel.Visible = !withoutTrophyset;

            if (availableTrophiesLabel != null)
                availableTrophiesLabel.Visible = !withoutTrophyset;

            bool verified = Builder.Value<bool>(GameField.Verified);
            List<GameField> unverifiedFields = fieldHelper.UnverifiedFields();

            if (!verified)
                foreach (GameFieldGroup group in groupHelper.VerifiedGroups)
                    foreach (GameField field in groupHelper.Fields(group))
                        Builder[field].ReadOnly = false;

            trophiesControlsHelper.SetTrophiesControlsVisible(verified);
            bool editionVisible =
                !isEmulator
                && (!verified || !Builder[GameField.Edition].IsEmpty);
            Builder.SetVisible(GameField.Edition, editionVisible);
            Builder.SetVisible(GameField.Series, 
                !isEmulator 
                && (!verified || !Builder[GameField.Series].IsEmpty)
            );

            Builder[GameField.Series].Top = (editionVisible
                ? Builder[GameField.Edition].Bottom
                : Builder[GameField.Edition].Top) 
                + Generator!.Offset(GameField.Series);

            if (verified)
                foreach (GameFieldGroup group in groupHelper.VerifiedGroups)
                    foreach (GameField field in groupHelper.Fields(group))
                        Builder[field].ReadOnly = !unverifiedFields.Contains(field);

            if (!verified)
                Builder[GameField.Owner].Enabled = AccountAvailable();

            if (afterSyncValues && ownerReadOnlyChanged)
                Builder[GameField.Owner].SetDefaultValue();

            Editor.Groups[GameFieldGroup.DLC].Visible = Builder.Value<bool>(GameField.Licensed) 
                && (!verified || !Builder[GameField.Dlcs].IsEmpty);
            return false;
        }

        private bool AvailableTrophyset =>
            new Game()
            {
                PlatformType = Builder[GameField.Platform].EnumValue<PlatformType>(),
                Format = Builder[GameField.Format].EnumValue<GameFormat>(),

            }.TrophysetAvailable;

        private void FillFormCaptionFromControls() => 
            FillFormCaption(
                new Game
                {
                    Name = Builder[GameField.Name].SingleStringValue,
                    PlatformType = Builder.Value<PlatformType>(GameField.Platform),
                    SourceType = Builder.Value<Source>(GameField.Source),
                    GameRegion = Builder.Value<GameRegion>(GameField.Region)
                });

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

        protected readonly TrophiesControlsHelper trophiesControlsHelper;

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
                GameField.Series,
                GameField.EmulatorType
            },
            new List<GameField> {
                GameField.TrophysetAccess,
                GameField.Difficult,
                GameField.CompleteTime,
                GameField.EarnedBronze,
                GameField.EarnedSilver,
                GameField.EarnedGold,
                GameField.EarnedPlatinum,
                GameField.EarnedFromDLC,
                GameField.EarnedNet
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
            {
                Builder[GameField.TrophysetAccess].Value = TrophysetAccess.NoSet;
                trophiesControlsHelper.CalcTrophiesControls();
            }

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
                Builder[GameField.Series].Clear();
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
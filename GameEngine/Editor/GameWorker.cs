﻿using OxLibrary;
using OxXMLEngine.ControlFactory;
using OxXMLEngine.Data.Fields;
using OxXMLEngine.Data.Filter;
using OxXMLEngine.Data.Types;
using OxXMLEngine.Editor;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.ControlFactory.Controls;
using PlayStationGames.GameEngine.Data.Types;
using OxLibrary.Controls;

namespace PlayStationGames.GameEngine.Editor
{
    public class GameWorker : DAOWorker<GameField, Game, GameFieldGroup>
    {
        public GameWorker() : base() =>
            trophiesControlsHelper = new TrophiesControlsHelper(Builder);

        protected override void PrepareStyles() =>
            Editor.MainPanel.BaseColor = new OxColorHelper(
                TypeHelper.BackColor(Builder.Value<Source>(GameField.Source))
            ).Darker(7);

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

            if (earnedTrophiesLabel == null)
                earnedTrophiesLabel = CreateTrophiesLabel("Earned", 76);

            if (availableTrophiesLabel == null)
                availableTrophiesLabel = CreateTrophiesLabel("Available", 148);
        }

        private OxLabel CreateTrophiesLabel(string text, int left) =>
            new()
            {
                Text = text,
                Left = left,
                Top = 52,
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
            PlacedControl<GameField>? licensedControl = Layouter.PlacedControl(GameField.Licensed);
            PlacedControl<GameField>? sourceControl = Layouter.PlacedControl(GameField.Source);

            if (licensedControl != null && sourceControl != null)
                licensedControl.Control.Left = sourceControl.LabelLeft - 1;
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
                    break;
                case GameField.Licensed:
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

        protected override void SetGroupsAvailability()
        {
            Editor.Groups[GameFieldGroup.Trophyset].Visible = CalcedTrophiesVisible;
            Editor.Groups[GameFieldGroup.Installations].Visible =
                sourceHelper.InstallationsSupport(Builder.Value<Source>(GameField.Source));

            bool isEmulator = Builder.Value<GameFormat>(GameField.Format) == GameFormat.Emulator;
            Editor.Groups[GameFieldGroup.Emulator].Visible = isEmulator;
            Editor.Groups[GameFieldGroup.Genre].Visible = !isEmulator;
            Editor.Groups[GameFieldGroup.RelatedGames].Visible = !isEmulator;
            Editor.Groups[GameFieldGroup.DLC].Visible = !isEmulator;
            Editor.Groups[GameFieldGroup.ReleaseBase].Visible = !isEmulator;

            Builder.SetVisible(GameField.Edition, !isEmulator);
            Builder.SetVisible(GameField.Series, !isEmulator);
            Builder.SetVisible(GameField.EmulatorType, isEmulator);

            bool withoutTrophyset = Builder.Value<TrophysetAccessibility>(GameField.TrophysetAccess) == TrophysetAccessibility.NoSet;

            Builder.SetVisible(GameField.AvailableBronze, !withoutTrophyset);
            Builder.SetVisible(GameField.AvailableSilver, !withoutTrophyset);
            Builder.SetVisible(GameField.AvailableGold, !withoutTrophyset);
            Builder.SetVisible(GameField.AvailablePlatinum, !withoutTrophyset);
            Builder.SetVisible(GameField.AvailableFromDLC, !withoutTrophyset);
            Builder.SetVisible(GameField.AvailableNet, !withoutTrophyset);
            Builder.SetVisible(GameField.EarnedBronze, !withoutTrophyset);
            Builder.SetVisible(GameField.EarnedSilver, !withoutTrophyset);
            Builder.SetVisible(GameField.EarnedGold, !withoutTrophyset);
            Builder.SetVisible(GameField.EarnedPlatinum, !withoutTrophyset);
            Builder.SetVisible(GameField.EarnedFromDLC, !withoutTrophyset);
            Builder.SetVisible(GameField.EarnedNet, !withoutTrophyset);
            Builder.SetVisible(GameField.Difficult, !withoutTrophyset);
            Builder.SetVisible(GameField.CompleteTime, !withoutTrophyset);

            if (earnedTrophiesLabel != null)
                earnedTrophiesLabel.Visible = !withoutTrophyset;

            if (availableTrophiesLabel != null)
                availableTrophiesLabel.Visible = !withoutTrophyset;

            bool verified = Builder.Value<bool>(GameField.Verified);
            GameFieldGroupHelper helper = TypeHelper.Helper<GameFieldGroupHelper>();

            foreach (GameFieldGroup group in verifiedGroups)
                foreach (GameField field in helper.Fields(group))
                    Builder[field].ReadOnly = verified;
        }

        private bool CalcedTrophiesVisible =>
            Builder.Value<bool>(GameField.Licensed)
            && TypeHelper.Helper<GameFormatHelper>().AvailableTrophies(Builder.Value<GameFormat>(GameField.Format))
            && TypeHelper.Helper<PlatformTypeHelper>().IsPSNPlatform(Builder.Value<PlatformType>(GameField.Platform));
        
        private readonly List<GameFieldGroup> verifiedGroups =
            new()
            {
                GameFieldGroup.Base,
                GameFieldGroup.DLC,
                GameFieldGroup.GameMode,
                GameFieldGroup.Genre,
                GameFieldGroup.Link,
                GameFieldGroup.ReleaseBase,
                GameFieldGroup.Trophyset,
                GameFieldGroup.Emulator
            };

        private void FillFormCaptionFromControls() => 
            FillFormCaption(
                new Game
                {
                    Name = Builder[GameField.Name].StringValue,
                    PlatformType = TypeHelper.Value<PlatformType>(Builder.Value(GameField.Platform)),
                    SourceType = TypeHelper.Value<Source>(Builder.Value(GameField.Source)),
                    GameRegion = TypeHelper.Value<GameRegion>(Builder.Value(GameField.Region))
                });

        private void SyncFormatWithPlatform() =>
            Builder[GameField.Format].Value = TypeHelper.TypeObject<GameFormat>(
                TypeHelper.Helper<GameFormatHelper>().DefaultFormat(
                    TypeHelper.Value<PlatformType>(
                        Builder.Value(GameField.Platform)
                    )
                )
            );

        protected readonly TrophiesControlsHelper trophiesControlsHelper;

        protected override List<List<GameField>> LabelGroups => new()
        {
            new List<GameField> {
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

            if (!CalcedTrophiesVisible)
            {
                Builder[GameField.TrophysetAccess].Value = TrophysetAccessibility.NoSet;
                trophiesControlsHelper.CalcTrophiesControls();
            }

            List<GameFieldGroup> invisibleGroups = new();

            if (Builder.Value<GameFormat>(GameField.Format) == GameFormat.Emulator)
            {
                invisibleGroups.Add(GameFieldGroup.Genre);
                invisibleGroups.Add(GameFieldGroup.RelatedGames);
                invisibleGroups.Add(GameFieldGroup.DLC);
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

            GameFieldGroupHelper helper = TypeHelper.Helper<GameFieldGroupHelper>();

            foreach (GameFieldGroup group in invisibleGroups)
                foreach (GameField field in helper.Fields(group))
                    Builder[field].Clear();

            Item[GameField.RelatedGames] = Builder.Value<RelatedGames>(GameField.RelatedGames);
        }

        protected override void AfterGrabControls()
        {
            if (Item != null)
                Item.ReleasePlatforms.Add(Item.PlatformType);
        }

        protected override EditorLayoutsGenerator<GameField, Game, GameFieldGroup> CreateLayoutsGenerator(
            FieldGroupFrames<GameField, GameFieldGroup> frames, ControlLayouter<GameField, Game> layouter
            ) =>
            new GameEditorLayoutsGenerator(frames, layouter);
    }
}
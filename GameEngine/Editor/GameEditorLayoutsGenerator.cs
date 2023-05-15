using OxXMLEngine.ControlFactory;
using OxXMLEngine.Data.Fields;
using OxXMLEngine.Editor;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Editor
{
    public class GameEditorLayoutsGenerator: EditorLayoutsGenerator<GameField, Game, GameFieldGroup>
    {
        public GameEditorLayoutsGenerator(FieldGroupFrames<GameField, GameFieldGroup> groupFrames,
            ControlLayouter<GameField, Game> layouter) : base(groupFrames, layouter) { }


        protected override List<GameField> ControlsWithoutLabel() =>
            new()
            {
                GameField.Image,
                GameField.RelatedGames,
                GameField.Dlcs,
                GameField.Links,
                GameField.GameModes,
                GameField.AvailablePlatinum,
                GameField.AvailableGold,
                GameField.AvailableSilver,
                GameField.AvailableBronze,
                GameField.AvailableFromDLC,
                GameField.AvailableNet,
                GameField.ReleasePlatforms,
                GameField.Verified,
                GameField.Licensed,
                GameField.Tags
            };

        protected override List<GameField> AutoSizeFields() =>
            new()
            {
                GameField.AvailablePlatinum,
                GameField.EarnedPlatinum,
                GameField.Verified,
                GameField.Licensed
            };

        protected override List<GameField> FillDockFields() =>
            new()
            {
                GameField.RelatedGames,
                GameField.Dlcs,
                GameField.Tags,
                GameField.Links,
                GameField.Installations,
                GameField.GameModes,
                GameField.EmulatorROMs
            };

        protected override List<GameField> OffsettingFields() =>
            new()
            {
                GameField.Image,
                GameField.Edition,
                GameField.Source,
                GameField.Platform,
                GameField.Format,
                GameField.CriticScore,
                GameField.Series,
                GameField.EarnedGold,
                GameField.EarnedSilver,
                GameField.EarnedBronze,
                GameField.EarnedFromDLC,
                GameField.EarnedNet,
                GameField.AvailableGold,
                GameField.AvailableSilver,
                GameField.AvailableBronze,
                GameField.AvailableFromDLC,
                GameField.AvailableNet,
                GameField.Difficult,
                GameField.CompleteTime,
                GameField.Genre,
                GameField.Publisher,
                GameField.Year,
                GameField.Pegi,
                GameField.Region
            };


        protected override AnchorStyles Anchors(GameField field)
        {
            AnchorStyles anchors = base.Anchors(field);

            switch (field)
            {
                case GameField.Verified:
                    anchors |= AnchorStyles.Bottom;
                    break;
            }

            return anchors;
        }

        protected override Color BackColor(GameField field) => 
            field switch
            {
                GameField.Image =>
                    GroupFrames[GameFieldGroup.Base].BaseColor,
                GameField.Verified or
                GameField.Licensed or
                GameField.AvailablePlatinum or
                GameField.EarnedPlatinum or
                GameField.ReleasePlatforms =>
                    Color.Transparent,
                _ =>
                    Color.FromArgb(250, 250, 250),
            };

        protected override int Top(GameField field) => 
            field switch
            {
                GameField.Licensed =>
                    Layouter[GameField.Image]!.Top,
                GameField.Verified =>
                    (Parent(field).Height - Height(field)) / 2,
                GameField.AvailablePlatinum or
                GameField.EarnedPlatinum =>
                    80,
                GameField.EmulatorType =>
                    Layouter[GameField.Edition]!.Top,
                GameField.Language or
                GameField.Code =>
                    Layouter[GameField.Region]!.Top,
                _ =>
                    8
            };

        protected override int Left(GameField field) => 
            field switch
            {
                GameField.Image or 
                GameField.Verified => 
                    8,
                GameField.Licensed => 
                    200,
                GameField.Source or 
                GameField.Platform or 
                GameField.Format => 
                    290,
                GameField.Name or 
                GameField.Edition or 
                GameField.Series or
                GameField.Region or
                GameField.EmulatorType =>
                    68,
                GameField.ScreenView or
                GameField.Genre =>
                    60,
                GameField.TrophysetAccess or 
                GameField.EarnedGold or 
                GameField.EarnedSilver or 
                GameField.EarnedBronze or 
                GameField.EarnedFromDLC or 
                GameField.EarnedNet or
                GameField.Difficult or
                GameField.CompleteTime =>
                    76,
                GameField.EarnedPlatinum => 
                    102,
                GameField.AvailablePlatinum => 
                    174,
                GameField.AvailableGold or 
                GameField.AvailableSilver or 
                GameField.AvailableBronze or 
                GameField.AvailableFromDLC or 
                GameField.AvailableNet =>
                    148,
                GameField.Language =>
                    206,
                GameField.Code =>
                    342,
                GameField.Developer or 
                GameField.Publisher or 
                GameField.Year or 
                GameField.Pegi or 
                GameField.CriticScore => 
                    84,
                GameField.ReleasePlatforms => 
                    455,
                _ => 
                    0,
            };

        protected override int Height(GameField field) =>
            field == GameField.Image
                ? 110
                : field == GameField.Verified ||
                    field == GameField.Licensed
                    ? 20
                    : base.Height(field);

        protected override int Width(GameField field) => 
            field switch
            {
                GameField.Image => 
                    200,
                GameField.Source or 
                GameField.Platform or 
                GameField.Format => 
                    140,
                GameField.ScreenView => 
                    112,
                GameField.Name or 
                GameField.Edition or 
                GameField.Series or 
                GameField.EmulatorType or
                GameField.Developer or 
                GameField.Publisher => 
                    362,
                GameField.TrophysetAccess => 
                    158,
                GameField.AvailableGold or 
                GameField.AvailableSilver or 
                GameField.AvailableBronze or 
                GameField.AvailableFromDLC or 
                GameField.AvailableNet or 
                GameField.EarnedGold or 
                GameField.EarnedSilver or 
                GameField.EarnedBronze or 
                GameField.EarnedFromDLC or 
                GameField.EarnedNet or 
                GameField.Year or 
                GameField.Pegi or 
                GameField.CriticScore or
                GameField.Difficult =>
                    64,
                GameField.Region =>
                    52,
                GameField.Language
                    => 80,
                GameField.CompleteTime =>
                    104,
                GameField.Code =>
                    88,
                GameField.Genre => 
                    174,
                GameField.ReleasePlatforms => 
                    600,
                _ => 
                    0,
            };

        protected override int Offset(GameField field) => 
            field switch
            {
                GameField.EarnedFromDLC or 
                GameField.AvailableFromDLC =>
                    12,
                GameField.Difficult =>
                    20,
                GameField.Image or
                GameField.Source => 
                    8,
                GameField.Edition => 
                    4,
                GameField.Region or
                GameField.Platform or 
                GameField.Format or 
                GameField.CriticScore or 
                GameField.Series or 
                GameField.EarnedSilver or 
                GameField.EarnedBronze or 
                GameField.EarnedNet or 
                GameField.AvailableSilver or 
                GameField.AvailableBronze or 
                GameField.AvailableNet or 
                GameField.CompleteTime or
                GameField.Genre or 
                GameField.Publisher or 
                GameField.Year or 
                GameField.Pegi => 
                    2,
                GameField.EarnedGold or 
                GameField.AvailableGold => -
                    4,
                _ => 
                    0,
            };
    }
}
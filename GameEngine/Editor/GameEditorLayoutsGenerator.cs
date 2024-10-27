using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Editor;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Editor
{
    public class GameEditorLayoutsGenerator: EditorLayoutsGenerator<GameField, Game, GameFieldGroup>
    {
        public GameEditorLayoutsGenerator(FieldGroupFrames<GameField, GameFieldGroup> groupFrames,
            ControlLayouter<GameField, Game> layouter) : base(groupFrames, layouter) { }


        public override List<GameField> ControlsWithoutLabel() =>
            new()
            {
                GameField.AvailablePlatinum,
                GameField.Image,
                GameField.RelatedGames,
                GameField.Dlcs,
                GameField.Links,
                GameField.ReleasePlatforms,
                GameField.Verified,
                GameField.Licensed,
                GameField.SinglePlayer,
                GameField.CoachMultiplayer,
                GameField.OnlineMultiplayer,
                GameField.Tags,
                GameField.Trophyset
            };

        public override List<GameField> AutoSizeFields() =>
            new()
            {
                GameField.AvailablePlatinum,
                GameField.Verified,
                GameField.Licensed,
                GameField.SinglePlayer,
                GameField.CoachMultiplayer,
                GameField.OnlineMultiplayer
            };

        public override List<GameField> FillDockFields() =>
            new()
            {
                GameField.RelatedGames,
                GameField.Dlcs,
                GameField.Tags,
                GameField.Links,
                GameField.Installations,
                GameField.EmulatorROMs
            };

        public override List<GameField> OffsettingFields() =>
            new()
            {
                GameField.Image,
                GameField.Edition,
                GameField.Source,
                GameField.Platform,
                GameField.Format,
                GameField.CriticScore,
                GameField.Serieses,
                GameField.Difficult,
                GameField.CompleteTime,
                GameField.Genre,
                GameField.Developer,
                GameField.Publisher,
                GameField.Pegi,
                GameField.Region,
                GameField.SinglePlayer,
                GameField.CoachMultiplayer,
                GameField.MaximumPlayers,
                GameField.OnlineMultiplayer,
                GameField.Devices
            };


        public override AnchorStyles Anchors(GameField field)
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

        public override Color BackColor(GameField field) => 
            field switch
            {
                GameField.Image =>
                    GroupFrames[GameFieldGroup.Base].BaseColor,
                GameField.Verified or
                GameField.Licensed or
                GameField.SinglePlayer or
                GameField.CoachMultiplayer or
                GameField.OnlineMultiplayer or
                GameField.ReleasePlatforms =>
                    Color.Transparent,
                _ =>
                    Color.FromArgb(250, 250, 250),
            };

        public override int Top(GameField field) => 
            field switch
            {
                GameField.Licensed =>
                    Layouter[GameField.Name]!.Top,
                GameField.Trophyset =>
                    0,
                GameField.Owner =>
                    Layouter[GameField.Image]!.Top,
                GameField.Verified =>
                    (Parent(field).Height - Height(field)) / 2,
                GameField.EmulatorType => 
                    Layouter[GameField.Region]!.Top,
                GameField.Language or
                GameField.Code =>
                    Layouter[GameField.Region]!.Top,
                _ =>
                    8
            };

        public override int Left(GameField field) => 
            field switch
            {
                GameField.Image or 
                GameField.Verified =>
                    8,
                GameField.SinglePlayer or
                GameField.CoachMultiplayer or
                GameField.OnlineMultiplayer => 
                    6,
                GameField.MaximumPlayers =>
                    164,
                GameField.Licensed =>
                    390,
                GameField.Owner or
                GameField.Source or 
                GameField.Platform or 
                GameField.Format => 
                    302,
                GameField.Name or 
                GameField.Edition or 
                GameField.Serieses or
                GameField.Region or
                GameField.EmulatorType =>
                    68,
                GameField.ScreenView or
                GameField.Genre or
                GameField.Devices =>
                    66,
                GameField.Language =>
                    206,
                GameField.Code =>
                    348,
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

        public override int Height(GameField field) =>
            field switch
            {
                GameField.Image =>
                    110,
                GameField.Name or
                GameField.Devices =>
                    42,
                GameField.Verified or
                GameField.SinglePlayer or
                GameField.CoachMultiplayer or
                GameField.OnlineMultiplayer or
                GameField.Licensed =>
                    20,
                GameField.Serieses or
                GameField.CriticScore or
                GameField.MaximumPlayers or
                GameField.Code =>
                    26,
                _ =>
                    base.Height(field)
            };

        public override int Width(GameField field) => 
            field switch
            {
                GameField.Image => 
                    200,
                GameField.Owner or
                GameField.Source or 
                GameField.Platform or 
                GameField.Format =>
                    140,
                GameField.ScreenView => 
                    95,
                GameField.Name =>
                    272,
                GameField.Edition or 
                GameField.Serieses or 
                GameField.EmulatorType =>
                    374,
                GameField.Developer or 
                GameField.Publisher => 
                    362,
                GameField.MaximumPlayers or
                GameField.Year or
                GameField.Pegi or
                GameField.CriticScore =>
                    64,
                GameField.Region =>
                    52,
                GameField.Language
                    => 80,
                GameField.Code =>
                    94,
                GameField.Genre or
                GameField.Devices => 
                    210,
                GameField.ReleasePlatforms => 
                    600,
                _ => 
                    0,
            };

        public override int Offset(GameField field) => 
            field switch
            {
                GameField.Image or
                GameField.SinglePlayer => 
                    8,
                GameField.Serieses or
                GameField.Devices or
                GameField.Developer =>
                    4,
                GameField.CoachMultiplayer or
                GameField.OnlineMultiplayer or
                GameField.MaximumPlayers =>
                    6,
                GameField.Edition or
                GameField.Platform or
                GameField.Region or
                GameField.Source or
                GameField.Format or 
                GameField.CriticScore or
                GameField.Genre or 
                GameField.Publisher or 
                GameField.Pegi => 
                    2,
                _ => 
                    0,
            };

        public override List<GameField> TitleAccordionFields() => new()
        {
            GameField.Name,
            GameField.Platform,
            GameField.Source,
            GameField.Region
        };

        public override GameField BackColorField => GameField.Source;
    }
}
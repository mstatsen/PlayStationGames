using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Editor;
using OxLibrary;
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

        public override OxWidth Top(GameField field) => 
            field switch
            {
                GameField.Licensed =>
                    Layouter[GameField.Name]!.Top,
                GameField.Trophyset =>
                    OxWh.W0,
                GameField.Owner =>
                    Layouter[GameField.Image]!.Top,
                GameField.Verified =>
                    OxWh.Div(
                        OxWh.Sub(
                            Parent(field).Height, 
                            Height(field)
                        ),
                        OxWh.W2
                    ),
                GameField.EmulatorType => 
                    Layouter[GameField.Region]!.Top,
                GameField.Language or
                GameField.Code =>
                    Layouter[GameField.Region]!.Top,
                _ =>
                    OxWh.W8
            };

        public override OxWidth Left(GameField field) => 
            field switch
            {
                GameField.Image or 
                GameField.Verified =>
                    OxWh.W8,
                GameField.SinglePlayer or
                GameField.CoachMultiplayer or
                GameField.OnlineMultiplayer =>
                    OxWh.W6,
                GameField.MaximumPlayers =>
                    OxWh.W164,
                GameField.Licensed =>
                    OxWh.W390,
                GameField.Owner or
                GameField.Source or 
                GameField.Platform or 
                GameField.Format =>
                    OxWh.W302,
                GameField.Name or 
                GameField.Edition or 
                GameField.Serieses or
                GameField.Region or
                GameField.EmulatorType =>
                    OxWh.W68,
                GameField.ScreenView or
                GameField.Genre or
                GameField.Devices =>
                    OxWh.W66,
                GameField.Language =>
                    OxWh.W206,
                GameField.Code =>
                    OxWh.W348,
                GameField.Developer or 
                GameField.Publisher or 
                GameField.Year or 
                GameField.Pegi or 
                GameField.CriticScore =>
                    OxWh.W84,
                GameField.ReleasePlatforms =>
                    OxWh.W455,
                _ =>
                    OxWh.W0,
            };

        public override OxWidth Height(GameField field) =>
            field switch
            {
                GameField.Image =>
                    OxWh.W110,
                GameField.Name or
                GameField.Devices =>
                    OxWh.W42,
                GameField.Verified or
                GameField.SinglePlayer or
                GameField.CoachMultiplayer or
                GameField.OnlineMultiplayer or
                GameField.Licensed =>
                    OxWh.W20,
                GameField.Serieses or
                GameField.CriticScore or
                GameField.MaximumPlayers or
                GameField.Code =>
                    OxWh.W26,
                _ =>
                    base.Height(field)
            };

        public override OxWidth Width(GameField field) => 
            field switch
            {
                GameField.Image =>
                    OxWh.W200,
                GameField.Owner or
                GameField.Source or 
                GameField.Platform or 
                GameField.Format =>
                    OxWh.W140,
                GameField.ScreenView =>
                    OxWh.W95,
                GameField.Name =>
                    OxWh.W272,
                GameField.Edition or 
                GameField.Serieses or 
                GameField.EmulatorType =>
                    OxWh.W374,
                GameField.Developer or 
                GameField.Publisher =>
                    OxWh.W362,
                GameField.MaximumPlayers or
                GameField.Year or
                GameField.Pegi or
                GameField.CriticScore =>
                    OxWh.W64,
                GameField.Region =>
                    OxWh.W52,
                GameField.Language
                    => OxWh.W80,
                GameField.Code =>
                    OxWh.W94,
                GameField.Genre or
                GameField.Devices =>
                    OxWh.W210,
                GameField.ReleasePlatforms =>
                    OxWh.W600,
                _ =>
                    OxWh.W0,
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
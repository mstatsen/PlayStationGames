using OxDAOEngine.Data.Fields;
using OxLibrary;

namespace PlayStationGames.GameEngine.Data.Fields
{
    public class GameFieldGroupHelper : FieldGroupHelper<GameField, GameFieldGroup>
    {
        public override GameFieldGroup EmptyValue() => 
            GameFieldGroup.Base;

        public override List<GameFieldGroup> EditedList() => 
            new()
            {
                GameFieldGroup.Emulator,GameFieldGroup.RelatedGames,
                GameFieldGroup.DLC,
                GameFieldGroup.Base,

                GameFieldGroup.Links,
                GameFieldGroup.Trophyset,

                GameFieldGroup.Tags,
                GameFieldGroup.Installations,
                GameFieldGroup.Genre,

                GameFieldGroup.ReleaseBase
            };

        public override string GetName(GameFieldGroup value) =>
            value switch
            {
                GameFieldGroup.Base => "Game",
                GameFieldGroup.Image => "Image",
                GameFieldGroup.RelatedGames => "Related Games",
                GameFieldGroup.DLC => "DLCs",
                GameFieldGroup.Genre => "Genre",
                GameFieldGroup.Installations => "Installations",
                GameFieldGroup.Links => "Links",
                GameFieldGroup.Trophyset => "Trophyset",
                GameFieldGroup.ReleaseBase => "Release",
                GameFieldGroup.ReleasePlatforms => "Platforms",
                GameFieldGroup.System => "System",
                GameFieldGroup.Tags => "Tags",
                GameFieldGroup.Emulator => "ROMs",
                _ => string.Empty,
            };

        public override GameFieldGroup Group(GameField field) => 
            field switch
            {
                GameField.Trophyset => 
                    GameFieldGroup.Trophyset,
                GameField.Developer or 
                GameField.Publisher or 
                GameField.Year or 
                GameField.Pegi or 
                GameField.CriticScore => 
                    GameFieldGroup.ReleaseBase,
                GameField.ReleasePlatforms => 
                    GameFieldGroup.ReleasePlatforms,
                GameField.Installations => 
                    GameFieldGroup.Installations,
                GameField.Genre or 
                GameField.ScreenView or
                GameField.SinglePlayer or
                GameField.CoachMultiplayer or
                GameField.MaximumPlayers or
                GameField.OnlineMultiplayer or
                GameField.Devices => 
                    GameFieldGroup.Genre,
                GameField.Dlcs => 
                    GameFieldGroup.DLC,
                GameField.Links => 
                    GameFieldGroup.Links,
                GameField.RelatedGames => 
                    GameFieldGroup.RelatedGames,
                GameField.Tags => 
                    GameFieldGroup.Tags,
                GameField.Field or 
                GameField.Verified => 
                    GameFieldGroup.System,
                GameField.EmulatorROMs => 
                    GameFieldGroup.Emulator,
                _ => 
                    GameFieldGroup.Base,
            };

        public override GameFieldGroup EditedGroup(GameField field) =>
            field is GameField.ReleasePlatforms
                ? GameFieldGroup.ReleaseBase
                : base.EditedGroup(field);

        public override OxWidth GroupWidth(GameFieldGroup group) => 
            group switch
            {
                GameFieldGroup.Base or 
                GameFieldGroup.DLC or 
                GameFieldGroup.RelatedGames or
                GameFieldGroup.Emulator =>
                    OxWh.W460,
                GameFieldGroup.Links or
                GameFieldGroup.Trophyset =>
                    OxWh.W292,
                GameFieldGroup.Genre or
                GameFieldGroup.Installations or 
                GameFieldGroup.Tags => 
                    OxWh.W294,
                GameFieldGroup.ReleaseBase =>
                    OxWh.W200,
                _ =>
                    OxWh.W0,
            };

        public override OxDock GroupDock(GameFieldGroup group) =>
            group is GameFieldGroup.ReleaseBase 
                ? OxDock.Bottom 
                : base.GroupDock(group);

        public override bool IsCalcedHeightGroup(GameFieldGroup group) =>
            new List<GameFieldGroup>
            {
                GameFieldGroup.Base,
                GameFieldGroup.Trophyset,
                GameFieldGroup.Genre,
                GameFieldGroup.ReleaseBase
            }.Contains(group);


        public override OxWidth DefaultGroupHeight(GameFieldGroup group) => 
            group switch
            {
                GameFieldGroup.Emulator =>
                    OxWh.W200,
                GameFieldGroup.Tags =>
                    OxWh.W69,
                _ =>
                    OxWh.W84,
            };

        public List<GameFieldGroup> VerifiedGroups =
            new()
            {
                GameFieldGroup.Base,
                GameFieldGroup.DLC,
                GameFieldGroup.Genre,
                GameFieldGroup.Links,
                GameFieldGroup.ReleaseBase,
                GameFieldGroup.Trophyset,
                GameFieldGroup.Emulator
            };
    }
}
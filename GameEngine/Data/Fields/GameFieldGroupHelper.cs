using OxDAOEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Fields
{
    public class GameFieldGroupHelper : FieldGroupHelper<GameField, GameFieldGroup>
    {
        public override GameFieldGroup EmptyValue() => 
            GameFieldGroup.Base;

        public override List<GameFieldGroup> EditedList() => 
            new()
            {
                GameFieldGroup.Emulator,
                GameFieldGroup.RelatedGames,
                GameFieldGroup.DLC,
                GameFieldGroup.Base,

                GameFieldGroup.Links,
                GameFieldGroup.Trophyset,

                GameFieldGroup.Tags,
                GameFieldGroup.Installations,
                GameFieldGroup.Devices,
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
                GameFieldGroup.Devices => "Used devices",
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
                GameField.OnlineMultiplayer => 
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
                GameField.Devices => 
                    GameFieldGroup.Devices,
                _ => 
                    GameFieldGroup.Base,
            };

        public override GameFieldGroup EditedGroup(GameField field) =>
            field == GameField.ReleasePlatforms
                ? GameFieldGroup.ReleaseBase
                : base.EditedGroup(field);

        public override int GroupWidth(GameFieldGroup group) => 
            group switch
            {
                GameFieldGroup.Base or 
                GameFieldGroup.DLC or 
                GameFieldGroup.RelatedGames or
                GameFieldGroup.Emulator =>
                    460,
                GameFieldGroup.Links or
                GameFieldGroup.Trophyset =>
                    292,
                GameFieldGroup.Genre or
                GameFieldGroup.Devices or
                GameFieldGroup.Installations or 
                GameFieldGroup.Tags => 
                    294,
                GameFieldGroup.ReleaseBase => 
                    200,
                _ => 
                    0,
            };

        public override DockStyle GroupDock(GameFieldGroup group) =>
            group == GameFieldGroup.ReleaseBase ? DockStyle.Bottom : base.GroupDock(group);

        public override bool IsCalcedHeightGroup(GameFieldGroup group) =>
            new List<GameFieldGroup>
            {
                GameFieldGroup.Base,
                GameFieldGroup.Trophyset,
                GameFieldGroup.Genre,
                GameFieldGroup.ReleaseBase
            }.Contains(group);


        public override int DefaultGroupHeight(GameFieldGroup group) => 
            group switch
            {
                GameFieldGroup.Emulator =>
                    200,
                GameFieldGroup.Tags =>
                    69,
                GameFieldGroup.Devices =>
                    56,
                _ =>
                    84,
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
                GameFieldGroup.Devices,
                GameFieldGroup.Emulator
            };
    }
}
using OxXMLEngine.Data.Fields;

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
                GameFieldGroup.DLC,
                GameFieldGroup.RelatedGames,
                GameFieldGroup.Base,

                GameFieldGroup.Genre,
                GameFieldGroup.Trophyset,

                GameFieldGroup.Tags,
                GameFieldGroup.Link,
                GameFieldGroup.Installations,
                GameFieldGroup.GameMode,

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
                GameFieldGroup.GameMode => "Play Modes",
                GameFieldGroup.Installations => "Installations",
                GameFieldGroup.Link => "Links",
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
                GameField.Difficult or
                GameField.CompleteTime or
                GameField.EarnedPlatinum or 
                GameField.EarnedGold or 
                GameField.EarnedSilver or 
                GameField.EarnedBronze or 
                GameField.EarnedFromDLC or 
                GameField.EarnedNet or 
                GameField.AvailablePlatinum or 
                GameField.AvailableGold or 
                GameField.AvailableSilver or 
                GameField.AvailableBronze or 
                GameField.AvailableFromDLC or 
                GameField.AvailableNet or 
                GameField.TrophysetAccess => 
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
                GameField.ScreenView => 
                    GameFieldGroup.Genre,
                GameField.GameModes => 
                    GameFieldGroup.GameMode,
                GameField.Dlcs => 
                    GameFieldGroup.DLC,
                GameField.Links => 
                    GameFieldGroup.Link,
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
                    448,
                GameFieldGroup.Trophyset or 
                GameFieldGroup.Genre =>
                    252,
                GameFieldGroup.GameMode or
                GameFieldGroup.Link or 
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
                _ =>
                    84,
            };
    }
}
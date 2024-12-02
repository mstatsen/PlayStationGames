using OxDAOEngine.Data.Types;
using OxLibrary;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class TrophyTypeHelper : AbstractStyledTypeHelper<TrophyType>
    {
        public override TrophyType EmptyValue() => TrophyType.Platinum;

        public override string GetName(TrophyType value) => 
            value switch
            {
                TrophyType.Platinum => "Platinum",
                TrophyType.Gold => "Gold",
                TrophyType.Silver => "Silver",
                TrophyType.Bronze => "Bronze",
                _ => string.Empty,
            };

        public List<TrophyType> CountingTrophies = new()
        { 
            TrophyType.Gold,
            TrophyType.Silver,
            TrophyType.Bronze,
        };

        private static readonly OxSize iconSize = new(OxWh.W24, OxWh.W24);

        private readonly Dictionary<TrophyType, Bitmap> iconsDictionary = new()
        {
            [TrophyType.Platinum] = OxImageBoxer.BoxingImage(OxIcons.PlatinumTrophy, iconSize),
            [TrophyType.Gold] = OxImageBoxer.BoxingImage(OxIcons.GoldTrophy, iconSize),
            [TrophyType.Silver] = OxImageBoxer.BoxingImage(OxIcons.SilverTrophy, iconSize),
            [TrophyType.Bronze] = OxImageBoxer.BoxingImage(OxIcons.BronzeTrophy, iconSize)
        };

        public Bitmap? Icon(TrophyType type) =>
            iconsDictionary.TryGetValue(type, out Bitmap? bitmap)
                ? bitmap
                : null;

        public override Color GetBaseColor(TrophyType value) => OxStyles.CardColor;

        public override Color GetFontColor(TrophyType value) =>
            value switch
            {
                TrophyType.Platinum => Color.FromArgb(139,138,136),
                TrophyType.Gold => Color.FromArgb(212,154,0),
                TrophyType.Silver => Color.FromArgb(126,126,126),
                TrophyType.Bronze => Color.FromArgb(105,102,0),
                _ => Color.Black,
            };

        public int Points(TrophyType type) =>
            type switch
            {
                TrophyType.Platinum => 300,
                TrophyType.Gold => 90,
                TrophyType.Silver => 30,
                TrophyType.Bronze => 15,
                _ => 0,
            };

        public int OldPoints(TrophyType type) =>
            type switch
            {
                TrophyType.Platinum => 180,
                _ => Points(type),
            };

        public GameField Field(TrophyType trophyType) =>
            trophyType switch
            {
                TrophyType.Platinum => GameField.AvailablePlatinum,
                TrophyType.Gold => GameField.AvailableGold,
                TrophyType.Silver => GameField.AvailableSilver,
                TrophyType.Bronze => GameField.AvailableBronze,
                _ => default!,
            };

        public AccountField FieldForAccount(TrophyType trophyType) =>
            trophyType switch
            {
                TrophyType.Platinum => AccountField.PlatinumCount,
                TrophyType.Gold => AccountField.GoldCount,
                TrophyType.Silver => AccountField.SilverCount,
                TrophyType.Bronze => AccountField.BronzeCount,
                _ => default!,
            };
    }
}
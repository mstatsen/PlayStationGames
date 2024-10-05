using OxDAOEngine.Data.Types;
using OxLibrary;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class TrophyTypeHelper : AbstractTypeHelper<TrophyType>
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

        private static readonly Size iconSize = new(24, 24);

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
    }
}
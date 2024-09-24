using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class GameRegionHelper : FieldAccordingHelper<GameField, GameRegion>
    {
        public override GameRegion EmptyValue() => 
            GameRegion.Europe;

        public override string GetShortName(GameRegion value) =>
            value switch
            {
                GameRegion.World => "WD",
                GameRegion.America => "US",
                GameRegion.Europe => "EU",
                GameRegion.Asia => "Asia",
                GameRegion.Australia => "AU",
                GameRegion.Russia => "RU",
                GameRegion.China => "CH",
                _ => string.Empty
            };

        public override string GetName(GameRegion value) =>
            ShortName(value);

        public override string GetFullName(GameRegion value) =>
            value switch
            {
                GameRegion.World => "Worldwide",
                GameRegion.America => "United States\nCanada\nU.S.Territories\nLatin America",
                GameRegion.Europe => "European Union\nJapan\nMiddle East\nEgypt\nSouth Africa\nGreenland\nFrench Territories",
                GameRegion.Asia => "Taiwan\nSouth Korea\nThe Phillippines\nIndonesia\nHong Kong",
                GameRegion.Australia => "Australia\nNew Zeland\nPacific islands\nBrazil",
                GameRegion.Russia => "Rusia\nEastern Europe\nPakistan\nIndia\nThe Majority of Africa\nNorth Kkorea\nMongolia",
                GameRegion.China => "Mainland China",
                _ => string.Empty
            };

        public override bool UseShortNameForControl => true;
        public override bool UseToolTipForControl => true;
    }
}
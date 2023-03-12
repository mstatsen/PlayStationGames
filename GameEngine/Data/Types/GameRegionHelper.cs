using OxXMLEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class GameRegionHelper
        : FieldAccordingHelper<GameField, GameRegion>
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
                GameRegion.World => "WORLDWIDE",
                GameRegion.America => "UNITED STATES, CANADA, U.S. TERRITORIES, LATIN AMERICA",
                GameRegion.Europe => "EUROPEAN UNION, JAPAN, MIDDLE EAST, EGYPT, SOUTH AFRICA, GREENLAND, FRENCH TERRITORIES",
                GameRegion.Asia => "TAIWAN, SOUTH KOREA, THE PHILIPPINES, INDONESIA, HONG KONG",
                GameRegion.Australia => "AUSTRALIA, NEW ZEALAND, PACIFIC ISLANDS, BRAZIL",
                GameRegion.Russia => "RUSSIA, EASTERN EUROPE, PAKISTAN, INDIA, THE MAJORITY OF AFRICA, NORTH KOREA, MONGOLIA",
                GameRegion.China => "MAINLAND CHINA",
                _ => string.Empty
            };
    }
}
using OxXMLEngine;
using OxXMLEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class DifficultRankHelper : AbstractTypeHelper<DifficultRank>
    {
        public override string GetShortName(DifficultRank value) => 
            value switch
            {
                DifficultRank.Unknown => Consts.Short_Unknown,
                _ => ((int)value).ToString(),
            };

        public override string GetName(DifficultRank value) => 
            GetShortName(value);

        public override DifficultRank EmptyValue() => 
            DifficultRank.Unknown;
    }
}
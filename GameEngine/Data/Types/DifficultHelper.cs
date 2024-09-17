using OxDAOEngine;
using OxDAOEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class DifficultHelper : AbstractTypeHelper<Difficult>
    {
        public override string GetShortName(Difficult value) => 
            value switch
            {
                Difficult.Unknown => Consts.Short_Unknown,
                _ => ((int)value).ToString(),
            };

        public override string GetName(Difficult value) => 
            GetShortName(value);

        public override Difficult EmptyValue() => 
            Difficult.Unknown;
    }
}
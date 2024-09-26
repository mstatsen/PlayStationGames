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

        public override string GetFullName(Difficult value) =>
            value switch
            {
                Difficult.One => "Put down the controller",
                Difficult.Two => "Press X to win",
                Difficult.Three => "Quite simply",
                Difficult.Four => "Simply",
                Difficult.Five => "Medium",
                Difficult.Six => "It will make you sweat",
                Difficult.Seven => "Hard",
                Difficult.Eight => "Very hard",
                Difficult.Nine => "Hardcore",
                Difficult.Ten => "Uber hardcore",
                _ => "Unknown"
            };

        public override bool UseToolTipForControl => true;
    }
}
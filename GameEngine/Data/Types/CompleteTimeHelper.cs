using OxDAOEngine;
using OxDAOEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class CompleteTimeHelper
        : AbstractTypeHelper<CompleteTime>
    {
        public override CompleteTime EmptyValue() => CompleteTime.ctUnknown;

        public override string GetName(CompleteTime value) => 
            value switch
            {
                CompleteTime.ct5 => "0 - 9",
                CompleteTime.ct15 => "10 - 24",
                CompleteTime.ct35 => "25 - 49",
                CompleteTime.ct65 => "50 - 74",
                CompleteTime.ct90 => "75 - 99",
                CompleteTime.ct150 => "100 - 199",
                CompleteTime.ct275 => "200 - 349",
                CompleteTime.ct425 => "350 - 499",
                CompleteTime.ct750 => "500 - 999",
                CompleteTime.ct1000 => "1000+",
                _ => Consts.Short_Unknown,
            };

        public override string GetFullName(CompleteTime value) =>
            value switch
            {
                CompleteTime.ctUnknown =>
                    "Unknown time",
                _ => $"roughy {GetShortName(value)} hours"
            };

        public override string GetShortName(CompleteTime value) =>
            ((int)value).ToString();

        public override bool UseToolTipForControl => true;
    }
}
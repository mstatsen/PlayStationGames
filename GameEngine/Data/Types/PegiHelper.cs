using OxLibrary;
using OxDAOEngine;
using OxDAOEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class PegiHelper : AbstractStyledTypeHelper<Pegi>
    {
        public override string GetName(Pegi value) => 
            value switch
            {
                Pegi.Zero => "0+",
                Pegi.Six => "6+",
                Pegi.Twelve => "12+",
                Pegi.Sixteen => "16+",
                Pegi.Eighteen => "18+",
                _ => Consts.Short_Unknown,
            };

        public override string GetShortName(Pegi value) =>
            ((int)value).ToString();

        public override Color GetBaseColor(Pegi value) => Styles.CardColor;

        public override Color GetFontColor(Pegi value) => 
            value switch
            {
                Pegi.Zero => Color.Blue,
                Pegi.Six => Color.Green,
                Pegi.Twelve => Color.Brown,
                Pegi.Sixteen => Color.Orange,
                Pegi.Eighteen => Color.DarkGray,
                _ => Color.DimGray,
            };

        public override Pegi EmptyValue() =>
            Pegi.Unknown;
    }
}
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
                Pegi.Three => "3+",
                Pegi.Seven => "7+",
                Pegi.Twelve => "12+",
                Pegi.Sixteen => "16+",
                Pegi.Eighteen => "18+",
                _ => Consts.Short_Unknown,
            };

        public override string GetShortName(Pegi value) =>
            ((int)value).ToString();

        public override Color GetBaseColor(Pegi value) => 
            new OxColorHelper(FontColor(value)).Lighter(8);

        public override Color GetFontColor(Pegi value) => 
            value switch
            {
                Pegi.Three => Color.Green,
                Pegi.Seven => Color.LimeGreen,
                Pegi.Twelve => Color.LightSalmon,
                Pegi.Sixteen => Color.DarkOrange,
                Pegi.Eighteen => Color.Crimson,
                _ => Color.DimGray,
            };

        public override Pegi EmptyValue() =>
            Pegi.Unknown;
    }
}
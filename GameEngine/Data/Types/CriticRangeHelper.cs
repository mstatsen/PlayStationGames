using OxLibrary;
using OxDAOEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types;

public class CriticRangeHelper : AbstractStyledTypeHelper<CriticRange>
{
    public override CriticRange EmptyValue() =>
        default!;

    public override Color GetBaseColor(CriticRange value) => 
        new OxColorHelper(FontColor(value)).Lighter(6);

    public override Color GetFontColor(CriticRange value) => 
        value switch
        {
            CriticRange.Bad => Color.OrangeRed,
            CriticRange.Medium => Color.SandyBrown,
            CriticRange.Good => Color.ForestGreen,
            CriticRange.Best => Color.Blue,
            _ => Color.DimGray,
        };

    public Color FontColor(int CriticScore) =>
        FontColor(Range(CriticScore));

    public override string GetName(CriticRange value) =>
        value switch
        {
            CriticRange.Bad => "Bad",
            CriticRange.Medium => "Medium",
            CriticRange.Good => "Good",
            CriticRange.Best => "Best",
            _ => "Unknown",
        };

    public CriticRange Range(int score) => 
        score switch
        {
            < 0 => CriticRange.Unknown,
            < 30 => CriticRange.Bad,
            < 60 => CriticRange.Medium,
            < 90 => CriticRange.Good,
            _ => CriticRange.Best
        };
}
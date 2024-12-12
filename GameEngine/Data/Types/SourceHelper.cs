using OxDAOEngine.Data.Types;
using OxLibrary;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types;

public class SourceHelper : FieldAccordingHelper<GameField, Source>
{
    public override string GetName(Source value) => 
        value switch
        {
            Source.PSN => "PSN",
            Source.PSPlus => "PSPlus",
            Source.PlayAtHome => "PlayAtHome",
            Source.Torrent => "Torrent",
            Source.PKGj => "PKGj",
            Source.Physical => "Physical",
            Source.Lost => "Lost",
            _ => "Other"
        };

    public override string GetShortName(Source value) => 
        value switch
        {
            Source.PSN => string.Empty,
            Source.PSPlus => "+",
            Source.PlayAtHome => "P",
            Source.Torrent => "T",
            Source.PKGj => "V",
            Source.Physical => "O",
            Source.Lost => "-",
            _ => "?",
        };

    private readonly Dictionary<Source, Color> ForeColors = new();

    public override Color GetFontColor(Source value)
    {
        if (!ForeColors.ContainsKey(value))
            ForeColors.Add(value, new OxColorHelper(BaseColor(value)).Darker(11));

        return ForeColors[value];
    }

    private readonly Dictionary<Source, Color> BaseColors = new();

    public override Color GetBaseColor(Source value)
    {
        if (!BaseColors.ContainsKey(value))
            BaseColors.Add(value, 
                value switch
                {
                    Source.PSN => Color.FromArgb(245, 251, 232),
                    Source.PSPlus => Color.FromArgb(255, 240, 224),
                    Source.PlayAtHome => Color.FromArgb(240, 255, 255),
                    Source.Torrent => Color.FromArgb(250, 240, 255),
                    Source.PKGj => Color.FromArgb(245, 235, 250),
                    Source.Physical => Color.FromArgb(255, 250, 210),
                    Source.Lost => Color.FromArgb(246, 246, 246),
                    _ => Color.White,
                }
            );

        return BaseColors[value];
    }

    public List<Source> ByLicense(bool licensed)
    {
        List<Source> result = new();

        if (licensed)
        {
            result.Add(Source.PSN);
            result.Add(Source.PSPlus);
            result.Add(Source.PlayAtHome);
        }
        else
        {
            result.Add(Source.Torrent);
            result.Add(Source.PKGj);
            
        }

        result.Add(Source.Physical);
        result.Add(Source.Lost);
        return result;
    }

    public List<Source> ByPlatform(PlatformType platform)
    {
        List<Source> result = new();

        foreach (Source item in Enum.GetValues(typeof(Source)))
        {
            if (IsDigital(item) &&
                !TypeHelper.Helper<PlatformTypeHelper>().StoragesSupport(platform))
                continue;

            result.Add(item);
        }

        if (platform is PlatformType.PSVita)
            result.Remove(Source.Torrent);
        else
            result.Remove(Source.PKGj);

        return result;
    }

    public override Source EmptyValue() => 
        Source.Physical;

    public override Source DefaultValue() =>
        Source.PSN;

    public override List<Source> DependedList(GameField field, object value)
    {
        switch (field)
        {
            case GameField.Licensed:
                if (value is bool licensed)
                    return ByLicense(licensed);

                break;
            case GameField.Platform:
                if (value is PlatformType platform)
                    return ByPlatform(platform);

                break;
        }

        return base.DependedList(field, value);
    }

    public bool IsPSN(Source value) => 
        value switch
        {
            Source.PSN or 
            Source.PSPlus or 
            Source.PlayAtHome => true,
            _ => false,
        };

    public bool IsDigital(Source value) => 
        value switch
        {
            Source.PSN or 
            Source.PSPlus or 
            Source.PlayAtHome or
            Source.Torrent or
            Source.PKGj => true,
            _ => false,
        };

    public bool InstallationsSupport(Source source) =>
        source is not Source.Lost;
}
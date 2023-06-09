﻿using OxXMLEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class SourceHelper : FieldAccordingHelper<GameField, Source>
    {
        public override string GetName(Source value) => 
            value switch
            {
                Source.PSN => "PSN",
                Source.PSPlus => "PSPlus",
                Source.PlayAtHome => "PlayAtHome",
                Source.Torrent => "Torrent",
                Source.Physical => "Physical",
                Source.Lost => "Lost",
                _ => "Other",
            };

        public override string GetShortName(Source value) => 
            value switch
            {
                Source.PSN => string.Empty,
                Source.PSPlus => "+",
                Source.PlayAtHome => "P",
                Source.Torrent => "T",
                Source.Physical => "O",
                Source.Lost => "-",
                _ => "?",
            };

        public override Color GetBaseColor(Source value) => 
            value switch
            {
                Source.PSN => Color.FromArgb(245, 251, 232),
                Source.PSPlus => Color.FromArgb(255, 240, 224),
                Source.PlayAtHome => Color.FromArgb(240, 255, 255),
                Source.Torrent => Color.FromArgb(250, 240, 255),
                Source.Physical => Color.FromArgb(255, 250, 210),
                Source.Lost => Color.FromArgb(246, 246, 246),
                _ => Color.White,
            };

        public List<Source> ByLicense(bool licensed)
        {
            List<Source> result = new();

            if (licensed)
            {
                result.Add(Source.PSN);
                result.Add(Source.PSPlus);
                result.Add(Source.PlayAtHome);
                result.Add(Source.Physical);
                result.Add(Source.Lost);
                result.Add(Source.Other);
            }
            else
            {
                result.Add(Source.Torrent);
                result.Add(Source.Physical);
                result.Add(Source.Lost);
                result.Add(Source.Other);
            }

            return result;
        }

        public override Source EmptyValue() => 
            Source.Other;

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
                Source.Torrent => true,
                _ => false,
            };

        public bool InstallationsSupport(Source source) =>
            source != Source.Lost;
    }
}
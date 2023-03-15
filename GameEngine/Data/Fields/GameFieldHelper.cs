using OxXMLEngine.Data.Fields;
using OxXMLEngine.Data.Filter;
using OxXMLEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Fields
{
    internal class GameFieldsFillingDictionary : FieldsFillingDictionary<GameField> { };
    internal class GameFieldsVariantDictionary : Dictionary<FieldsVariant, GameFieldsFillingDictionary> { };

    public class GameFieldHelper : FieldHelper<GameField>
    {
        public override GameField EmptyValue() => GameField.Field;

        public override string ColumnCaption(GameField field) => 
            field switch
            {
                GameField.Image => string.Empty,
                GameField.TrophysetAccess => "Trophyset",
                GameField.ReleasePlatforms => "Released on",
                _ => base.ColumnCaption(field),
            };

        public override string GetName(GameField value) =>
            value switch
            {
                GameField.Id => "Game ID:",
                GameField.Name => "Name",
                GameField.Licensed => "Licensed",
                GameField.Favorite => "Favorite",
                GameField.TrophysetTODO => "Trophyset complete queue",
                GameField.Image => "Image",
                GameField.Edition => "Edition",
                GameField.Series => "Series",
                GameField.CriticScore => "Critic Score",
                GameField.Platform => "Platform",
                GameField.Format => "Format",
                GameField.EarnedPlatinum => "Platinum",
                GameField.EarnedGold => "Gold",
                GameField.EarnedSilver => "Silver",
                GameField.EarnedBronze => "Bronze",
                GameField.EarnedFromDLC => "From DLC",
                GameField.EarnedNet => "Net",
                GameField.AvailablePlatinum => "Platinum",
                GameField.AvailableGold => "Gold",
                GameField.AvailableSilver => "Silver",
                GameField.AvailableBronze => "Bronze",
                GameField.AvailableFromDLC => "From DLC",
                GameField.AvailableNet => "Net",
                GameField.TrophysetAccess => "Access",
                GameField.Source => "Source",
                GameField.Developer => "Developer",
                GameField.Publisher => "Publisher",
                GameField.Year => "Year",
                GameField.Pegi => "PEGI",
                GameField.ReleasePlatforms => "Released on",
                GameField.Installations => "Installations",
                GameField.Difficult => "Difficult",
                GameField.CompleteTime => "Full Time",
                GameField.Genre => "Genre",
                GameField.ScreenView => "Screen",
                GameField.GameModes => "Play Modes",
                GameField.Dlcs => "DLCs",
                GameField.Links => "Links",
                GameField.RelatedGames => "Related games",
                GameField.FullGenre => "Genre",
                GameField.Progress => "Progress",
                GameField.EarnedPoints => "Earned Points",
                GameField.EarnedPointsOld => "Earned Points (Old)",
                GameField.FullPlatinum => "Platinum",
                GameField.FullGold => "Gold",
                GameField.FullSilver => "Silver",
                GameField.FullBronze => "Bronze",
                GameField.FullFromDLC => "From DLC",
                GameField.FullNet => "Net",
                GameField.Status => "Status",
                GameField.Field => "Field",
                GameField.StrategeLink => "Stratege",
                GameField.PSNProfilesLink => "PSNProfiles",
                GameField.Verified => "All information verified",
                GameField.PlatformFamily => "Platform Family",
                GameField.EmulatorType => "Emulate",
                GameField.EmulatorROMs => "ROMs",
                GameField.Region => "Region",
                GameField.Language => "Language",
                GameField.Code => "Code",
                _ => string.Empty,
            };

        public override string GetFullName(GameField value) => 
            value switch
            {
                GameField.EarnedPlatinum => "Earned Platinum",
                GameField.EarnedGold => "Earned Gold",
                GameField.EarnedSilver => "Earned Silver",
                GameField.EarnedBronze => "Earned Bronze",
                GameField.EarnedFromDLC => "Earned From DLC",
                GameField.EarnedNet => "Earned Net",
                GameField.AvailablePlatinum => "Available Platinum",
                GameField.AvailableGold => "Available Gold",
                GameField.AvailableSilver => "Available Silver",
                GameField.AvailableBronze => "Available Bronze",
                GameField.AvailableFromDLC => "Available From DLC",
                GameField.AvailableNet => "Available Net",
                GameField.TrophysetAccess => "Trophyset Accessibility",
                GameField.EmulatorType => "Emulator Type",
                GameField.EmulatorROMs => "Emulator ROMs",
                _ => GetName(value),
            };

        private readonly GameFieldsVariantDictionary fieldDictionary = new()
        {
            [FieldsVariant.Category] = new GameFieldsFillingDictionary()
            {
                [FieldsFilling.Full] = new List<GameField>
                {
                    GameField.Licensed,
                    GameField.CompleteTime,
                    GameField.CriticScore,
                    GameField.Developer,
                    GameField.Difficult,
                    GameField.Edition,
                    GameField.Format,
                    GameField.Genre,
                    GameField.Pegi,
                    GameField.Platform,
                    GameField.Publisher,
                    GameField.Year,
                    GameField.ScreenView,
                    GameField.Series,
                    GameField.Source,
                    GameField.TrophysetAccess,
                    GameField.Favorite,
                    GameField.TrophysetTODO,
                    GameField.Region,
                    GameField.Language
                },
                [FieldsFilling.Default] = new List<GameField>
                {
                    GameField.Developer,
                    GameField.Publisher
                }
            },
            [FieldsVariant.Html] = new GameFieldsFillingDictionary()
            {
                [FieldsFilling.Full] = new List<GameField>
                {
                    GameField.Image,
                    GameField.Name,
                    GameField.Licensed,
                    GameField.Platform,
                    GameField.Source,
                    GameField.Format,
                    GameField.Status,
                    GameField.Edition,
                    GameField.Dlcs,
                    GameField.Series,
                    GameField.CriticScore,
                    GameField.FullGenre,
                    GameField.GameModes,
                    GameField.Pegi,
                    GameField.Year,
                    GameField.Developer,
                    GameField.Publisher,
                    GameField.ReleasePlatforms,
                    GameField.Difficult,
                    GameField.CompleteTime,
                    GameField.TrophysetAccess,
                    GameField.Progress,
                    GameField.FullPlatinum,
                    GameField.FullGold,
                    GameField.FullSilver,
                    GameField.FullBronze,
                    GameField.FullFromDLC,
                    GameField.FullNet,
                    GameField.EarnedPoints,
                    GameField.RelatedGames,
                    GameField.Links,
                    GameField.Verified,
                    GameField.Favorite,
                    GameField.TrophysetTODO,
                    GameField.EmulatorType,
                    GameField.EmulatorROMs,
                    GameField.Region,
                    GameField.Language,
                    GameField.Code
                },
                [FieldsFilling.Default] = new List<GameField>
                {
                    GameField.Image,
                    GameField.Name,
                    GameField.Licensed,
                    GameField.Region,
                    GameField.Platform,
                    GameField.Source,
                    GameField.Status,
                    GameField.Edition,
                    GameField.Dlcs,
                    GameField.CriticScore,
                    GameField.FullGenre,
                    GameField.GameModes,
                    GameField.Year,
                    GameField.Difficult,
                    GameField.CompleteTime,
                    GameField.TrophysetAccess,
                    GameField.Progress,
                    GameField.FullPlatinum,
                    GameField.FullGold,
                    GameField.FullSilver,
                    GameField.FullBronze,
                    GameField.FullFromDLC,
                    GameField.FullNet,
                    GameField.RelatedGames,
                    GameField.Links,
                    GameField.Code
                }
            },
            [FieldsVariant.Inline] = new GameFieldsFillingDictionary()
            {
                [FieldsFilling.Full] = new List<GameField>
                {
                    GameField.Licensed,
                    GameField.Platform,
                    GameField.Source,
                    GameField.Region,
                    GameField.Language,
                    GameField.Format,
                    GameField.Status,
                    GameField.Edition,
                    GameField.Dlcs,
                    GameField.Series,
                    GameField.CriticScore,
                    GameField.FullGenre,
                    GameField.GameModes,
                    GameField.Pegi,
                    GameField.Year,
                    GameField.Developer,
                    GameField.Publisher,
                    GameField.Difficult,
                    GameField.CompleteTime,
                    GameField.TrophysetAccess,
                    GameField.Progress,
                    GameField.FullPlatinum,
                    GameField.FullGold,
                    GameField.FullSilver,
                    GameField.FullBronze,
                    GameField.FullFromDLC,
                    GameField.FullNet,
                    GameField.EarnedPoints,
                    GameField.Verified,
                    GameField.Favorite,
                    GameField.TrophysetTODO,
                    GameField.Code
                },
                [FieldsFilling.Default] = new List<GameField>
                {
                    GameField.Platform,
                    GameField.Source
                }
            },
            [FieldsVariant.QuickFilter] = new GameFieldsFillingDictionary()
            {
                [FieldsFilling.Full] = new List<GameField>
                {
                    GameField.Licensed,
                    GameField.Source,
                    GameField.Status,
                    GameField.Progress,
                    GameField.Platform,
                    GameField.Format,
                    GameField.ScreenView,
                    GameField.Genre,
                    GameField.TrophysetAccess,
                    GameField.AvailablePlatinum,
                    GameField.Difficult,
                    GameField.CompleteTime,
                    GameField.Pegi,
                    GameField.Year
                },
                [FieldsFilling.Default] = new List<GameField>
                {
                    GameField.Licensed,
                    GameField.Source,
                    GameField.Status,
                    GameField.Progress,
                    GameField.Platform,
                    GameField.TrophysetAccess,
                    GameField.AvailablePlatinum,
                    GameField.Year
                },
                [FieldsFilling.Min] = new List<GameField>
                {
                    GameField.Source,
                    GameField.Status,
                    GameField.Platform
                }
            },
            [FieldsVariant.QuickFilterText] = new GameFieldsFillingDictionary()
            {
                [FieldsFilling.Full] = new List<GameField>
                {
                    GameField.Developer,
                    GameField.Edition,
                    GameField.Name,
                    GameField.Publisher,
                    GameField.Series
                },
                [FieldsFilling.Default] = new List<GameField>
                {
                    GameField.Name,
                    GameField.Series
                }
            },
            [FieldsVariant.Summary] = new GameFieldsFillingDictionary()
            {
                [FieldsFilling.Full] = new List<GameField>
                {
                    GameField.Licensed,
                    GameField.Platform,
                    GameField.Source,
                    GameField.Region,
                    GameField.Language,
                    GameField.Format,
                    GameField.Status,
                    GameField.Edition,
                    GameField.Series,
                    GameField.CriticScore,
                    GameField.ScreenView,
                    GameField.Genre,
                    GameField.Pegi,
                    GameField.Year,
                    GameField.Developer,
                    GameField.Publisher,
                    GameField.Difficult,
                    GameField.CompleteTime,
                    GameField.TrophysetAccess,
                    GameField.Progress,
                    GameField.Favorite,
                    GameField.TrophysetTODO
                },
                [FieldsFilling.Default] = new List<GameField>
                {
                    GameField.Platform,
                    GameField.Source,
                    GameField.Region,
                    GameField.Language,
                    GameField.Format,
                    GameField.Status,
                    GameField.Pegi,
                    GameField.Year,
                    GameField.Developer,
                    GameField.Publisher,
                    GameField.Difficult,
                    GameField.CompleteTime,
                    GameField.TrophysetAccess,
                },
                [FieldsFilling.Min] = new List<GameField>
                {
                    GameField.Platform,
                    GameField.Source,
                    GameField.Region,
                    GameField.Language,
                    GameField.Format,
                    GameField.Status,
                    GameField.Difficult,
                    GameField.CompleteTime
                },
            },
            [FieldsVariant.Table] = new GameFieldsFillingDictionary()
            {
                [FieldsFilling.Full] = new List<GameField>
                {
                    GameField.Image,
                    GameField.Name,
                    GameField.Platform,
                    GameField.Licensed,
                    GameField.Region,
                    GameField.Language,
                    GameField.Code,
                    GameField.Source,
                    GameField.Edition,
                    GameField.Series,
                    GameField.CriticScore,
                    GameField.Format,
                    GameField.Developer,
                    GameField.Publisher,
                    GameField.Year,
                    GameField.Pegi,
                    GameField.ReleasePlatforms,
                    GameField.Difficult,
                    GameField.CompleteTime,
                    GameField.Dlcs,
                    GameField.RelatedGames,
                    GameField.GameModes,
                    GameField.TrophysetAccess,
                    GameField.FullGenre,
                    GameField.Progress,
                    GameField.FullPlatinum,
                    GameField.FullGold,
                    GameField.FullSilver,
                    GameField.FullBronze,
                    GameField.FullFromDLC,
                    GameField.FullNet,
                    GameField.Status,
                    GameField.EarnedPoints,
                    GameField.StrategeLink,
                    GameField.PSNProfilesLink,
                    GameField.Verified,
                    GameField.Favorite,
                    GameField.TrophysetTODO

                },
                [FieldsFilling.Default] = new List<GameField>
                {
                    GameField.Image,
                    GameField.Name,
                    GameField.Platform,
                    GameField.Licensed,
                    GameField.Region,
                    GameField.Language,
                    GameField.Source,
                    GameField.FullGenre,
                    GameField.Year,
                    GameField.CriticScore,
                    GameField.Pegi,
                    GameField.Difficult,
                    GameField.CompleteTime,
                    GameField.Progress,
                    GameField.FullPlatinum,
                    GameField.FullGold,
                    GameField.FullSilver,
                    GameField.FullBronze
                },
                [FieldsFilling.Min] = new List<GameField>
                {
                    GameField.Image,
                    GameField.Name,
                    GameField.Source,
                    GameField.Platform
                }
            },
            [FieldsVariant.BatchUpdate] = new GameFieldsFillingDictionary()
            {
                [FieldsFilling.Full] = new List<GameField>
                {
                    GameField.CompleteTime,
                    GameField.CriticScore,
                    GameField.Developer,
                    GameField.Difficult,
                    GameField.Dlcs,
                    GameField.Installations,
                    GameField.Edition,
                    GameField.Format,
                    GameField.GameModes,
                    GameField.Genre,
                    GameField.Pegi,
                    GameField.Platform,
                    GameField.Publisher,
                    GameField.ReleasePlatforms,
                    GameField.Year,
                    GameField.ScreenView,
                    GameField.Series,
                    GameField.Source,
                    GameField.AvailablePlatinum,
                    GameField.AvailableGold,
                    GameField.AvailableSilver,
                    GameField.AvailableBronze,
                    GameField.AvailableFromDLC,
                    GameField.AvailableNet,
                    GameField.TrophysetAccess,
                    GameField.Verified,
                    GameField.Licensed,
                    GameField.Favorite,
                    GameField.TrophysetTODO,
                    GameField.EmulatorType,
                    GameField.EmulatorROMs,
                    GameField.Region,
                    GameField.Language
                }
            }
        };

        public override List<GameField> GetFields(FieldsVariant variant, FieldsFilling filling)
        {
            List<GameField> result = new();

            if (!fieldDictionary.TryGetValue(variant, out var fillingDictionary))
                return result;

            if (!fillingDictionary.TryGetValue(filling, out var fields))
                return result;

            result.AddRange(fields);
            return result;
        }

        public override List<GameField> FullList(FieldsVariant variant) =>
            GetFields(variant, FieldsFilling.Full);

        protected override List<GameField> GetMandatoryFields() => new()
        {
            GameField.Image,
            GameField.Name,
            GameField.Platform,
            GameField.Source
        };

        protected override List<GameField> GetGroupByFields() => new()
        {
            GameField.Platform,
            GameField.Licensed,
            GameField.Format,
            GameField.Status,
            GameField.AvailablePlatinum,
            GameField.Source,
            GameField.Developer,
            GameField.Publisher,
            GameField.Year,
            GameField.Pegi,
            GameField.Difficult,
            GameField.CompleteTime,
            GameField.Genre,
            GameField.ScreenView,
            GameField.TrophysetAccess,
            GameField.Verified,
            GameField.Favorite,
            GameField.TrophysetTODO,
            GameField.Region,
            GameField.Language
        };

        protected override List<GameField> GetCalcedFields() => new()
        {
            GameField.FullGenre,
            GameField.Progress,
            GameField.EarnedPoints,
            GameField.EarnedPointsOld,
            GameField.FullPlatinum,
            GameField.FullGold,
            GameField.FullSilver,
            GameField.FullBronze,
            GameField.FullFromDLC,
            GameField.FullNet,
            GameField.Status,
            GameField.StrategeLink,
            GameField.PSNProfilesLink
        };

        protected override List<GameField> GetIconFields() => new()
        {
            GameField.Image,
            GameField.Name,
            GameField.Platform,
            GameField.Source
        };

        protected override List<GameField> GetEditingFields() => new()
        {
            GameField.Name,
            GameField.Image,
            GameField.Licensed,
            GameField.Source,
            GameField.Platform,
            GameField.Format,
            GameField.Region,
            GameField.Code,
            GameField.Language,
            GameField.Edition,
            GameField.Series,
            GameField.TrophysetAccess,
            GameField.EarnedPlatinum,
            GameField.EarnedGold,
            GameField.EarnedSilver,
            GameField.EarnedBronze,
            GameField.EarnedFromDLC,
            GameField.EarnedNet,
            GameField.AvailablePlatinum,
            GameField.AvailableGold,
            GameField.AvailableSilver,
            GameField.AvailableBronze,
            GameField.AvailableFromDLC,
            GameField.AvailableNet,
            GameField.Difficult,
            GameField.CompleteTime,
            GameField.Developer,
            GameField.Publisher,
            GameField.Year,
            GameField.Pegi,
            GameField.CriticScore,
            GameField.ReleasePlatforms,
            GameField.Installations,
            GameField.ScreenView,
            GameField.Genre,
            GameField.GameModes,
            GameField.Dlcs,
            GameField.Links,
            GameField.RelatedGames,
            GameField.Verified,
            GameField.PlatformFamily,
            GameField.Favorite,
            GameField.TrophysetTODO,
            GameField.EmulatorType,
            GameField.EmulatorROMs
        };

        protected override List<GameField> GetEditedFieldsExtended() => new()
        {
            GameField.Id,
            GameField.Name,
            GameField.Region,
            GameField.Code,
            GameField.Language,
            GameField.Image,
            GameField.Edition,
            GameField.Series,
            GameField.Licensed,
            GameField.CriticScore,
            GameField.PlatformFamily,
            GameField.Platform,
            GameField.Format,
            GameField.EarnedPlatinum,
            GameField.EarnedGold,
            GameField.EarnedSilver,
            GameField.EarnedBronze,
            GameField.EarnedFromDLC,
            GameField.EarnedNet,
            GameField.AvailablePlatinum,
            GameField.AvailableGold,
            GameField.AvailableSilver,
            GameField.AvailableBronze,
            GameField.AvailableFromDLC,
            GameField.AvailableNet,
            GameField.TrophysetAccess,
            GameField.Source,
            GameField.Developer,
            GameField.Publisher,
            GameField.Year,
            GameField.Pegi,
            GameField.ReleasePlatforms,
            GameField.Installations,
            GameField.Difficult,
            GameField.CompleteTime,
            GameField.Genre,
            GameField.ScreenView,
            GameField.GameModes,
            GameField.Dlcs,
            GameField.Links,
            GameField.RelatedGames,
            GameField.Verified,
            GameField.Favorite,
            GameField.TrophysetTODO,
            GameField.EmulatorType,
            GameField.EmulatorROMs
        };

        protected override List<GameField> GetFullInfoFields() => new()
        {
            GameField.Image,
            GameField.Name,
            GameField.Edition,
            GameField.Series,
            GameField.CriticScore,
            GameField.Platform,
            GameField.Licensed,
            GameField.Status,
            GameField.Format,
            GameField.EarnedPlatinum,
            GameField.EarnedGold,
            GameField.EarnedSilver,
            GameField.EarnedBronze,
            GameField.EarnedFromDLC,
            GameField.EarnedNet,
            GameField.AvailablePlatinum,
            GameField.AvailableGold,
            GameField.AvailableSilver,
            GameField.AvailableBronze,
            GameField.AvailableFromDLC,
            GameField.AvailableNet,
            GameField.TrophysetAccess,
            GameField.FullBronze,
            GameField.FullSilver,
            GameField.FullGold,
            GameField.FullPlatinum,
            GameField.FullFromDLC,
            GameField.FullNet,
            GameField.Progress,
            GameField.Source,
            GameField.Developer,
            GameField.Publisher,
            GameField.Year,
            GameField.Pegi,
            GameField.ReleasePlatforms,
            GameField.Installations,
            GameField.Difficult,
            GameField.CompleteTime,
            GameField.FullGenre,
            GameField.Genre,
            GameField.ScreenView,
            GameField.GameModes,
            GameField.Dlcs,
            GameField.Links,
            GameField.RelatedGames,
            GameField.Verified,
            GameField.Favorite,
            GameField.TrophysetTODO,
            GameField.Region,
            GameField.Language,
            GameField.Code
        };

        protected override List<GameField> GetCardFields() => new()
        {
            GameField.Image,
            GameField.Name,
            GameField.CriticScore,
            GameField.Platform,
            GameField.Format,
            GameField.AvailableNet,
            GameField.TrophysetAccess,
            GameField.Source,
            GameField.Developer,
            GameField.Publisher,
            GameField.Year,
            GameField.Pegi,
            GameField.ReleasePlatforms,
            GameField.Difficult,
            GameField.CompleteTime,
            GameField.Links,
            GameField.Region,
            GameField.Language,
            GameField.Code,

            GameField.FullGenre,
            GameField.Progress,
            GameField.EarnedPoints,
            GameField.EarnedPointsOld,
            GameField.FullPlatinum,
            GameField.FullGold,
            GameField.FullSilver,
            GameField.FullBronze,
            GameField.FullFromDLC,
            GameField.FullNet,
            GameField.Status
        };

        protected override FilterOperation GetDefaultFilterOperation(GameField field) => 
            field switch 
            {
                GameField.Name or
                GameField.Edition or
                GameField.Series or
                GameField.Developer or
                GameField.Publisher or
                GameField.ReleasePlatforms or
                GameField.Genre or
                GameField.GameModes or
                GameField.Dlcs or
                GameField.Links or
                GameField.Installations or
                GameField.RelatedGames or
                GameField.FullGenre or
                GameField.EmulatorType or
                GameField.EmulatorROMs => 
                    FilterOperation.Contains,
                GameField.Image or
                GameField.StrategeLink or
                GameField.PSNProfilesLink => 
                    FilterOperation.NotBlank,
                _ =>
                    FilterOperation.Equals
            };

        protected override FieldFilterOperations<GameField> GetAvailableFilterOperations() => new()
        {
            [GameField.Id] = FilterOperations.EnumOperations,
            [GameField.Licensed] = FilterOperations.EnumOperations,
            [GameField.Favorite] = FilterOperations.EnumOperations,
            [GameField.TrophysetTODO] = FilterOperations.EnumOperations,
            [GameField.Name] = FilterOperations.StringOperations,
            [GameField.Image] = FilterOperations.UnaryOperations,
            [GameField.Edition] = FilterOperations.StringOperations,
            [GameField.Series] = FilterOperations.StringOperations,
            [GameField.CriticScore] = FilterOperations.NumericOperations,
            [GameField.PlatformFamily] = FilterOperations.EnumOperations,
            [GameField.Platform] = FilterOperations.EnumOperations,
            [GameField.Format] = FilterOperations.EnumOperations,
            [GameField.EarnedPlatinum] = FilterOperations.NumericOperations,
            [GameField.EarnedGold] = FilterOperations.NumericOperations,
            [GameField.EarnedSilver] = FilterOperations.NumericOperations,
            [GameField.EarnedBronze] = FilterOperations.NumericOperations,
            [GameField.EarnedFromDLC] = FilterOperations.NumericOperations,
            [GameField.EarnedNet] = FilterOperations.NumericOperations,
            [GameField.AvailablePlatinum] = FilterOperations.NumericOperations,
            [GameField.AvailableGold] = FilterOperations.NumericOperations,
            [GameField.AvailableSilver] = FilterOperations.NumericOperations,
            [GameField.AvailableBronze] = FilterOperations.NumericOperations,
            [GameField.AvailableFromDLC] = FilterOperations.NumericOperations,
            [GameField.AvailableNet] = FilterOperations.NumericOperations,
            [GameField.TrophysetAccess] = FilterOperations.EnumOperations,
            [GameField.Source] = FilterOperations.EnumOperations,
            [GameField.Developer] = FilterOperations.StringOperations,
            [GameField.Publisher] = FilterOperations.StringOperations,
            [GameField.Year] = FilterOperations.NumericOperations,
            [GameField.Pegi] = FilterOperations.NumericOperations,
            [GameField.ReleasePlatforms] = FilterOperations.ObjectOperations,
            [GameField.Difficult] = FilterOperations.EnumOperations,
            [GameField.CompleteTime] = FilterOperations.EnumOperations,
            [GameField.Genre] = FilterOperations.StringOperations,
            [GameField.ScreenView] = FilterOperations.EnumOperations,
            [GameField.GameModes] = FilterOperations.ObjectOperations,
            [GameField.Dlcs] = FilterOperations.ObjectOperations,
            [GameField.Links] = FilterOperations.ObjectOperations,
            [GameField.Installations] = FilterOperations.ObjectOperations,
            [GameField.RelatedGames] = FilterOperations.ObjectOperations,
            [GameField.FullGenre] = FilterOperations.StringOperations,
            [GameField.Progress] = FilterOperations.NumericOperations,
            [GameField.EarnedPoints] = FilterOperations.NumericOperations,
            [GameField.EarnedPointsOld] = FilterOperations.NumericOperations,
            [GameField.FullPlatinum] = FilterOperations.StringOperations,
            [GameField.FullGold] = FilterOperations.StringOperations,
            [GameField.FullSilver] = FilterOperations.StringOperations,
            [GameField.FullBronze] = FilterOperations.StringOperations,
            [GameField.FullFromDLC] = FilterOperations.StringOperations,
            [GameField.FullNet] = FilterOperations.StringOperations,
            [GameField.Status] = FilterOperations.EnumOperations,
            [GameField.StrategeLink] = FilterOperations.UnaryOperations,
            [GameField.PSNProfilesLink] = FilterOperations.UnaryOperations,
            [GameField.Verified] = FilterOperations.BoolOperations,
            [GameField.EmulatorType] = FilterOperations.StringOperations,
            [GameField.EmulatorROMs] = FilterOperations.StringOperations,
            [GameField.Region] = FilterOperations.EnumOperations,
            [GameField.Language] = FilterOperations.EnumOperations,
            [GameField.Code] = FilterOperations.StringOperations
        };


        public List<GameField> SameTrophysetFields = new()
        {
            GameField.TrophysetAccess,
            GameField.EarnedPlatinum,
            GameField.EarnedGold,
            GameField.EarnedSilver,
            GameField.EarnedBronze,
            GameField.EarnedFromDLC,
            GameField.EarnedNet,
            GameField.AvailablePlatinum,
            GameField.AvailableGold,
            GameField.AvailableSilver,
            GameField.AvailableBronze,
            GameField.AvailableFromDLC,
            GameField.AvailableNet,
            GameField.Difficult,
            GameField.CompleteTime
        };

        public List<GameField> SyncronizedRelatedFields = new()
        {
            GameField.Series,
            GameField.Genre,
            GameField.ScreenView
        };

        public List<GameField> SyncronizedReleaseFields = new()
        {
            GameField.Developer,
            GameField.Publisher,
            GameField.Pegi,
            GameField.ReleasePlatforms
        };

        protected override List<GameField> GetSelectQuickFilterFields() => new()
        {
            GameField.Source,
            GameField.Platform,
            GameField.Format,
            GameField.Year
        };

        public List<GameField> AlwaysUpdateFields = new()
        {
            GameField.RelatedGames
        };

        public override GameField FieldMetaData => GameField.Field;

        public override GameField TitleField => GameField.Name;

        public override GameField UniqueField => GameField.Id;

        public override List<GameField> Depended(GameField field) => 
            field switch
            {
                GameField.Source => new List<GameField>()
                        {
                            GameField.Licensed
                        },
                GameField.Platform => new List<GameField>()
                        {
                            GameField.PlatformFamily,
                            GameField.Source
                        },
                GameField.Format => new List<GameField>()
                        {
                            GameField.Licensed,
                            GameField.Platform
                        },
                _ => base.Depended(field),
            };

        public override DataGridViewContentAlignment ColumnAlign(GameField field) =>
            field == GameField.Name
                ? DataGridViewContentAlignment.MiddleLeft
                : DataGridViewContentAlignment.MiddleCenter;


        public override int ColumnWidth(GameField field) => 
            field switch
            {
                GameField.Name or
                GameField.RelatedGames => 
                    350,
                GameField.Platform => 
                    70,
                GameField.Image or 
                GameField.Format or 
                GameField.ReleasePlatforms => 
                    70,
                GameField.Edition or 
                GameField.Series or 
                GameField.Dlcs or 
                GameField.GameModes or 
                GameField.FullGenre or 
                GameField.Developer or 
                GameField.Publisher => 
                    120,
                GameField.EarnedPlatinum or 
                GameField.EarnedGold or 
                GameField.EarnedSilver or 
                GameField.EarnedBronze or 
                GameField.EarnedFromDLC or 
                GameField.EarnedNet or 
                GameField.AvailablePlatinum or 
                GameField.AvailableGold or 
                GameField.AvailableSilver or 
                GameField.AvailableBronze or 
                GameField.AvailableFromDLC or 
                GameField.AvailableNet => 
                    24,
                GameField.Source => 
                    88,
                GameField.CriticScore or 
                GameField.Year or 
                GameField.Pegi => 
                    40,
                GameField.Difficult or 
                GameField.Progress or 
                GameField.FullPlatinum or 
                GameField.FullGold or 
                GameField.FullSilver or 
                GameField.FullBronze or 
                GameField.FullFromDLC or 
                GameField.FullNet => 
                    60,
                GameField.CompleteTime or 
                GameField.Status or 
                GameField.Installations => 
                    80,
                GameField.ScreenView or 
                GameField.EarnedPoints or 
                GameField.EarnedPointsOld => 
                    64,
                _ => 
                    100,
            };

        public override FieldType GetFieldType(GameField field) => 
            field switch
            {
                GameField.EmulatorROMs => 
                    FieldType.Memo,
                GameField.Image => 
                    FieldType.Image,
                GameField.Edition or 
                GameField.Series or 
                GameField.Developer or 
                GameField.Publisher or 
                GameField.Genre or 
                GameField.FullGenre or 
                GameField.EmulatorType => 
                    FieldType.Extract,
                GameField.PlatformFamily or 
                GameField.Platform or 
                GameField.ScreenView or 
                GameField.Format or 
                GameField.Source or 
                GameField.Pegi or 
                GameField.CompleteTime or 
                GameField.Difficult or 
                GameField.Status or 
                GameField.TrophysetAccess or
                GameField.Region or
                GameField.Language =>
                    FieldType.Enum,
                GameField.EarnedPlatinum or 
                GameField.AvailablePlatinum or 
                GameField.Verified or 
                GameField.Licensed or 
                GameField.Favorite or 
                GameField.TrophysetTODO => 
                    FieldType.Boolean,
                GameField.EarnedGold or 
                GameField.EarnedSilver or 
                GameField.EarnedBronze or 
                GameField.EarnedFromDLC or 
                GameField.EarnedNet or 
                GameField.AvailableGold or 
                GameField.AvailableSilver or 
                GameField.AvailableBronze or 
                GameField.AvailableFromDLC or 
                GameField.AvailableNet or 
                GameField.EarnedPoints or 
                GameField.EarnedPointsOld or 
                GameField.CriticScore or 
                GameField.Progress => 
                    FieldType.Integer,
                GameField.Id => 
                    FieldType.Guid,
                GameField.GameModes or 
                GameField.Dlcs or 
                GameField.Links or 
                GameField.Installations or 
                GameField.RelatedGames or 
                GameField.ReleasePlatforms => 
                    FieldType.List,
                GameField.Year or 
                GameField.StrategeLink or 
                GameField.PSNProfilesLink => 
                    FieldType.Custom,
                _ => 
                    FieldType.String,
            };

        public override ITypeHelper? GetHelper(GameField field) =>
            field switch
            {
                GameField.Region => TypeHelper.Helper<GameRegionHelper>(),
                GameField.Language => TypeHelper.Helper<GameLanguageHelper>(),
                GameField.Source => TypeHelper.Helper<SourceHelper>(),
                GameField.PlatformFamily => TypeHelper.Helper<PlatformFamilyHelper>(),
                GameField.Platform => TypeHelper.Helper<PlatformTypeHelper>(),
                GameField.Format => TypeHelper.Helper<GameFormatHelper>(),
                GameField.Pegi => TypeHelper.Helper<PegiHelper>(),
                GameField.Difficult => TypeHelper.Helper<DifficultHelper>(),
                GameField.CompleteTime => TypeHelper.Helper<CompleteTimeHelper>(),
                GameField.ScreenView => TypeHelper.Helper<ScreenViewHelper>(),
                GameField.TrophysetAccess => TypeHelper.Helper<TrophysetAccessHelper>(),
                GameField.Status => TypeHelper.Helper<StatusHelper>(),
                _ => null
            };
    }
}
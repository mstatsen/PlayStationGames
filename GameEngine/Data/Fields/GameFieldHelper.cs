using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Context;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Fields.Types;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Data.Filter.Types;
using OxDAOEngine.Data.Types;
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
                GameField.Verified => "Verified",
                GameField.TrophysetType => "Trophyset",
                GameField.ReleasePlatforms => "Released on",
                _ => base.ColumnCaption(field),
            };

        public override string GetName(GameField value) =>
            value switch
            {
                GameField.Id => "Game ID",
                GameField.Name => "Name",
                GameField.Owner => "PSN Profile",
                GameField.Licensed => "Licensed",
                GameField.Tags => "Tags",
                GameField.Image => "Image",
                GameField.Edition => "Edition",
                GameField.Serieses => "Serieses",
                GameField.CriticScore => "Critic Score",
                GameField.Platform => "Platform",
                GameField.Format => "Format",
                GameField.AvailablePlatinum => "Platinum",
                GameField.AvailableGold => "Gold",
                GameField.AvailableSilver => "Silver",
                GameField.AvailableBronze => "Bronze",
                GameField.TrophysetType => "Type",
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
                GameField.SinglePlayer => "Single player",
                GameField.CoachMultiplayer => "Coach multiplayer",
                GameField.MaximumPlayers => "Maximum players",
                GameField.OnlineMultiplayer => "Online multiplayer",
                GameField.Dlcs => "DLCs",
                GameField.Links => "Links",
                GameField.RelatedGames => "Related games",
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
                GameField.Trophyset => "Trophyset",
                GameField.Devices => "Devices",
                GameField.AppliesTo => "Applies to",
                GameField.Installed => "Installed",
                GameField.Multiplayer => "Multiplayer",
                GameField.ExistsDLCsWithTrophyset => "+DLCs trophies",
                _ => string.Empty,
            };

        public override string GetFullName(GameField value) => 
            value switch
            {
                GameField.TrophysetType => "Trophyset Accessibility",
                GameField.EmulatorType => "Emulator Type",
                GameField.EmulatorROMs => "Emulator ROMs",
                GameField.Devices => "Used devices",
                _ => GetName(value),
            };

        private readonly GameFieldsVariantDictionary fieldDictionary = new()
        {
            [FieldsVariant.Category] = new()
            {
                [FieldsFilling.Full] = new()
                {
                    GameField.Owner,
                    GameField.Licensed,
                    GameField.CompleteTime,
                    GameField.CriticScore,
                    GameField.Developer,
                    GameField.Difficult,
                    GameField.Edition,
                    GameField.Format,
                    GameField.Genre,
                    GameField.SinglePlayer,
                    GameField.CoachMultiplayer,
                    GameField.MaximumPlayers,
                    GameField.OnlineMultiplayer,
                    GameField.Pegi,
                    GameField.Platform,
                    GameField.Publisher,
                    GameField.Year,
                    GameField.ScreenView,
                    GameField.Serieses,
                    GameField.Source,
                    GameField.TrophysetType,
                    GameField.Region,
                    GameField.Language,
                    GameField.Tags,
                    GameField.Installed,
                    GameField.Multiplayer,
                    GameField.ExistsDLCsWithTrophyset
                },
                [FieldsFilling.Default] = new()
                {
                    GameField.Tags
                }
            },
            [FieldsVariant.Inline] = new()
            {
                [FieldsFilling.Full] = new()
                {
                    GameField.Owner,
                    GameField.Licensed,
                    GameField.Platform,
                    GameField.Source,
                    GameField.Region,
                    GameField.Language,
                    GameField.Format,
                    GameField.Edition,
                    GameField.Dlcs,
                    GameField.Serieses,
                    GameField.CriticScore,
                    GameField.Genre,
                    GameField.SinglePlayer,
                    GameField.CoachMultiplayer,
                    GameField.MaximumPlayers,
                    GameField.OnlineMultiplayer,
                    GameField.Pegi,
                    GameField.Year,
                    GameField.Developer,
                    GameField.Publisher,
                    GameField.Difficult,
                    GameField.CompleteTime,
                    GameField.TrophysetType,
                    GameField.Verified,
                    GameField.Tags,
                    GameField.Code,
                    GameField.Devices,
                    GameField.AppliesTo
                },
                [FieldsFilling.Default] = new()
                {
                    GameField.Platform,
                    GameField.Source
                }
            },
            [FieldsVariant.QuickFilter] = new()
            {
                [FieldsFilling.Full] = new()
                {
                    GameField.Owner,
                    GameField.Licensed,
                    GameField.Source,
                    GameField.Platform,
                    GameField.Format,
                    GameField.ScreenView,
                    GameField.Genre,
                    GameField.Installed,
                    GameField.SinglePlayer,
                    GameField.Multiplayer,
                    GameField.TrophysetType,
                    GameField.Difficult,
                    GameField.CompleteTime,
                    GameField.Pegi,
                    GameField.Year,
                },
                [FieldsFilling.Default] = new()
                {
                    GameField.Owner,
                    GameField.Licensed,
                    GameField.Source,
                    GameField.Platform,
                    GameField.TrophysetType,
                    GameField.AvailablePlatinum,
                    GameField.Year
                },
                [FieldsFilling.Min] = new()
                {
                    GameField.Source,
                    GameField.Platform
                }
            },
            [FieldsVariant.QuickFilterText] = new()
            {
                [FieldsFilling.Full] = new()
                {
                    GameField.Developer,
                    GameField.Edition,
                    GameField.Name,
                    GameField.Publisher
                },
                [FieldsFilling.Default] = new()
                {
                    GameField.Name
                }
            },
            [FieldsVariant.Table] = new()
            {
                [FieldsFilling.Full] = new()
                {
                    GameField.Image,
                    GameField.Name,
                    GameField.Platform,
                    GameField.Owner,
                    GameField.Licensed,
                    GameField.Region,
                    GameField.Language,
                    GameField.Code,
                    GameField.Source,
                    GameField.Edition,
                    GameField.Serieses,
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
                    GameField.ExistsDLCsWithTrophyset,
                    GameField.RelatedGames,
                    GameField.TrophysetType,
                    GameField.Genre,
                    GameField.SinglePlayer,
                    GameField.CoachMultiplayer,
                    GameField.MaximumPlayers,
                    GameField.OnlineMultiplayer,
                    GameField.Multiplayer,
                    GameField.StrategeLink,
                    GameField.PSNProfilesLink,
                    GameField.Verified,
                    GameField.Tags,
                    GameField.Devices,
                    GameField.AppliesTo,
                    GameField.Installed,
                },
                [FieldsFilling.Default] = new()
                {
                    GameField.Image,
                    GameField.Name,
                    GameField.Platform,
                    GameField.Owner,
                    GameField.Licensed,
                    GameField.Region,
                    GameField.Language,
                    GameField.Source,
                    GameField.Genre,
                    GameField.Year,
                    GameField.CriticScore,
                    GameField.Pegi,
                    GameField.Difficult,
                    GameField.CompleteTime,
                },
                [FieldsFilling.Min] = new()
                {
                    GameField.Image,
                    GameField.Name,
                    GameField.Source,
                    GameField.Platform
                }
            },
            [FieldsVariant.BatchUpdate] = new()
            {
                [FieldsFilling.Full] = new()
                {
                    GameField.CompleteTime,
                    GameField.CriticScore,
                    GameField.Developer,
                    GameField.Difficult,
                    GameField.Dlcs,
                    GameField.Installations,
                    GameField.Edition,
                    GameField.Format,
                    GameField.Genre,
                    GameField.SinglePlayer,
                    GameField.CoachMultiplayer,
                    GameField.MaximumPlayers,
                    GameField.OnlineMultiplayer,
                    GameField.Pegi,
                    GameField.Platform,
                    GameField.Publisher,
                    GameField.ReleasePlatforms,
                    GameField.Year,
                    GameField.ScreenView,
                    GameField.Serieses,
                    GameField.Source,
                    GameField.TrophysetType,
                    GameField.Verified,
                    GameField.Owner,
                    GameField.Licensed,
                    GameField.Tags,
                    GameField.EmulatorType,
                    GameField.EmulatorROMs,
                    GameField.Region,
                    GameField.Language,
                    GameField.Devices,
                }
            }
        };

        protected override List<GameField> GetSummaryFields() => new()
        {
            GameField.Verified,
            GameField.Licensed,
            GameField.Installed,
            GameField.AvailablePlatinum,
            GameField.SinglePlayer,
            GameField.Multiplayer,
            GameField.Tags,
            GameField.CriticScore,
            GameField.Pegi,
            GameField.Year,
            GameField.Publisher,
            GameField.Developer,
            GameField.CompleteTime,
            GameField.Difficult,
            GameField.TrophysetType,
            GameField.Genre,
            GameField.ScreenView,
            GameField.Serieses,
            GameField.Edition,
            GameField.Language,
            GameField.Region,
            GameField.Source,
            GameField.Format,
            GameField.Platform,
            GameField.Owner,
        };

        public override List<GameField> GetFieldsInternal(FieldsVariant variant, FieldsFilling filling)
        {
            List<GameField> result = new();

            if (!fieldDictionary.TryGetValue(variant, out var fillingDictionary))
                return result;

            if (!fillingDictionary.TryGetValue(filling, out var fields))
                return result;

            result.AddRange(fields);
            return result;
        }

        protected override List<GameField> GetMandatoryFields() => new()
        {
            GameField.Image,
            GameField.Name,
            GameField.Licensed,
            GameField.Owner,
            GameField.Platform,
            GameField.Source
        };

        protected override List<GameField> GetGroupByFields() => new()
        {
            GameField.Platform,
            GameField.Owner,
            GameField.Licensed,
            GameField.Format,
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
            GameField.SinglePlayer,
            GameField.CoachMultiplayer,
            GameField.MaximumPlayers,
            GameField.OnlineMultiplayer,
            GameField.TrophysetType,
            GameField.Verified,
            GameField.Tags,
            GameField.Region,
            GameField.Language,
            GameField.Installed,
            GameField.Multiplayer,
        };

        protected override List<GameField> GetCalcedFields() => new()
        {
            GameField.TrophysetType,
            GameField.AppliesTo,
            GameField.Difficult,
            GameField.CompleteTime,
            GameField.AvailableBronze,
            GameField.AvailableSilver,
            GameField.AvailableGold,
            GameField.AvailablePlatinum,
            GameField.StrategeLink,
            GameField.PSNProfilesLink,
            GameField.Installed,
            GameField.Multiplayer,
            GameField.ExistsDLCsWithTrophyset,
        };

        protected override List<GameField> GetEditingFields() => new()
        {
            GameField.Name,
            GameField.Image,
            GameField.Licensed,
            GameField.Owner,
            GameField.Platform,
            GameField.Source,
            GameField.Format,
            GameField.Region,
            GameField.Code,
            GameField.Language,
            GameField.Edition,
            GameField.Serieses,
            GameField.Trophyset,
            GameField.Developer,
            GameField.Publisher,
            GameField.Year,
            GameField.Pegi,
            GameField.CriticScore,
            GameField.ReleasePlatforms,
            GameField.Installations,
            GameField.ScreenView,
            GameField.Genre,
            GameField.SinglePlayer,
            GameField.CoachMultiplayer,
            GameField.MaximumPlayers,
            GameField.OnlineMultiplayer,
            GameField.Dlcs,
            GameField.Links,
            GameField.RelatedGames,
            GameField.Verified,
            GameField.PlatformFamily,
            GameField.Tags,
            GameField.EmulatorType,
            GameField.EmulatorROMs,
            GameField.Devices
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
            GameField.Serieses,
            GameField.Trophyset,
            GameField.Owner,
            GameField.Licensed,
            GameField.CriticScore,
            GameField.PlatformFamily,
            GameField.Platform,
            GameField.Format,
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
            GameField.SinglePlayer,
            GameField.CoachMultiplayer,
            GameField.MaximumPlayers,
            GameField.OnlineMultiplayer,
            GameField.ScreenView,
            GameField.Dlcs,
            GameField.Links,
            GameField.RelatedGames,
            GameField.Verified,
            GameField.Tags,
            GameField.EmulatorType,
            GameField.EmulatorROMs,
            GameField.Devices
        };

        protected override List<GameField> GetFullInfoFields() => new()
        {
            GameField.Image,
            GameField.Name,
            GameField.Edition,
            GameField.Serieses,
            GameField.CriticScore,
            GameField.Platform,
            GameField.Format,
            GameField.Owner,
            GameField.Licensed,
            GameField.TrophysetType,
            GameField.Difficult,
            GameField.CompleteTime,
            GameField.AvailablePlatinum,
            GameField.AvailableGold,
            GameField.AvailableSilver,
            GameField.AvailableBronze,
            GameField.Source,
            GameField.Developer,
            GameField.Publisher,
            GameField.Year,
            GameField.Pegi,
            GameField.ReleasePlatforms,
            GameField.Installations,
            GameField.Genre,
            GameField.Dlcs,
            GameField.Links,
            GameField.RelatedGames,
            GameField.Tags,
            GameField.Region,
            GameField.Language,
            GameField.Code,
            GameField.Devices,
            GameField.AppliesTo
        };

        protected override List<GameField> GetCardFields() => new()
        {
            GameField.Image,
            GameField.Name,
            GameField.Licensed,
            GameField.Installed,
            GameField.Owner,
            GameField.Source,
            GameField.Platform,
            GameField.Region,
            GameField.Serieses,
            GameField.Edition,
            GameField.Genre,
            GameField.Devices,
            GameField.ExistsDLCsWithTrophyset,
            GameField.Developer,
            GameField.Publisher,
            GameField.Year,
            GameField.Pegi,
            GameField.ReleasePlatforms,
            GameField.CriticScore,
            GameField.TrophysetType,
            GameField.AppliesTo,
            GameField.Difficult,
            GameField.CompleteTime,
            GameField.AvailablePlatinum,
            GameField.AvailableGold,
            GameField.AvailableSilver,
            GameField.AvailableBronze,
            GameField.Links,
        };

        protected override FilterOperation GetDefaultFilterOperation(GameField field) => 
            field switch 
            {
                GameField.Name or
                GameField.Edition or
                GameField.Serieses or
                GameField.Developer or
                GameField.Publisher or
                GameField.ReleasePlatforms or
                GameField.Genre or
                GameField.Dlcs or
                GameField.Links or
                GameField.Installations or
                GameField.RelatedGames or
                GameField.EmulatorType or
                GameField.EmulatorROMs or
                GameField.Trophyset or
                GameField.Tags or
                GameField.Devices or
                GameField.AppliesTo => 
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
            [GameField.Owner] = FilterOperations.StringOperations,
            [GameField.Licensed] = FilterOperations.EnumOperations,
            [GameField.Tags] = FilterOperations.ObjectOperations,
            [GameField.Name] = FilterOperations.StringOperations,
            [GameField.Image] = FilterOperations.UnaryOperations,
            [GameField.Edition] = FilterOperations.StringOperations,
            [GameField.Serieses] = FilterOperations.ObjectOperations,
            [GameField.CriticScore] = FilterOperations.NumericOperations,
            [GameField.PlatformFamily] = FilterOperations.EnumOperations,
            [GameField.Platform] = FilterOperations.EnumOperations,
            [GameField.Format] = FilterOperations.EnumOperations,
            [GameField.AvailablePlatinum] = FilterOperations.NumericOperations,
            [GameField.AvailableGold] = FilterOperations.NumericOperations,
            [GameField.AvailableSilver] = FilterOperations.NumericOperations,
            [GameField.AvailableBronze] = FilterOperations.NumericOperations,
            [GameField.TrophysetType] = FilterOperations.EnumOperations,
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
            [GameField.Dlcs] = FilterOperations.ObjectOperations,
            [GameField.Trophyset] = FilterOperations.ObjectOperations,
            [GameField.Links] = FilterOperations.ObjectOperations,
            [GameField.Installations] = FilterOperations.ObjectOperations,
            [GameField.RelatedGames] = FilterOperations.ObjectOperations,
            [GameField.StrategeLink] = FilterOperations.UnaryOperations,
            [GameField.PSNProfilesLink] = FilterOperations.UnaryOperations,
            [GameField.Verified] = FilterOperations.BoolOperations,
            [GameField.SinglePlayer] = FilterOperations.BoolOperations,
            [GameField.CoachMultiplayer] = FilterOperations.BoolOperations,
            [GameField.MaximumPlayers] = FilterOperations.StringOperations,
            [GameField.OnlineMultiplayer] = FilterOperations.BoolOperations,
            [GameField.EmulatorType] = FilterOperations.StringOperations,
            [GameField.EmulatorROMs] = FilterOperations.StringOperations,
            [GameField.Region] = FilterOperations.EnumOperations,
            [GameField.Language] = FilterOperations.EnumOperations,
            [GameField.Code] = FilterOperations.StringOperations,
            [GameField.Devices] = FilterOperations.ObjectOperations,
            [GameField.AppliesTo] = FilterOperations.ObjectOperations,
            [GameField.Installed] = FilterOperations.BoolOperations,
            [GameField.Multiplayer] = FilterOperations.BoolOperations,
            [GameField.ExistsDLCsWithTrophyset] = FilterOperations.BoolOperations,
        };

        protected override List<GameField> GetSelectQuickFilterFields() => new()
        {
            GameField.Source,
            GameField.Platform,
            GameField.Format,
            GameField.Year
        };

        public override GameField FieldMetaData => GameField.Field;
        public override GameField TitleField => GameField.Name;
        public override GameField ImageField => GameField.Image;
        public override GameField UniqueField => GameField.Id;

        public readonly List<GameField> TrophiesFields = new()
        {
            GameField.AvailablePlatinum,
            GameField.AvailableGold,
            GameField.AvailableSilver,
            GameField.AvailableBronze
        };

        public override List<GameField> Depended(GameField field) => 
            field switch
            {
                GameField.Source => new List<GameField>()
                    {
                        GameField.Licensed,
                        GameField.Platform
                    },
                GameField.Platform => new List<GameField>()
                    {
                        GameField.PlatformFamily
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
                GameField.Name => 
                    350,
                GameField.Format or 
                GameField.ReleasePlatforms => 
                    70,
                GameField.Edition or 
                GameField.Serieses or 
                GameField.Genre or 
                GameField.Developer or 
                GameField.Publisher => 
                    120,
                GameField.AvailablePlatinum or 
                GameField.AvailableGold or 
                GameField.AvailableSilver or 
                GameField.AvailableBronze => 
                    24,
                GameField.RelatedGames or
                GameField.Source => 
                    70,
                GameField.Dlcs or
                GameField.CriticScore or 
                GameField.Year or
                GameField.MaximumPlayers or
                GameField.Pegi => 
                    40,
                GameField.Licensed or
                GameField.Verified or
                GameField.Installed or
                GameField.Platform or
                GameField.Image or
                GameField.SinglePlayer or
                GameField.Multiplayer or
                GameField.CoachMultiplayer or
                GameField.OnlineMultiplayer or
                GameField.ExistsDLCsWithTrophyset or
                GameField.Difficult =>
                    60,
                GameField.CompleteTime or 
                GameField.Installations => 
                    80,
                GameField.ScreenView =>
                    64,
                _ => 
                    100,
            };

        public override FieldType GetFieldType(GameField field) => 
            field switch
            {
                GameField.Name =>
                    FieldType.ShortMemo,
                GameField.EmulatorROMs => 
                    FieldType.Memo,
                GameField.Image => 
                    FieldType.Image,
                GameField.Edition or 
                GameField.Developer or 
                GameField.Publisher or 
                GameField.Genre or 
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
                GameField.TrophysetType or
                GameField.Region or
                GameField.Language =>
                    FieldType.Enum,
                GameField.AvailablePlatinum or 
                GameField.Verified or 
                GameField.Licensed or
                GameField.Installed or
                GameField.Multiplayer or
                GameField.SinglePlayer or
                GameField.CoachMultiplayer or
                GameField.OnlineMultiplayer or
                GameField.ExistsDLCsWithTrophyset => 
                    FieldType.Boolean,
                GameField.AvailableGold or
                GameField.AvailableSilver or
                GameField.AvailableBronze or
                GameField.MaximumPlayers or
                GameField.CriticScore => 
                    FieldType.Integer,
                GameField.Id =>
                    FieldType.Guid,
                GameField.Dlcs or
                GameField.Installations or 
                GameField.RelatedGames or 
                GameField.ReleasePlatforms or
                GameField.Tags or
                GameField.Serieses or
                GameField.Devices or
                GameField.AppliesTo => 
                    FieldType.List,
                GameField.StrategeLink or
                GameField.PSNProfilesLink =>
                    FieldType.Link,
                GameField.Links =>
                    FieldType.LinkList,
                GameField.Year or 
                GameField.Owner or
                GameField.Trophyset => 
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
                GameField.TrophysetType => TypeHelper.Helper<TrophysetTypeHelper>(),
                _ => null
            };

        public static List<GameField> UnverifiedFields() =>
            new()
            {
                GameField.Installations,
                GameField.Links,
                GameField.RelatedGames,
                GameField.Tags
            };

        public override void FillAdditionalContext(GameField field, IAccessorContext context)
        {
            if (context.Scope == ControlScope.Editor && 
                field == GameField.Name)
                context.AdditionalContext = true;
        }

        public override ILinkHelper<GameField>? GetLinkHelper() => TypeHelper.Helper<GameLinkTypeHelper>();

        protected override List<GameField> GetSynchronizedFields() =>
            new()
            {
                GameField.Name,
                GameField.Image,
                GameField.Licensed,
                GameField.Region,
                GameField.Language,
                GameField.Code,
                GameField.Edition,
                GameField.Serieses,
                GameField.EmulatorType,
                GameField.EmulatorROMs,
                GameField.Dlcs,
                GameField.Genre,
                GameField.ScreenView,
                GameField.SinglePlayer,
                GameField.CoachMultiplayer,
                GameField.MaximumPlayers,
                GameField.OnlineMultiplayer,
                GameField.Tags,
                GameField.Trophyset,
                GameField.CriticScore,
                GameField.Developer,
                GameField.Publisher,
                GameField.Year,
                GameField.Pegi,
                GameField.ReleasePlatforms,
                GameField.Devices,
                GameField.AppliesTo
            };
    }
}
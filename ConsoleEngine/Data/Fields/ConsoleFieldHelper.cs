using OxXMLEngine.Data.Fields;
using OxXMLEngine.Data.Filter;
using OxXMLEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Data.Fields
{
    public class ConsoleFieldHelper : FieldHelper<ConsoleField>
    {
        public override ConsoleField EmptyValue() =>
            ConsoleField.Id;

        public override string GetName(ConsoleField value) => 
            value switch
            {
                ConsoleField.Id => "Id",
                ConsoleField.Name => "Name",
                ConsoleField.Generation => "Generation",
                ConsoleField.Model => "Model",
                ConsoleField.Storages => "Storages",
                ConsoleField.Folders => "Folders",
                ConsoleField.Accessories => "Accessories",
                ConsoleField.Games => "Games",
                ConsoleField.Accounts => "Accounts",
                ConsoleField.Firmware => "Firmware",
                ConsoleField.Console => "Console",
                ConsoleField.Field => "Field",
                ConsoleField.Icon => "Icon",
                _ => "",
            };

        internal class ConsoleFieldsFillingDictionary : FieldsFillingDictionary<ConsoleField> { };
        internal class ConsoleFieldsVariantDictionary : Dictionary<FieldsVariant, ConsoleFieldsFillingDictionary> { };

        protected override List<ConsoleField> GetMandatoryFields() =>
            new()
            {
                ConsoleField.Icon,
                ConsoleField.Name,
                ConsoleField.Generation,
                ConsoleField.Model,
                ConsoleField.Firmware
            };

        protected override List<ConsoleField> GetEditingFields() =>
            new()
            {
                ConsoleField.Name,
                ConsoleField.Generation,
                ConsoleField.Model,
                ConsoleField.Firmware,
                ConsoleField.Storages,
                ConsoleField.Folders,
                ConsoleField.Accessories,
                ConsoleField.Accounts
            };

        private static readonly ConsoleFieldsVariantDictionary fieldDictionary = 
            new()
            {
                [FieldsVariant.Table] = new ConsoleFieldsFillingDictionary()
                {
                    [FieldsFilling.Full] = new List<ConsoleField>
                    {
                        ConsoleField.Icon,
                        ConsoleField.Name,
                        ConsoleField.Generation,
                        ConsoleField.Model,
                        ConsoleField.Firmware,
                        ConsoleField.Storages,
                        ConsoleField.Folders,
                        ConsoleField.Accessories,
                        ConsoleField.Games,
                        ConsoleField.Accounts
                    },
                    [FieldsFilling.Default] = new List<ConsoleField>
                    {
                        ConsoleField.Icon,
                        ConsoleField.Name,
                        ConsoleField.Generation,
                        ConsoleField.Model
                    },
                    [FieldsFilling.Min] = new List<ConsoleField>
                    {
                        ConsoleField.Icon,
                        ConsoleField.Name,
                        ConsoleField.Generation
                    }
                },
                [FieldsVariant.Category] = new ConsoleFieldsFillingDictionary(),
                [FieldsVariant.QuickFilter] = new ConsoleFieldsFillingDictionary(),
                [FieldsVariant.QuickFilterText] = new ConsoleFieldsFillingDictionary(),
                [FieldsVariant.Summary] = new ConsoleFieldsFillingDictionary(),
                [FieldsVariant.BatchUpdate] = new ConsoleFieldsFillingDictionary()
            };

        public override ConsoleField FieldMetaData => ConsoleField.Field;

        public override ConsoleField TitleField => ConsoleField.Name;

        public override ConsoleField UniqueField => ConsoleField.Id;

        public override List<ConsoleField> GetFieldsInternal(FieldsVariant variant, FieldsFilling filling)
        {
            List<ConsoleField> result = new();

            if (!fieldDictionary.TryGetValue(variant, out var fillingDictionary))
                return result;

            if (!fillingDictionary.TryGetValue(filling, out var fields))
                return result;

            result.AddRange(fields);
            return result;
        }

        protected override List<ConsoleField> GetCalcedFields() => new();

        protected override List<ConsoleField> GetIconFields() =>
            new()
            {
                ConsoleField.Icon,
                ConsoleField.Name,
                ConsoleField.Generation,
                ConsoleField.Model,
                ConsoleField.Firmware
            };

        protected override List<ConsoleField> GetEditedFieldsExtended() =>
            EditingFields;

        protected override List<ConsoleField> GetFullInfoFields() =>
            new()
            {
                ConsoleField.Icon,
                ConsoleField.Name,
                ConsoleField.Generation,
                ConsoleField.Model,
                ConsoleField.Firmware,
                ConsoleField.Storages,
                ConsoleField.Folders,
                ConsoleField.Accessories,
                ConsoleField.Games,
                ConsoleField.Accounts
            };

        protected override List<ConsoleField> GetCardFields() =>
            new()
            {
                ConsoleField.Icon,
                ConsoleField.Name,
                ConsoleField.Generation,
                ConsoleField.Model,
                ConsoleField.Firmware,
                ConsoleField.Storages,
                ConsoleField.Folders,
                ConsoleField.Accessories,
                ConsoleField.Games,
                ConsoleField.Accounts
            };

        protected override FilterOperation GetDefaultFilterOperation(ConsoleField field) =>
            field switch
            {
                ConsoleField.Name or
                ConsoleField.Storages or
                ConsoleField.Folders or
                ConsoleField.Accessories or
                ConsoleField.Games or
                ConsoleField.Accounts =>
                    FilterOperation.Contains,
                _ =>
                    FilterOperation.Equals
            };

        protected override FieldFilterOperations<ConsoleField> GetAvailableFilterOperations() => new();

        protected override List<ConsoleField> GetGroupByFields() => new();

        public override List<ConsoleField> Depended(ConsoleField field) => 
            field switch
            {
                ConsoleField.Model => new List<ConsoleField>() 
                { 
                    ConsoleField.Generation 
                },
                ConsoleField.Accounts => new List<ConsoleField>()
                { 
                    ConsoleField.Generation,
                    ConsoleField.Firmware
                },
                _ => base.Depended(field),
            };

        protected override List<ConsoleField> GetSelectQuickFilterFields() =>
            new()
            {
                ConsoleField.Generation,
                ConsoleField.Model,
                ConsoleField.Firmware
            };

        public override DataGridViewContentAlignment ColumnAlign(ConsoleField field) =>
            field == ConsoleField.Name 
                ? DataGridViewContentAlignment.MiddleLeft
                : DataGridViewContentAlignment.MiddleCenter;

        public override int ColumnWidth(ConsoleField field) =>
            field switch
            {
                ConsoleField.Name => 
                    200,
                ConsoleField.Generation or
                ConsoleField.Model =>
                    100,
                ConsoleField.Firmware =>
                    90,
                ConsoleField.Storages or
                ConsoleField.Folders or
                ConsoleField.Accessories or
                ConsoleField.Games =>
                    75,
                _ => base.ColumnWidth(field)
            };

        public override FieldType GetFieldType(ConsoleField field) => 
            field switch
            {
                ConsoleField.Field => 
                    FieldType.MetaData,
                ConsoleField.Console => 
                    FieldType.Extract,
                ConsoleField.Id => 
                    FieldType.Guid,
                ConsoleField.Name => 
                    FieldType.String,
                ConsoleField.Generation or 
                ConsoleField.Model or 
                ConsoleField.Firmware => 
                    FieldType.Enum,
                ConsoleField.Storages or 
                ConsoleField.Folders or
                ConsoleField.Accessories or
                ConsoleField.Games or
                ConsoleField.Accounts =>
                    FieldType.List,
                ConsoleField.Icon =>
                    FieldType.Image,
                _ => 
                    FieldType.String,
            };

        public override string ColumnCaption(ConsoleField field) =>
            field == ConsoleField.Icon 
                ? string.Empty 
                : base.ColumnCaption(field);

        public override ITypeHelper? GetHelper(ConsoleField field) => 
            field switch
            {
                ConsoleField.Generation => TypeHelper.Helper<ConsoleGenerationHelper>(),
                ConsoleField.Model => TypeHelper.Helper<ConsoleModelHelper>(),
                ConsoleField.Firmware => TypeHelper.Helper<FirmwareTypeHelper>(),
                _ => null
            };
    }
}
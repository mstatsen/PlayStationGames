using PlayStationGames.ConsoleEngine.ControlFactory.Controls;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Grid;
using PlayStationGames.ConsoleEngine.View;
using OxXMLEngine.ControlFactory;
using OxXMLEngine.ControlFactory.Accessors;
using OxXMLEngine.ControlFactory.Context;
using OxXMLEngine.Grid;
using OxXMLEngine.View;

using OxXMLEngine.ControlFactory.Initializers;
using PlayStationGames.ConsoleEngine.ControlFactory.Initializers;

namespace PlayStationGames.ConsoleEngine.ControlFactory
{
    public class ConsoleControlFactory : ControlFactory<ConsoleField, PSConsole>
    {
        protected override IControlAccessor? CreateOtherAccessor(IBuilderContext<ConsoleField, PSConsole> context) => 
            context is FieldContext<ConsoleField, PSConsole> accessorContext
                ? accessorContext.Field switch
                {
                    ConsoleField.Generation => CreateEnumAccessor<ConsoleGeneration>(context),
                    ConsoleField.Model => CreateEnumAccessor<ConsoleModel>(context),
                    ConsoleField.Firmware => CreateEnumAccessor<FirmwareType>(context),
                    ConsoleField.Storages => CreateListAccessor<Storage, Storages, StoragesControl>(context),
                    ConsoleField.Folders => CreateListAccessor<Folder, Folders, FoldersControl>(context),
                    ConsoleField.Accessories => CreateListAccessor<Accessory, Accessories, AccessoriesControl>(context),
                    _ => base.CreateOtherAccessor(context),
                }
                : 
            context.Name == "AccessoryType"
                ? CreateEnumAccessor<AccessoryType>(context)
                : context.Name == "JoystickType"
                    ? CreateEnumAccessor<JoystickType>(context)
                    : base.CreateOtherAccessor(context);

        protected override IInitializer? Initializer(IBuilderContext<ConsoleField, PSConsole> context) => 
            context.Name switch
            {
                "StorageSelector" => new ExtractInitializer<ConsoleField, PSConsole>(ConsoleField.Storages, true),
                "FolderSelector" => new ExtractInitializer<ConsoleField, PSConsole>(ConsoleField.Folders, true),
                "AccessoryType" => new AccessoryTypeInitializer((PSConsole)context.AdditionalContext!),
                "JoystickType" => new JoystickTypeInitializer((PSConsole)context.AdditionalContext!),
                _ => base.Initializer(context),
            };

        public override IItemInfo<ConsoleField, PSConsole> CreateInfoCard() =>
            new ConsoleFullInfoCard();

        public override GridPainter<ConsoleField, PSConsole> CreateGridPainter(
            GridFieldColumns<ConsoleField> columns, GridUsage usage) => new ConsolesGridPainter(columns);

        public override IItemCard<ConsoleField, PSConsole> CreateCard(ItemViewMode viewMode) => 
            new ConsoleCard(viewMode);

        protected override ItemColorer<ConsoleField, PSConsole> CreateItemColorer() =>
            new ConsoleColorer();
    }
}
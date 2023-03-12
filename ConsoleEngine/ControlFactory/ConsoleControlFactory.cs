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

namespace PlayStationGames.ConsoleEngine.ControlFactory
{
    public class ConsoleControlFactory : ControlFactory<ConsoleField, PSConsole>
    {
        protected override IControlAccessor? CreateOtherAccessor(IBuilderContext<ConsoleField, PSConsole> context)
        {
            if (context is FieldContext<ConsoleField, PSConsole> accessorContext)
                switch (accessorContext.Field)
                {
                    case ConsoleField.Generation:
                        return CreateEnumAccessor<ConsoleGeneration>(context);
                    case ConsoleField.Model:
                        return CreateEnumAccessor<ConsoleModel>(context);
                    case ConsoleField.Firmware:
                        return CreateEnumAccessor<FirmwareType>(context);
                    case ConsoleField.Storages:
                        return CreateListAccessor<Storage, Storages, StoragesControl>(context);
                    case ConsoleField.Folders:
                        return CreateListAccessor<Folder, Folders, FoldersControl>(context);
                }

            return base.CreateOtherAccessor(context);
        }

        protected override IInitializer? Initializer(IBuilderContext<ConsoleField, PSConsole> context)
        {
            if (context.Name == "StorageSelector")
                return new ExtractInitializer<ConsoleField, PSConsole>(ConsoleField.Storages, true);

            if (context.Name == "FolderSelector")
                return new ExtractInitializer<ConsoleField, PSConsole>(ConsoleField.Folders, true);

            return base.Initializer(context);
        }

        public override IItemView<ConsoleField, PSConsole> CreateInfoCard() =>
            new ConsoleFullInfoCard();

        public override GridPainter<ConsoleField, PSConsole> CreateGridPainter(
            GridFieldColumns<ConsoleField> columns, GridUsage usage) => new ConsolesGridPainter(columns);

        public override IItemCard<ConsoleField, PSConsole> CreateCard(ItemViewMode viewMode) => 
            new ConsoleCard(viewMode);

        protected override ItemColorer<ConsoleField, PSConsole> CreateItemColorer() =>
            new ConsoleColorer();
    }
}
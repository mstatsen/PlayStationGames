using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Context;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Grid;
using OxDAOEngine.View;
using PlayStationGames.AccountEngine.ControlFactory.Accessors;
using PlayStationGames.ConsoleEngine.ControlFactory.Controls;
using PlayStationGames.ConsoleEngine.ControlFactory.Initializers;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Grid;
using PlayStationGames.ConsoleEngine.View;

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
                    ConsoleField.Accounts => CreateListAccessor<ConsoleAccount, ConsoleAccounts, AccountsControl>(context),
                    _ => base.CreateOtherAccessor(context),
                }
                : 
            context.Name == "AccessoryType"
                ? CreateEnumAccessor<AccessoryType>(context)
                : context.Name == "JoystickType"
                    ? CreateEnumAccessor<JoystickType>(context)
                    : context.Name == "ConsoleAccount"
                        ? CreateAccountAccessor(context)
                        : base.CreateOtherAccessor(context);

        private static IControlAccessor CreateAccountAccessor(IBuilderContext<ConsoleField, PSConsole> context) =>
            new AccountAccessor<ConsoleField, PSConsole>(context);

        protected override IInitializer? Initializer(IBuilderContext<ConsoleField, PSConsole> context) => 
            context.Name switch
            {
                "StorageSelector" => new ExtractInitializer<ConsoleField, PSConsole>(
                    ConsoleField.Storages, 
                    fullExtract: true, 
                    fixedExtract: true
                ),
                "FolderSelector" => new ExtractInitializer<ConsoleField, PSConsole>(
                    ConsoleField.Folders, 
                    fullExtract: true, 
                    fixedExtract: true),
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
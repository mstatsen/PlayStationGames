using Microsoft.VisualBasic.FileIO;
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
            context.Key switch
            {
                "Storage:Name" or
                "Storage:Size" or
                "Storage:FreeSize" =>
                    CreateTextBoxAccessor(context),
                "Storage:Placement" =>
                    CreateEnumAccessor<StoragePlacement>(context),
                "Folder:Name" =>
                    CreateTextBoxAccessor(context),
                "Accessory:Type" =>
                    CreateEnumAccessor<AccessoryType>(context),
                "Joystick:Type" =>
                    CreateEnumAccessor<JoystickType>(context),
                "ConsoleAccount" =>
                    CreateAccountAccessor(context),
                _ =>
                    base.CreateOtherAccessor(context)
            };

        private static IControlAccessor CreateAccountAccessor(IBuilderContext<ConsoleField, PSConsole> context) =>
            new AccountAccessor<ConsoleField, PSConsole>(context);

        protected override IInitializer? Initializer(IBuilderContext<ConsoleField, PSConsole> context)
        {
            if (context is FieldContext<ConsoleField, PSConsole> fieldContext)
                switch (fieldContext.Field)
                {
                    case ConsoleField.Console:
                        return new ConsoleInitializer(ConsoleField.Console, false, true, true);
                }

            return context.Key switch
            {
                "Installation:Storage" => 
                    CreateExtractInitializer(ConsoleField.Storages),
                "Installation:Folder" =>
                    CreateExtractInitializer(ConsoleField.Folders),
                "Accessory:Type" => 
                    new AccessoryTypeInitializer((PSConsole)context.AdditionalContext!),
                "Joystick:Type" => 
                    new JoystickTypeInitializer((PSConsole)context.AdditionalContext!),
                _ => 
                    base.Initializer(context),
            };
        }

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
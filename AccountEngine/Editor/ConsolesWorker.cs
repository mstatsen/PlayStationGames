using OxLibrary;
using OxXMLEngine.ControlFactory;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Filter;
using OxXMLEngine.Grid;
using PlayStationGames.AccountEngine.ControlFactory.ValueAccessors;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.AccountEngine.Editor
{
    public class ConsolesWorker
    {
        public Color BaseColor { get; set; } = Styles.CardColor;

        private static ControlBuilder<AccountField, Account> Builder =>
            DataManager.Builder<AccountField, Account>(ControlScope.Editor);

        private Account? account;

        private bool CanUnselectItemHandler(PSConsole currentItem, RootListDAO<ConsoleField, PSConsole> selectedList)
        {
            if (account == null)
                return false;

            currentItem.Accounts.Remove((a) => a.Id == account.Id);
            return true;
        }

        private bool CanSelectItemHandler(PSConsole currentItem, RootListDAO<ConsoleField, PSConsole> selectedList)
        {
            if (account == null)
                return false;

            currentItem.Accounts.Add(account);
            return true;
        }

        private readonly IListController<ConsoleField, PSConsole> ConsolesController = 
            DataManager.ListController<ConsoleField, PSConsole>();

        private RootListDAO<ConsoleField, PSConsole> FullConsolesList =>
            ConsolesController.FullItemsList;

        public void Show()
        {
            if (account == null)
                return;

            RootListDAO<ConsoleField, PSConsole> availableConsoles = new();
            availableConsoles.CopyFrom(FullConsolesList.FilteredList(SuilableConsolesFilter));

            ItemsChooserParams<ConsoleField, PSConsole> chooserParams = new(
                availableConsoles, existsConsoles)
            {
                Title = "Consoles",
                AvailableTitle = "Available Consoles",
                SelectedTitle = "Exists Consoles",
                SelectedGridFields = new List<ConsoleField>()
                {
                    ConsoleField.Icon,
                    ConsoleField.Name,
                    ConsoleField.Generation,
                    ConsoleField.Model,
                    ConsoleField.Firmware
                },
                BaseColor = BaseColor
            };

            chooserParams.CanSelectItem += CanSelectItemHandler;
            chooserParams.CanUnselectItem += CanUnselectItemHandler;

            if (ItemsChooser<ConsoleField, PSConsole>.ChooseItems(chooserParams, out RootListDAO<ConsoleField, PSConsole> selectedConsoles))
                existsConsoles.LinkedCopyFrom(selectedConsoles);

            selectedConsoles.Clear();
        }

        private readonly RootListDAO<ConsoleField, PSConsole> existsConsoles = new();

        private IMatcher<ConsoleField> SuilableConsolesFilter
        {
            get
            {
                Filter<ConsoleField, PSConsole> filter = new();

                if (account != null)
                {
                    filter.AddFilter(ConsoleField.Generation, ConsoleGeneration.PS5, FilterConcat.OR);
                    filter.AddFilter(ConsoleField.Generation, ConsoleGeneration.PS4, FilterConcat.OR);
                    filter.AddFilter(ConsoleField.Generation, ConsoleGeneration.PS3, FilterConcat.OR);
                    filter.AddFilter(ConsoleField.Generation, ConsoleGeneration.PSVita, FilterConcat.OR);
                    SimpleFilter<ConsoleField, PSConsole> pspFilter = new();
                    pspFilter.AddFilter(ConsoleField.Generation, ConsoleGeneration.PSP);
                    pspFilter.AddFilter(ConsoleField.Firmware, FirmwareType.Official);
                    filter.AddFilter(pspFilter, FilterConcat.OR);
                }

                return filter;
            }
        }

        public void Renew(Account? account)
        {
            this.account = account;
            existsConsoles.Clear();

            if (account != null)
                foreach (PSConsole console in DataManager.FullItemsList<ConsoleField, PSConsole>()
                    .FindAll((c) => c.Accounts.GetById(account.Id) != null))
                {
                    PSConsole tempConsole = new();
                    tempConsole.CopyFrom(console);
                    existsConsoles.Add(tempConsole);
                }
        }

        public void Save()
        {
            if (account == null)
                return;

            RenewOwners();
            existsConsoles.Clear();
        }

        private void RenewOwners()
        {
            if (account == null)
                return;

            FullConsolesList.StartSilentChange();

            try
            {
                foreach (PSConsole console in FullConsolesList
                    .FindAll((c) => c.Accounts.GetById(account.Id) != null &&
                            !existsConsoles.Contains((c2) => c2.Id == c.Id)
                        )
                    )
                    console.Accounts.RemoveAll((a) => a.Id == account.Id);
                    

                foreach (PSConsole console in existsConsoles)
                    ConsolesController.Item(ConsoleField.Id, console.Id)?.Accounts.Add(account);
            }
            finally
            {
                FullConsolesList.FinishSilentChange();

                foreach (PSConsole console in FullConsolesList.FindAll((c) => c.Accounts.GetById(account.Id) != null))
                    console.ChangeHandler?.Invoke(console, new DAOEntityEventArgs(DAOOperation.Modify));

                ConsolesController.ListChanged?.Invoke(ConsolesController, new EventArgs());
            }
        }
    }
}
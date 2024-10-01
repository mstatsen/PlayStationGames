using OxLibrary;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Grid;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using OxDAOEngine.Data.Types;
using OxLibrary.Dialogs;

namespace PlayStationGames.AccountEngine.Editor
{
    public class ConsolesWorker
    {
        public Color BaseColor { get; set; } = Styles.CardColor;

        private Account? account;

        private CanSelectResult CanUnselectItemHandler(PSConsole currentItem, 
            RootListDAO<ConsoleField, PSConsole> selectedList, 
            ItemsChooser<ConsoleField, PSConsole> chooser)
        {
            if (account == null)
                return CanSelectResult.Return;

            currentItem.Accounts.Remove((a) => a.Id == account.Id);
            return CanSelectResult.Available;
        }

        private CanSelectResult CanSelectItemHandler(PSConsole currentItem, 
            RootListDAO<ConsoleField, PSConsole> selectedList, 
            ItemsChooser<ConsoleField, PSConsole> chooser)
        {
            if (account == null)
                return CanSelectResult.Return;

            if (currentItem.Accounts.Count >= TypeHelper.Helper<ConsoleGenerationHelper>()
                .MaxAccountsCount(currentItem.Generation, currentItem.Firmware))
            {
                OxMessage.ShowError($"Console \"{currentItem.Name}\" already contains maximum count of possible registered accounts.", chooser);
                return CanSelectResult.Continue;
            }

            currentItem.Accounts.Add(account);
            return CanSelectResult.Available;
        }

        private readonly IListController<ConsoleField, PSConsole> ConsolesController = 
            DataManager.ListController<ConsoleField, PSConsole>();

        private RootListDAO<ConsoleField, PSConsole> FullConsolesList =>
            ConsolesController.FullItemsList;

        public void Show(Control owner)
        {
            if (account == null)
                return;

            RootListDAO<ConsoleField, PSConsole> availableConsoles = new();
            availableConsoles.CopyFrom(FullConsolesList.FilteredList(SuilableConsolesFilter));

            ItemsChooserParams<ConsoleField, PSConsole> chooserParams = new(
                availableConsoles, registeredConsoles)
            {
                Title = "Consoles",
                AvailableTitle = "Available consoles",
                SelectedTitle = "Registered on consoles",
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

            if (ItemsChooser<ConsoleField, PSConsole>.ChooseItems(owner, 
                chooserParams, 
                out RootListDAO<ConsoleField, PSConsole> selectedConsoles))
            {
                registeredConsoles.LinkedCopyFrom(selectedConsoles);
                Modified = true;
            }

            selectedConsoles.Clear();
        }

        private readonly RootListDAO<ConsoleField, PSConsole> registeredConsoles = new();

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

        public int ConsolesCount => registeredConsoles.Count;

        public void Renew(Account? account)
        {
            Modified = false;
            this.account = account;
            registeredConsoles.Clear();

            if (account != null)
                foreach (PSConsole console in DataManager.FullItemsList<ConsoleField, PSConsole>()
                    .FindAll((c) => c.Accounts.GetById(account.Id) != null))
                {
                    PSConsole tempConsole = new();
                    tempConsole.CopyFrom(console);
                    registeredConsoles.Add(tempConsole);
                }
        }

        public void Save()
        {
            if (account == null || !Modified)
                return;

            RenewOwners();
            registeredConsoles.Clear();
            Modified = false;
        }

        private bool Modified = false;

        private void RenewOwners()
        {
            if (account == null)
                return;

            FullConsolesList.StartSilentChange();

            try
            {
                foreach (PSConsole console in FullConsolesList
                    .FindAll((c) => c.Accounts.GetById(account.Id) != null)
                    )
                    console.Accounts.RemoveAll((a) => a.Id == account.Id);

                foreach (PSConsole console in registeredConsoles)
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
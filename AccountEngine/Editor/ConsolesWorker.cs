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
using OxDAOEngine.Data.Filter.Types;

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
            if (account is null)
                return CanSelectResult.Return;

            currentItem.Accounts.Remove((a) => a.Id == account.Id);
            return CanSelectResult.Available;
        }

        private CanSelectResult CanSelectItemHandler(PSConsole currentItem, 
            RootListDAO<ConsoleField, PSConsole> selectedList, 
            ItemsChooser<ConsoleField, PSConsole> chooser)
        {
            if (account is null)
                return CanSelectResult.Return;

            if (currentItem.Accounts.Count >= TypeHelper.Helper<ConsoleGenerationHelper>()
                .MaxAccountsCount(currentItem.Generation, currentItem.Firmware) 
                && !currentItem.Accounts.Contains(c => c.Id == account.Id))
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
            if (account is null)
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
                Filter<ConsoleField, PSConsole> filter = new(FilterConcat.OR);

                if (account is not null)
                {
                    FilterGroup<ConsoleField, PSConsole> baseGenerationGroup = filter.AddGroup(FilterConcat.OR);
                    baseGenerationGroup.Add(ConsoleField.Generation, ConsoleGeneration.PS5);
                    baseGenerationGroup.Add(ConsoleField.Generation, ConsoleGeneration.PS4);
                    baseGenerationGroup.Add(ConsoleField.Generation, ConsoleGeneration.PS3);
                    baseGenerationGroup.Add(ConsoleField.Generation, ConsoleGeneration.PSVita);
                    FilterGroup<ConsoleField, PSConsole> pspGroup = filter.AddGroup(FilterConcat.AND);
                    pspGroup.Add(ConsoleField.Generation, ConsoleGeneration.PSP);
                    pspGroup.Add(ConsoleField.Firmware, FirmwareType.Official);
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

            if (account is not null)
                foreach (PSConsole console in DataManager.FullItemsList<ConsoleField, PSConsole>()
                    .FindAll((c) => c.Accounts.GetById(account.Id) is not null))
                {
                    PSConsole tempConsole = new();
                    tempConsole.CopyFrom(console);
                    registeredConsoles.Add(tempConsole);
                }
        }

        public void Save()
        {
            if (account is null 
                || !Modified)
                return;

            RenewOwners();
            registeredConsoles.Clear();
            Modified = false;
        }

        private bool Modified = false;

        private void RenewOwners()
        {
            if (account is null)
                return;

            FullConsolesList.StartSilentChange();

            try
            {
                foreach (PSConsole console in FullConsolesList
                    .FindAll((c) => c.Accounts.GetById(account.Id) is not null)
                    )
                    console.Accounts.RemoveAll((a) => a.Id.Equals(account.Id));

                foreach (PSConsole console in registeredConsoles)
                    ConsolesController.Item(ConsoleField.Id, console.Id)?.Accounts.Add(account);
            }
            finally
            {
                FullConsolesList.FinishSilentChange();

                foreach (PSConsole console in FullConsolesList.FindAll(c => 
                            c.Accounts.GetById(account.Id) is not null)
                        )
                    console.ChangeHandler?.Invoke(console, new DAOEntityEventArgs(DAOOperation.Modify));

                ConsolesController.ListChanged?.Invoke(ConsolesController, new EventArgs());
            }
        }
    }
}
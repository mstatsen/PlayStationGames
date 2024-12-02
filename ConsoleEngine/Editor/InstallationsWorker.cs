using OxLibrary;
using OxLibrary.Forms;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Data.Filter.Types;
using OxDAOEngine.Grid;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.Editor
{
    public class InstallationsWorker
    {
        public Color BaseColor { get; set; } = OxStyles.CardColor;

        private static ControlBuilder<ConsoleField, PSConsole> Builder =>
            DataManager.Builder<ConsoleField, PSConsole>(ControlScope.Editor);

        private PSConsole? Console;

        private CanSelectResult CanUnselectItemHandler(Game currentItem, 
            RootListDAO<GameField, Game> selectedList, 
            ItemsChooser<GameField, Game> chooser)
        {
            if (Console is null)
                return CanSelectResult.Return;

            bool uninstallAvailable = !needShowUninstallMessage
                || OxMessage.ShowConfirm(
                    $"Are you sure to want uninstall {(selectedList.Count > 1 ? "selected games" : currentItem.FullTitle())}?", 
                    chooser) is DialogResult.Yes;

            needShowUninstallMessage = false;

            if (uninstallAvailable)
            {
                currentItem.Installations.RemoveAll(i => i.ConsoleId.Equals(Console.Id));
                return CanSelectResult.Available;
            }

            return CanSelectResult.Return;
        }

        private readonly InstallationPlaceSelector placeSelector = new();
        private bool ApplyPlacementForAll = false;
        private bool needShowUninstallMessage = true;

        private CanSelectResult CanSelectItemHandler(Game currentItem, 
            RootListDAO<GameField, Game> selectedList, 
            ItemsChooser<GameField, Game> chooser)
        {
            if (Console is null)
                return CanSelectResult.Return;

            if (!ApplyPlacementForAll)
            {
                placeSelector.GamesCount = selectedList.Count;
                placeSelector.Game = currentItem;
                DialogResult result = placeSelector.ShowAsDialog(chooser);

                if (result.Equals(OxDialogButtonsHelper.Result(OxDialogButton.Cancel)))
                    return CanSelectResult.Return;

                ApplyPlacementForAll = result.Equals(OxDialogButtonsHelper.Result(OxDialogButton.ApplyForAll));
            }

            currentItem.Installations.Add(
                new Installation()
                {
                    ConsoleId = Console.Id,
                    StorageId = placeSelector.SelectedStorageId,
                    Folder = placeSelector.SelectedFolderName
                }
            );

            return CanSelectResult.Available;
        }

        public void Show(Control owner)
        {
            if (Console is null)
                return;

            RootListDAO<GameField, Game> availableGames = new();
            availableGames.CopyFrom(DataManager.FullItemsList<GameField, Game>().FilteredList(SuilableGamesFilter));
            availableGames.RemoveAll(
                g => g.AccountAvailable 
                    && !Console.Accounts.Contains(
                         a => a.Id.Equals(g.Owner)
                        ), 
                false
            );

            ItemsChooserParams<GameField, Game> chooserParams = new(
                availableGames, installedGames)
            {
                Title = "Games",
                AvailableTitle = "Available for install",
                SelectedTitle = "Installed",
                SelectButtonTip = "Install games",
                UnselectButtonTip = "Uninstall games",
                SelectedGridFields = new List<GameField>()
                {
                    GameField.Image,
                    GameField.Name
                },
                SelectedGridAdditionalColumns = new()
                {
                    new CustomGridColumn<GameField, Game>("Storage", GameStorageGetter, 80),
                    new CustomGridColumn<GameField, Game>("Folder", GameFolderGetter)
                },
                BaseColor = BaseColor
            };

            chooserParams.CanSelectItem += CanSelectItemHandler;
            chooserParams.CompleteSelect += CompleteSelectHandler;
            chooserParams.CanUnselectItem += CanUnselectItemHandler;

            if (ItemsChooser<GameField, Game>.ChooseItems(owner, chooserParams, out RootListDAO<GameField, Game> selectedGames))
            {
                installedGames.LinkedCopyFrom(selectedGames);
                Modified = true;
            }

            selectedGames.Clear();
        }

        private object? GameStorageGetter(Game item)
        {
            if (Console is null)
                return null;

            Guid? storageId = item.Installations.Find(i => i.ConsoleId.Equals(Console.Id))?.StorageId;

            if (storageId is null)
                return null;

            return Console.Storages.Find(s => s.Id.Equals(storageId))?.Name;
        }

        private object? GameFolderGetter(Game item)
        {
            if (Console is null)
                return null;

            return item.Installations.Find(i => i.ConsoleId.Equals(Console.Id))?.Folder;
        }

        private void CompleteSelectHandler(object? sender, EventArgs e)
        {
            ApplyPlacementForAll = false;
            needShowUninstallMessage = true;
        }

        private readonly RootListDAO<GameField, Game> installedGames = new();

        public int InstalledGamesCount => installedGames.Count;

        private static IMatcher<GameField> SuilableGamesFilter
        {
            get
            {
                SuitableConsoleGames suitableConsoleGames = SuitableConsoleGames.SuitableFor(
                    Builder.Value<ConsoleGeneration>(ConsoleField.Generation),
                    Builder.Value<ConsoleModel>(ConsoleField.Model),
                    Builder.Value<FirmwareType>(ConsoleField.Firmware)
                );

                Filter<GameField, Game> filter = new(FilterConcat.OR);

                foreach (SuitableConsoleGame suitableGame in suitableConsoleGames)
                {
                    FilterGroup<GameField, Game> group = filter.AddGroup(FilterConcat.AND);
                    group.Add(GameField.Platform, suitableGame.PlatformType);
                    group.Add(GameField.Source, suitableGame.Source);
                    group.Add(GameField.Licensed, suitableGame.Licensed);
                }

                return filter;
            }
        }

        public void Renew(PSConsole? console)
        {
            Modified = false;
            Console = console;
            installedGames.Clear();

            if (console is not null)
                foreach (Game game in DataManager.FullItemsList<GameField, Game>())
                    if (game.Installations.Contains(i => i.ConsoleId.Equals(console.Id)))
                    {
                        Game tempGame = new();
                        tempGame.CopyFrom(game);
                        installedGames.Add(tempGame);
                    }
        }

        public void Save()
        {
            if (Console is null 
                || !Modified)
                return;

            RemoveInactiveInstallations();
            RenewInstallations();
            installedGames.Clear();
            Modified = false;
        }

        private bool Modified = false;

        private void RenewInstallations()
        {
            if (Console is null)
                return;

            foreach (Game installedGame in installedGames)
            {
                Game? game = DataManager.Item<GameField, Game>(GameField.Id, installedGame.Id);

                if (game is null)
                    continue;
                
                Installation? newInstallation = installedGame.Installations.Find(i => i.ConsoleId.Equals(Console.Id));

                if (newInstallation is null)
                    continue;

                Installation? currentInstallation = game.Installations.Find(i => i.ConsoleId.Equals(Console.Id));

                if (currentInstallation is null)
                    game.Installations.Add(newInstallation);
                else
                    currentInstallation.CopyFrom(newInstallation);
            }
        }

        private void RemoveInactiveInstallations()
        {
            if (Console is null)
                return;

            foreach (Game game in DataManager.FullItemsList<GameField, Game>()
                    .FindAll(g => g.Installations.Contains(i => i.ConsoleId.Equals(Console.Id))))
                if (!installedGames.Contains(g => g.Id.Equals(game.Id)))
                    game.Installations.Remove(i => i.ConsoleId.Equals(Console.Id));
        }
    }
}
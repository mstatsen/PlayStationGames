using OxLibrary;
using OxLibrary.Dialogs;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Filter;
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
        public Color BaseColor { get; set; } = Styles.CardColor;

        private static ControlBuilder<ConsoleField, PSConsole> Builder =>
            DataManager.Builder<ConsoleField, PSConsole>(ControlScope.Editor);

        private PSConsole? Console;

        private bool CanUnselectItemHandler(Game currentItem, RootListDAO<GameField, Game> selectedList)
        {
            if (Console == null)
                return false;

            bool uninstallAvailable = !needShowUninstallMessage 
                || OxMessage.ShowConfirm(
                $"Are you sure to want uninstall {(selectedList.Count > 1 ? "selected games" : currentItem.FullTitle())}?"
            ) == DialogResult.Yes;

            needShowUninstallMessage = false;


            if (uninstallAvailable)
            {
                currentItem.Installations.RemoveAll(i => i.ConsoleId == Console.Id);
                return true;
            }

            return false;
        }

        private readonly InstallationPlaceSelector placeSelector = new();
        private bool ApplyPlacementForAll = false;
        private bool needShowUninstallMessage = true;

        private bool CanSelectItemHandler(Game currentItem, RootListDAO<GameField, Game> selectedList)
        {
            if (Console == null)
                return false;

            if (!ApplyPlacementForAll)
            {
                placeSelector.GamesCount = selectedList.Count;
                placeSelector.Game = currentItem;
                DialogResult result = placeSelector.ShowAsDialog();
                if (result == OxDialogButtonsHelper.Result(OxDialogButton.Cancel))
                    return false;
                    
                ApplyPlacementForAll = result == OxDialogButtonsHelper.Result(OxDialogButton.ApplyForAll);
            }

            currentItem.Installations.Add(
                new Installation()
                {
                    ConsoleId = Console.Id,
                    StorageId = placeSelector.SelectedStorageId,
                    Folder = placeSelector.SelectedFolderName
                }
            );

            return true;
        }

        public void Show()
        {
            if (Console == null)
                return;

            RootListDAO<GameField, Game> availableGames = new();
            availableGames.CopyFrom(DataManager.FullItemsList<GameField, Game>().FilteredList(SuilableGamesFilter));

            ItemsChooserParams<GameField, Game> chooserParams = new(
                availableGames, installedGames)
            {
                Title = "Installed Games",
                AvailableTitle = "Available Games",
                SelectedTitle = "Installed Games",
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

            if (ItemsChooser<GameField, Game>.ChooseItems(chooserParams, out RootListDAO<GameField, Game> selectedGames))
                installedGames.LinkedCopyFrom(selectedGames);

            selectedGames.Clear();
        }

        private object? GameStorageGetter(Game item)
        {
            if (Console == null)
                return null;

            Guid? storageId = item.Installations.Find(i => i.ConsoleId == Console.Id)?.StorageId;

            if (storageId == null)
                return null;

            return Console.Storages.Find(s => s.Id == storageId)?.Name;
        }

        private object? GameFolderGetter(Game item)
        {
            if (Console == null)
                return null;

            return item.Installations.Find(i => i.ConsoleId == Console.Id)?.Folder;
        }

        private void CompleteSelectHandler(object? sender, EventArgs e)
        {
            ApplyPlacementForAll = false;
            needShowUninstallMessage = true;
        }

        private readonly RootListDAO<GameField, Game> installedGames = new();

        private static IMatcher<GameField> SuilableGamesFilter
        {
            get
            {
                SuitableConsoleGames suitableConsoleGames = SuitableConsoleGames.SuitableFor(
                    Builder.Value<ConsoleGeneration>(ConsoleField.Generation),
                    Builder.Value<ConsoleModel>(ConsoleField.Model),
                    Builder.Value<FirmwareType>(ConsoleField.Firmware)
                );

                Filter<GameField, Game> filter = new();

                foreach (SuitableConsoleGame suitableGame in suitableConsoleGames)
                    filter.AddFilter(
                        new SimpleFilter<GameField, Game>()
                            .AddFilter(GameField.Platform, suitableGame.PlatformType)
                            .AddFilter(GameField.Source, suitableGame.Source)
                            .AddFilter(GameField.Licensed, suitableGame.Licensed),
                        FilterConcat.OR
                    );

                return filter;
            }
        }

        public void Renew(PSConsole? console)
        {
            Console = console;
            installedGames.Clear();

            if (console != null)
                foreach (Game game in DataManager.FullItemsList<GameField, Game>())
                    if (game.Installations.Contains(i => i.ConsoleId == console.Id))
                    {
                        Game tempGame = new();
                        tempGame.CopyFrom(game);
                        installedGames.Add(tempGame);
                    }
        }

        public void Save()
        {
            if (Console == null)
                return;

            RemoveInactiveInstallations();
            RenewInstallations();
            installedGames.Clear();
        }

        private void RenewInstallations()
        {
            if (Console == null)
                return;

            foreach (Game installedGame in installedGames)
            {
                Game? game = DataManager.Item<GameField, Game>(GameField.Id, installedGame.Id);

                if (game == null)
                    continue;

                game.Installations.RemoveAll(i => i.ConsoleId == Console.Id);

                Installation? newInstallation = installedGame.Installations.Find(i => i.ConsoleId == Console.Id);

                if (newInstallation != null)
                    game.Installations.Add(newInstallation);
            }
        }

        private void RemoveInactiveInstallations()
        {
            if (Console == null)
                return;

            foreach (Game game in DataManager.FullItemsList<GameField, Game>()
                    .FindAll(g => g.Installations.Contains(i => i.ConsoleId == Console.Id)))
                if (!installedGames.Contains(g => g.Id == game.Id))
                    game.Installations.Remove(i => i.ConsoleId == Console.Id);
        }
    }
}
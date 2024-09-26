using OxLibrary;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Grid;
using PlayStationGames.AccountEngine.ControlFactory.ValueAccessors;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;
using Microsoft.VisualBasic.FileIO;

namespace PlayStationGames.AccountEngine.Editor
{
    public class GamesWorker
    {
        public Color BaseColor { get; set; } = Styles.CardColor;

        private Account? account;

        private CanSelectResult CanUnselectItemHandler(Game currentItem, RootListDAO<GameField, Game> selectedList)
        {
            currentItem.Owner = AccountValueAccessor.NullAccount.Id;
            return CanSelectResult.Available;
        }

        private CanSelectResult CanSelectItemHandler(Game currentItem, RootListDAO<GameField, Game> selectedList)
        {
            if (account == null)
                return CanSelectResult.Return;

            currentItem.Owner = account.Id;
            return CanSelectResult.Available;
        }

        public void Show()
        {
            if (account == null)
                return;

            RootListDAO<GameField, Game> availableGames = new();
            availableGames.CopyFrom(DataManager.FullItemsList<GameField, Game>().FilteredList(SuilableGamesFilter));

            ItemsChooserParams<GameField, Game> chooserParams = new(
                availableGames, existsGames)
            {
                Title = "Games",
                AvailableTitle = "Available Games",
                SelectedTitle = "Exists Games",
                SelectedGridFields = new List<GameField>()
                {
                    GameField.Image,
                    GameField.Name,
                    GameField.Platform
                },
                BaseColor = BaseColor
            };

            chooserParams.CanSelectItem += CanSelectItemHandler;
            chooserParams.CanUnselectItem += CanUnselectItemHandler;

            if (ItemsChooser<GameField, Game>.ChooseItems(chooserParams, out RootListDAO<GameField, Game> selectedGames))
            {
                existsGames.LinkedCopyFrom(selectedGames);
                Modified = true;
            }

            selectedGames.Clear();
        }

        private readonly RootListDAO<GameField, Game> existsGames = new();

        private IMatcher<GameField> SuilableGamesFilter
        {
            get
            {
                Filter<GameField, Game> filter = new(FilterConcat.OR);

                if (account != null)
                {
                    FilterGroup<GameField, Game> psnGroup = filter.AddGroup(FilterConcat.OR);
                    psnGroup.Add(GameField.Source, FilterOperation.Equals, Source.PSN);
                    psnGroup.Add(GameField.Source, FilterOperation.Equals, Source.PlayAtHome);
                    psnGroup.Add(GameField.Source, FilterOperation.Equals, Source.PSPlus);
                    FilterGroup<GameField, Game> pkgjGroup = filter.AddGroup(FilterConcat.AND);
                    pkgjGroup.Add(pkgjGroup.Add(GameField.Source, FilterOperation.Equals, Source.PKGj));
                    pkgjGroup.Add(pkgjGroup.Add(GameField.Platform, FilterOperation.Equals, PlatformType.PSVita));
                }

                return filter;
            }
        }

        public void Renew(Account? account)
        {
            Modified = false;
            this.account = account;
            existsGames.Clear();

            if (account != null)
                foreach (Game game in DataManager.FullItemsList<GameField, Game>().FindAll((g) => g.Owner == account.Id))
                {
                    Game tempGame = new();
                    tempGame.CopyFrom(game);
                    existsGames.Add(tempGame);
                }
        }

        public void Save()
        {
            if (account == null || !Modified)
                return;

            RenewOwners();
            existsGames.Clear();
            Modified = false;
        }

        private bool Modified = false;

        private void RenewOwners()
        {
            if (account == null)
                return;

            IListController<GameField, Game> gamesController = DataManager.ListController<GameField, Game>();
            gamesController.FullItemsList.StartSilentChange();

            try
            {
                foreach (Game game in gamesController.FullItemsList.FindAll((g) => 
                        g.Owner == account.Id && 
                        !existsGames.Contains((g2) => g2.Id == g.Id)
                    )
                )
                    game.Owner = AccountValueAccessor.NullAccount.Id;

                foreach (Game game in existsGames)
                {
                    Game? foundGame = DataManager.ListController<GameField, Game>().Item(GameField.Id, game.Id);

                    if (foundGame != null)
                        foundGame.Owner = account.Id;
                }
            }
            finally
            {
                gamesController.FullItemsList.FinishSilentChange();

                foreach (Game game in gamesController.FullItemsList.FindAll((g) => g.Owner == account.Id))
                    game.ChangeHandler?.Invoke(game, new DAOEntityEventArgs(DAOOperation.Modify));

                gamesController.ListChanged?.Invoke(gamesController, new EventArgs());
            }
        }
    }
}
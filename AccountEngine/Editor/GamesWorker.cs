using OxLibrary;
using OxXMLEngine.ControlFactory;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Filter;
using OxXMLEngine.Grid;
using PlayStationGames.AccountEngine.ControlFactory.ValueAccessors;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;
using System.Security.Principal;

namespace PlayStationGames.AccountEngine.Editor
{
    public class GamesWorker
    {
        public Color BaseColor { get; set; } = Styles.CardColor;

        private static ControlBuilder<AccountField, Account> Builder =>
            DataManager.Builder<AccountField, Account>(ControlScope.Editor);

        private Account? account;

        private bool CanUnselectItemHandler(Game currentItem, RootListDAO<GameField, Game> selectedList)
        {
            currentItem.Owner = AccountValueAccessor.NullAccount.Id;
            return true;
        }

        private bool CanSelectItemHandler(Game currentItem, RootListDAO<GameField, Game> selectedList)
        {
            if (account == null)
                return false;

            currentItem.Owner = account.Id;
            return true;
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
                existsGames.LinkedCopyFrom(selectedGames);

            selectedGames.Clear();
        }

        private readonly RootListDAO<GameField, Game> existsGames = new();

        private IMatcher<GameField> SuilableGamesFilter
        {
            get
            {
                Filter<GameField, Game> filter = new();

                if (account != null)
                {
                    filter.AddFilter(GameField.Licensed, true, FilterConcat.AND);
                    filter.AddFilter(GameField.Source, Source.PSN, FilterConcat.OR);
                    filter.AddFilter(GameField.Source, Source.PlayAtHome, FilterConcat.OR);
                    filter.AddFilter(GameField.Source, Source.PSPlus, FilterConcat.OR);
                }

                return filter;
            }
        }

        public void Renew(Account? account)
        {
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
            if (account == null)
                return;

            RenewOwners();
            existsGames.Clear();
        }

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
using OxLibrary;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Grid;
using PlayStationGames.AccountEngine.ControlFactory.ValueAccessors;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;
using PlayStationGames.AccountEngine.Data.Types;
using OxDAOEngine.Data.Filter.Types;

namespace PlayStationGames.AccountEngine.Editor
{
    public class GamesWorker
    {
        public Color BaseColor { get; set; } = Styles.CardColor;

        private Account? account;

        private CanSelectResult CanUnselectItemHandler(Game currentItem, 
            RootListDAO<GameField, Game> selectedList, 
            ItemsChooser<GameField, Game> chooser)
        {
            currentItem.Owner = AccountValueAccessor.NullAccount.Id;
            return CanSelectResult.Available;
        }

        private CanSelectResult CanSelectItemHandler(Game currentItem, 
            RootListDAO<GameField, Game> selectedList, 
            ItemsChooser<GameField, Game> chooser)
        {
            if (account is null)
                return CanSelectResult.Return;

            currentItem.Owner = account.Id;
            return CanSelectResult.Available;
        }

        public void Show(Control owner)
        {
            if (account is null)
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

            if (ItemsChooser<GameField, Game>.ChooseItems(owner, chooserParams, out RootListDAO<GameField, Game> selectedGames))
            {
                existsGames.LinkedCopyFrom(selectedGames);
                Modified = true;
            }

            selectedGames.Clear();
        }

        private readonly RootListDAO<GameField, Game> existsGames = new();

        public int GamesCount => existsGames.Count;

        private IMatcher<GameField> SuilableGamesFilter
        {
            get
            {
                Filter<GameField, Game> filter = new(FilterConcat.OR);

                if (account is not null)
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

            if (account is not null)
                foreach (Game game in DataManager.FullItemsList<GameField, Game>().FindAll((g) => g.Owner == account.Id))
                {
                    Game tempGame = new();
                    tempGame.CopyFrom(game);
                    existsGames.Add(tempGame);
                }
        }

        public void Save()
        {
            if (account is null 
                || ClearOwnerForGamesForLocalAccount() 
                || !Modified)
                return;

            RenewOwners();
            existsGames.Clear();
            Modified = false;
        }

        private bool ClearOwnerForGamesForLocalAccount()
        {
            if (account is null 
                || account.Type is not AccountType.Local)
                return false;

            List<Game> owneredGameList = FullGameList.FindAll(g => g.Owner == account.Id);

                foreach (Game game in owneredGameList)
                    game.Owner = AccountValueAccessor.NullAccount.Id;

            return true;
        }

        private bool Modified = false;

        private readonly IListController<GameField, Game> gamesController = DataManager.ListController<GameField, Game>();
        private RootListDAO<GameField, Game> FullGameList => gamesController.FullItemsList;

        private void RenewOwners()
        {
            if (account is null)
                return;

            gamesController.FullItemsList.StartSilentChange();
            List<Game> updatedList = FullGameList.FindAll((g) =>
                g.Owner == account.Id 
                && !existsGames.Contains((g2) => g2.Id.Equals(g.Id))
            );

            try
            {
                foreach (Game game in updatedList)
                    game.Owner = AccountValueAccessor.NullAccount.Id;

                List<Game> updatedList2 = FullGameList.FindAll(g => existsGames.Contains(g2 => g2.Id == g.Id));

                foreach (Game game in updatedList2)
                    game.Owner = account.Id;

                updatedList.AddRange(updatedList2);
            }
            finally
            {
                gamesController.FullItemsList.FinishSilentChange();
            }
        }
    }
}
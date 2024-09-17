using OxLibrary;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class RelatedGamesControl : ListItemsControl<RelatedGames, RelatedGame, 
        RelatedGameEditor, GameField, Game>
    {
        protected override void InitButtons()
        {
            base.InitButtons();
            PrepareViewButton(
                CreateButton(OxIcons.eye),
                (s, e) => DataManager.ViewItem<GameField, Game>(GameField.Id, SelectedItem.GameId), 
                true);
        }

        protected override string GetText() => "Related Games";
    }
}
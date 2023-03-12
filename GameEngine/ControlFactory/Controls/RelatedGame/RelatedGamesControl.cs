using OxLibrary;
using OxXMLEngine.ControlFactory.Controls;
using OxXMLEngine.Data;
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
                ViewRelatedGameHandler, 
                true);
        }

        private void ViewRelatedGameHandler(object? sender, EventArgs e) =>
            DataManager.ViewItem<GameField, Game>(GameField.Id, SelectedItem.GameId);

        protected override string GetText() => "Related Games";
    }
}
using OxLibrary;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using OxLibrary.Controls;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class RelatedGamesControl : ListItemsControl<RelatedGames, RelatedGame, 
        RelatedGameEditor, GameField, Game>
    {
    protected override void InitButtons()
        {
            base.InitButtons();
            OxIconButton viewButton = CreateButton(OxIcons.Eye);
            viewButton.ToolTipText = "View the game";
            PrepareViewButton(
                viewButton,
                (s, e) =>
                {
                    if (SelectedItem != null)
                        DataManager.ViewItem<GameField, Game>(GameField.Id, SelectedItem.GameId);
                }, 
                true);
        }

        protected override string GetText() => "Related Games";

        protected override string ItemName() => "Related Game";
    }
}
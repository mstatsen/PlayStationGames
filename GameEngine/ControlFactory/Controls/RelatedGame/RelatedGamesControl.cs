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
        private bool availableTrophies = true;
        public bool AvailableTrophies 
        {
            get => availableTrophies;
            set => availableTrophies = value;
        }

        protected override void PrepareEditor(RelatedGameEditor editor) => 
            editor.AvailableTrophies = availableTrophies;

        protected override void InitButtons()
        {
            base.InitButtons();
            OxIconButton viewButton = CreateButton(OxIcons.Eye);
            viewButton.ToolTipText = "View the game";
            PrepareViewButton(
                viewButton,
                (s, e) => DataManager.ViewItem<GameField, Game>(GameField.Id, SelectedItem.GameId), 
                true);
        }

        protected override string GetText() => "Related Games";

        protected override string ItemName() => "Related Game";
    }
}
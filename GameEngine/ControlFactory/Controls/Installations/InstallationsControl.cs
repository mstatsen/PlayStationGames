using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using OxLibrary;
using OxLibrary.Controls;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class InstallationsControl : ListItemsControl<ListDAO<Installation>, Installation, InstallationEditor, GameField, Game> 
    {
        protected override void InitButtons()
        {
            base.InitButtons();
            OxIconButton viewButton = CreateButton(OxIcons.Eye);
            viewButton.ToolTipText = "View the console";

            PrepareViewButton(
                viewButton,
                (s, e) => DataManager.ViewItem<ConsoleField, PSConsole>(ConsoleField.Id, SelectedItem.ConsoleId),
                true);
        }

        protected override string GetText() => 
            "Installations";

        protected override string ItemName() => "Installation";
    }
}
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using OxLibrary;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class InstallationsControl : ListItemsControl<ListDAO<Installation>, Installation, InstallationEditor, GameField, Game> 
    {
        protected override void InitButtons()
        {
            base.InitButtons();
            PrepareViewButton(
                CreateButton(OxIcons.eye),
                (s, e) => DataManager.ViewItem<ConsoleField, PSConsole>(ConsoleField.Id, SelectedItem.ConsoleId),
                true);
        }

        protected override string GetText() => 
            "Installations";
    }
}
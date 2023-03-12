using OxLibrary;
using OxLibrary.Controls;
using OxXMLEngine.ControlFactory.Controls;
using OxXMLEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using System.Diagnostics;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class LinksListControl : ListItemsControl<ListDAO<Link>, Link, LinkEditor, GameField, Game>
    {
        protected override void InitButtons()
        {
            base.InitButtons();
            PrepareViewButton(CreateButton(OxIcons.go), GoUrlHandler, true);
        }

        private void GoUrlHandler(object? sender, EventArgs e) => 
            Process.Start(SelectedItem.Url);

        protected override string GetText() => "Links";
    }
}
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
            PrepareViewButton(
                CreateButton(OxIcons.go),
                (s, e) => Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = SelectedItem.Url,
                        UseShellExecute = true
                    }
                ), 
                true);
        }

        protected override string GetText() => "Links";
    }
}
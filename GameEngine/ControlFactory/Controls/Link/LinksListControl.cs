using OxLibrary;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using System.Diagnostics;
using OxLibrary.Controls;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class LinksListControl : ListItemsControl<ListDAO<Link>, Link, LinkEditor, GameField, Game>
    {
        protected override void InitButtons()
        {
            base.InitButtons();
            OxIconButton goButton = CreateButton(OxIcons.go);
            goButton.ToolTipText = "Follow the link";
            PrepareViewButton(
                goButton,
                (s, e) => Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = SelectedItem.Url,
                        UseShellExecute = true
                    }
                ), 
                true);
        }

        protected override string ItemName() => "Link";

        protected override string GetText() => "Links";
    }
}
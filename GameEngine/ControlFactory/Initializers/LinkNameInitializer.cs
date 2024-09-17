using OxLibrary.Controls;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Extract;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class LinkNameInitializer : EmptyControlInitializer
    {
        private readonly ListDAO<Link>? ExistingLinks;

        private void AddLinkNameToComboBox(string? linkName)
        {
            if (ComboBox!.Items.IndexOf(linkName) < 0
                && (ExistingLinks == null ||
                        !ExistingLinks.Contains(l => l.Name == linkName)))
                ComboBox!.Items.Add(linkName);
        }

        private OxComboBox? ComboBox;

        public override void InitControl(Control control)
        {
            ComboBox = (OxComboBox)control;
            ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            ComboBox.Items.Clear();
            AddLinkNameToComboBox("Stratege");
            AddLinkNameToComboBox("PSNProfiles");

            List<object> linksNames = new FieldExtractor<GameField, Game>(
                DataManager.FullItemsList<GameField, Game>()).Extract(GameField.Links, true);

            foreach (object linkName in linksNames)
                AddLinkNameToComboBox(linkName.ToString());

            if (ComboBox.Items.Count > 0)
                ComboBox.SelectedIndex = 0;
        }

        public LinkNameInitializer(ListDAO<Link>? existingLinks) =>
            ExistingLinks = existingLinks;
    }
}
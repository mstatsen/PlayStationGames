using OxLibrary.Controls;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Extract;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class TagNameInitializer : EmptyControlInitializer
    {
        private readonly ListDAO<Tag>? ExistingTags;

        private void AddTagNameToComboBox(string? tagName)
        {
            if (ComboBox!.Items.IndexOf(tagName) < 0
                && (ExistingTags == null ||
                        !ExistingTags.Contains(l => l.Name == tagName)))
                ComboBox!.Items.Add(tagName);
        }

        private OxComboBox? ComboBox;

        public override void InitControl(Control control)
        {
            ComboBox = (OxComboBox)control;
            ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            ComboBox.Items.Clear();

            List<object> tagNames = new FieldExtractor<GameField, Game>(
                DataManager.FullItemsList<GameField, Game>()).Extract(GameField.Tags, true);

            foreach (object tagName in tagNames)
                AddTagNameToComboBox(tagName.ToString());

            if (ComboBox.Items.Count > 0)
                ComboBox.SelectedIndex = 0;
        }

        public TagNameInitializer(ListDAO<Tag>? existingTags) =>
            ExistingTags = existingTags;
    }
}
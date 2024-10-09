using OxLibrary.Controls;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Extract;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls.Initializers
{
    public class SeriesNameInitializer : EmptyControlInitializer
    {
        private readonly ListDAO<Tag>? ExistingSeries;

        private void AddSeriesNameToComboBox(string? seriesName)
        {
            if (ComboBox!.Items.IndexOf(seriesName) < 0
                && (ExistingSeries == null ||
                        !ExistingSeries.Contains(l => l.Name == seriesName)))
                ComboBox!.Items.Add(seriesName);
        }

        private OxComboBox? ComboBox;

        public override void InitControl(Control control)
        {
            ComboBox = (OxComboBox)control;
            ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            ComboBox.Items.Clear();

            List<object> seriesNames = new FieldExtractor<GameField, Game>(
                DataManager.FullItemsList<GameField, Game>()).Extract(GameField.Series, true);

            foreach (object tagName in seriesNames)
                AddSeriesNameToComboBox(tagName.ToString());

            if (ComboBox.Items.Count > 0)
                ComboBox.SelectedIndex = 0;
        }

        public SeriesNameInitializer(ListDAO<Tag>? existingSeries) =>
            ExistingSeries = existingSeries;
    }
}
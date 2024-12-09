using OxLibrary.Controls;
using OxLibrary.Interfaces;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Extract;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls.Initializers;

public class SeriesNameInitializer : EmptyControlInitializer
{
    public readonly ListDAO<Series> ExistingSeries = new();

    private void AddSeriesNameToComboBox(string? seriesName)
    {
        if (ComboBox!.Items.IndexOf(seriesName) < 0
            && (ExistingSeries.Count is 0 
                || !ExistingSeries.Contains(l => l.Name.Equals(seriesName)))
            )
            ComboBox!.Items.Add(seriesName);
    }

    private OxComboBox? ComboBox;

    public override void InitControl(IOxControl control)
    {
        ComboBox = (OxComboBox)control;
        ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
        ComboBox.Items.Clear();

        List<object> seriesNames = new FieldExtractor<GameField, Game>(
            DataManager.FullItemsList<GameField, Game>()).Extract(GameField.Serieses, true);

        foreach (object seriesName in seriesNames)
            AddSeriesNameToComboBox(seriesName.ToString());

        if (ComboBox.Items.Count > 0)
            ComboBox.SelectedIndex = 0;
    }

    public SeriesNameInitializer() { }
}
using OxLibrary;
using OxDAOEngine;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Grid;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.Grid
{
    public class ConsolesGridPainter : GridPainter<ConsoleField, PSConsole>
    {
        public ConsolesGridPainter(GridFieldColumns<ConsoleField> columnsDictionary) : base(columnsDictionary)
        { }

        public override DataGridViewCellStyle GetCellStyle(PSConsole? console, ConsoleField field, bool selected = false)
        {
            DataGridViewCellStyle style = new()
            {
                BackColor = TypeHelper.BackColor(console?.Firmware),
                ForeColor = EngineStyles.DefaultGridFontColor
            };

            FontStyle fontStyle = FontStyle.Regular;
            float fontSize = OxStyles.DefaultFontSize;

            style.SelectionBackColor = new OxColorHelper(style.BackColor).Darker(2);

            if (selected)
                fontStyle |= FontStyle.Bold;

            style.Font = OxStyles.Font(fontSize, fontStyle);
            style.SelectionForeColor = style.ForeColor;
            return style;
        }
    }
}
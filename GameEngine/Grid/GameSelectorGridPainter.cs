using OxLibrary;
using OxXMLEngine.Data.Types;
using OxXMLEngine.Grid;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Decorator;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Grid
{
    public class GameSelectorGridPainter : GridPainter<GameField, Game>
    {
        private readonly OxColorHelper colorHelper = new(Color.FromArgb(245, 250, 255));

        public GameSelectorGridPainter() : base(new GridFieldColumns<GameField>())
        { }

        public override DataGridViewCellStyle GetCellStyle(Game item, GameField field, bool selected = false)
        {
            DataGridViewCellStyle style = new()
            {
                BackColor = colorHelper.BaseColor,
                SelectionBackColor = colorHelper.Darker(2),
                ForeColor = colorHelper.Darkest
            };

            FontStyle fontStyle = FontStyle.Regular;
            float fontSize = Styles.DefaultFontSize;

            if (selected)
                fontStyle |= FontStyle.Bold;

            style.Font = new Font(Styles.FontFamily, fontSize, fontStyle);
            style.BackColor = TypeHelper.BackColor(item.SourceType);
            style.SelectionBackColor = new OxColorHelper(style.BackColor).Darker(2);
            style.ForeColor = TypeHelper.FontColor(new GameCalculations(item).GetGameStatus());

            style.SelectionForeColor = style.ForeColor;
            return style;
        }
    }
}
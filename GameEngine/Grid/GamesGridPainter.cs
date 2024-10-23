using OxLibrary;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Grid;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Grid
{
    public class GamesGridPainter : GridPainter<GameField, Game>
    {
        public GamesGridPainter(GridFieldColumns<GameField> columnsDictionary) : base(columnsDictionary) { }

        public override DataGridViewCellStyle GetCellStyle(Game? item, GameField field, bool selected = false)
        {
            DataGridViewCellStyle style = new()
            {
                BackColor = TypeHelper.BackColor(item?.SourceType),
                SelectionBackColor = TypeHelper.FontColor(item?.SourceType)
            };

            FontStyle fontStyle = FontStyle.Regular;
            float fontSize = Styles.DefaultFontSize;
            style.ForeColor = TypeHelper.FontColor(item?.SourceType);

            switch (field)
            {
                case GameField.Pegi:
                    fontStyle |= FontStyle.Bold;
                    style.ForeColor = new OxColorHelper(
                        TypeHelper.FontColor(item?.Pegi)
                    ).Darker(2);
                    fontSize -= 1;
                    break;
                case GameField.CriticScore:
                    fontStyle |= FontStyle.Bold;
                    style.BackColor = TypeHelper.BackColor(item?.CriticRange);

                    if (item?.CriticRange == CriticRange.Best)
                        style.ForeColor = Color.FromArgb(225,225,225);
                    break;
            }

            style.SelectionBackColor = new OxColorHelper(style.BackColor).Darker(2);

            if (selected)
                fontStyle |= FontStyle.Bold;

            style.Font = Styles.Font(fontSize, fontStyle);
            style.SelectionForeColor = style.ForeColor;
            return style;
        }
    }
}
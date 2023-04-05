using OxLibrary;
using OxXMLEngine;
using OxXMLEngine.Data.Fields;
using OxXMLEngine.Data.Types;
using OxXMLEngine.Grid;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Decorator;
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
                SelectionBackColor = Color.Transparent,
                ForeColor = TypeHelper.FontColor(item != null ? new GameCalculations(item).GetGameStatus() : null) 
            };

            FontStyle fontStyle = FontStyle.Regular;
            float fontSize = Styles.DefaultFontSize;

            switch (field)
            {
                case GameField.Progress:
                case GameField.FullPlatinum:
                case GameField.FullGold:
                case GameField.FullSilver:
                case GameField.FullBronze:
                case GameField.FullFromDLC:
                case GameField.FullNet:
                    if (item != null && item.TrophysetAccess == TrophysetAccess.NeverGet)
                        style.ForeColor = Color.Red;
                    break;
                case GameField.Pegi:
                    fontStyle |= FontStyle.Bold;
                    style.ForeColor = new OxColorHelper(
                        TypeHelper.FontColor(item?.Pegi)
                    ).Darker(2);
                    fontSize -= 1;
                    break;
                case GameField.CriticScore:
                    style.ForeColor = Color.Black;

                    if (item != null)
                        style.BackColor = CriticScoreBackColor(item.CriticScore);

                    break;
            }

            style.SelectionBackColor = new OxColorHelper(style.BackColor).Darker(2);

            if (selected)
                fontStyle |= FontStyle.Bold;

            style.Font = new Font(Styles.FontFamily, fontSize, fontStyle);
            style.SelectionForeColor = style.ForeColor;
            return style;
        }

        private static Color CriticScoreBackColor(int criticScore) => 
            criticScore > 74
                ? Color.FromArgb(200, 255, 200)
                : criticScore > 49
                    ? Color.FromArgb(255, 255, 200)
                    : criticScore > 0
                        ? Color.FromArgb(255, 200, 200)
                        : Color.Silver;
    }
}
using OxLibrary;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Grid;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data;

namespace PlayStationGames.AccountEngine.Grid
{
    public class AccountGridPainter : GridPainter<AccountField, Account>
    {
        public AccountGridPainter(GridFieldColumns<AccountField> columnsDictionary) : base(columnsDictionary)
        { }

        public override DataGridViewCellStyle GetCellStyle(Account? account, AccountField field, bool selected = false)
        {
            DataGridViewCellStyle style = new()
            {
                BackColor = Color.FromArgb(245, 251, 232),
                ForeColor = EngineStyles.DefaultGridFontColor
            };

            FontStyle fontStyle = FontStyle.Regular;
            float fontSize = Styles.DefaultFontSize;

            style.SelectionBackColor = new OxColorHelper(style.BackColor).Darker(2);

            if (selected)
                fontStyle |= FontStyle.Bold;

            style.Font = new Font(Styles.FontFamily, fontSize, fontStyle);
            style.SelectionForeColor = style.ForeColor;
            return style;
        }
    }
}
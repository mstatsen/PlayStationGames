using OxLibrary;
using OxDAOEngine;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Grid;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data;

namespace PlayStationGames.AccountEngine.Grid;

public class AccountGridPainter : GridPainter<AccountField, Account>
{
    public AccountGridPainter(GridFieldColumns<AccountField> columnsDictionary) : base(columnsDictionary)
    { }

    public override DataGridViewCellStyle GetCellStyle(Account? account, AccountField field, bool selected = false)
    {
        DataGridViewCellStyle style = new()
        {
            BackColor = TypeHelper.BackColor(account?.Type),
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
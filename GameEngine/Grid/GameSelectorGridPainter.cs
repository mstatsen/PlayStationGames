using OxLibrary;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Grid;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Grid;

public class GameSelectorGridPainter : GridPainter<GameField, Game>
{
    private readonly OxColorHelper colorHelper = new(Color.FromArgb(245, 250, 255));

    public GameSelectorGridPainter() : base(new GridFieldColumns<GameField>())
    { }

    public override DataGridViewCellStyle GetCellStyle(Game? item, GameField field, bool selected = false)
    {
        DataGridViewCellStyle style = new()
        {
            BackColor = colorHelper.BaseColor,
            SelectionBackColor = colorHelper.Darker(2),
            ForeColor = colorHelper.Darkest
        };

        FontStyle fontStyle = FontStyle.Regular;
        float fontSize = OxStyles.DefaultFontSize;

        if (selected)
            fontStyle |= FontStyle.Bold;

        style.Font = OxStyles.Font(fontSize, fontStyle);
        style.BackColor = TypeHelper.BackColor(item?.SourceType);
        style.SelectionBackColor = new OxColorHelper(style.BackColor).Darker(2);
        style.SelectionForeColor = TypeHelper.FontColor(item?.SourceType);
        return style;
    }
}
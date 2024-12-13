using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Forms;
using OxLibrary.Handlers;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Grid;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using OxLibrary.Geometry;

namespace PlayStationGames.GameEngine.ControlFactory.Controls;

public partial class RelatedGameEditor : CustomItemEditor<RelatedGame, GameField, Game>
{
    private OxLabel? GameLabel;
    private IControlAccessor? GameControl;
    private OxButton? GameSelectButton;
    private OxButton? SynchronizeButton;
    public override Bitmap FormIcon => OxIcons.Related;

    private Game? selectedGame;
    public Game? SelectedGame
    {
        get => selectedGame;
        set
        {
            selectedGame = value;
            SetGameControlValue();
            firstSet = selectedGame is null;
            SynchronizeButton!.SetEnabled(selectedGame is not null);

            if (firstSet)
            {
                SelectGame();

                if (selectedGame is null)
                    DialogResult = DialogResult.Cancel;
            }
        }
    }

    private bool firstSet = true; 

    protected override void CreateControls()
    {
        CreateGameControl();
        GameSelectButton = CreateButton(
            "Select...", 
            OxIcons.Select, 
            "Select game for relate",
            GameControl!.Left,
            SelectGameHandler);
        SynchronizeButton = CreateButton(
            "Synchronize", 
            OxIcons.Synchronize, 
            "Synchronize data between games",
            OxSh.Add(GameSelectButton.Right, 4),
            SynchronizeHandler);
        GameSelectButton.SizeChanged += SelecButtonSizeChangedHandler;
    }

    private void SelecButtonSizeChangedHandler(object sender, OxSizeChangedEventArgs args) => 
        SynchronizeButton!.Left = OxSh.Add(GameSelectButton!.Right, 4);

    private OxButton CreateButton(string text, Bitmap icon, string toolTipText, short left, EventHandler clickHandler)
    {
        OxButton button = new(text, icon)
        {
            Parent = FormPanel,
            Top = OxSh.Add(GameControl!.Bottom, 4),
            Left = left,
            Font = OxStyles.Font(FontStyle.Bold),
            ToolTipText = toolTipText,
            Size = new(140, 20)
        };
        button.Click += clickHandler;
        return button;
    }

    private void SynchronizeHandler(object? sender, EventArgs e) => 
        ItemsSynchronizer<GameField, Game>.SynchronizeItems(OwnerDAO!, SelectedGame!, FormPanel);

    private void CreateGameControl()
    {
        GameLabel = new OxLabel()
        {
            Parent = this,
            Left = 8,
            Text = "Game"
        };

        GameControl = Context.Accessor("RelatedGame:Name", FieldType.Memo, true);
        GameControl.Parent = this;
        GameControl.Top = 8;
        GameControl.Left = OxSh.Add(GameLabel.Right, 8);
        GameControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        GameControl.ReadOnly = OxB.T;
        GameControl.Width = OxSh.Sub(FormPanel.Width, GameControl.Left, 8);
        GameControl.Height = 56;
        GameControl.Control.BackColor = BackColor;
        ((OxTextBox)GameControl.ReadOnlyControl!).BorderStyle = BorderStyle.FixedSingle;
        OxControlHelper.AlignByBaseLine(GameControl.Control, GameLabel);
        SetKeyUpHandler(GameControl.Control);
        FirstFocusControl = GameControl.Control;
    }

    private void SelectGameHandler(object? sender, EventArgs e) =>
        SelectGame();

    private void SelectGame()
    {
        if (OwnerDAO is null)
            return;

        Game initialGame = new()
        {
            Name = OwnerDAO.OriginalName
        };

        if (DataManager.SelectItem(out Game? newSelected, FormPanel, initialGame, Filter))
            SelectedGame = newSelected;
    }

    private void SetGameControlValue()
    {
        GameControl!.Value = SelectedGame is not null
            ? SelectedGame.FullTitle()
            : string.Empty;
    }

    protected override short ContentWidth => 400;
    protected override short ContentHeight => OxSh.Add(SynchronizeButton!.Bottom, 2);

    protected override RelatedGame CreateNewItem() => new();

    protected override void FillControls(RelatedGame item) => 
        SelectedGame = DataManager.Item<GameField, Game>(GameField.Id, item.GameId);

    protected override void GrabControls(RelatedGame item)
    {
        if (SelectedGame is null)
        {
            OxMessage.ShowError("Select the Game for relate!", this);
            return;
        }

        item.GameId = SelectedGame.Id;
    }

    protected override string EmptyMandatoryField() =>
        GameControl!.IsEmpty
            ? "Releated game"
            : base.EmptyMandatoryField();
}
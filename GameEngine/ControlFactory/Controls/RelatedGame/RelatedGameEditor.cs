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

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
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
                SynchronizeButton!.Enabled = selectedGame is not null;

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
                OxWh.W(GameControl!.Left),
                SelectGameHandler);
            SynchronizeButton = CreateButton(
                "Synchronize", 
                OxIcons.Synchronize, 
                "Synchronize data between games",
                GameSelectButton.Right | OxWh.W4, 
                SynchronizeHandler);
            GameSelectButton.SizeChanged += SelecButtonSizeChangedHandler;
        }

        private void SelecButtonSizeChangedHandler(object sender, OxSizeChangedEventArgs args) => 
            SynchronizeButton!.Left = GameSelectButton!.Right | OxWh.W4;

        private OxButton CreateButton(string text, Bitmap icon, string toolTipText, OxWidth left, EventHandler clickHandler)
        {
            OxButton button = new(text, icon)
            {
                Parent = MainPanel,
                Top = OxWh.Add(GameControl!.Bottom, OxWh.W4),
                Left = left,
                Font = OxStyles.Font(FontStyle.Bold),
                ToolTipText = toolTipText,
                Size = new(OxWh.W140, OxWh.W20)
            };
            button.Click += clickHandler;
            return button;
        }

        private void SynchronizeHandler(object? sender, EventArgs e) => 
            ItemsSynchronizer<GameField, Game>.SynchronizeItems(OwnerDAO!, SelectedGame!, MainPanel);

        private void CreateGameControl()
        {
            GameLabel = new OxLabel()
            {
                Parent = this,
                Left = OxWh.W8,
                Text = "Game"
            };

            GameControl = Context.Accessor("RelatedGame:Name", FieldType.Memo, true);
            GameControl.Parent = this;
            GameControl.Top = 8;
            GameControl.Left = OxWh.Int(GameLabel.Right) + 8;
            GameControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            GameControl.ReadOnly = true;
            GameControl.Width =
                OxWh.Int(
                    OxWh.Sub(
                        OxWh.Sub(MainPanel.Width, GameControl.Left),
                        OxWh.W8)
                    );
            GameControl.Height = 56;
            GameControl.Control.BackColor = MainPanel.BackColor;
            ((OxTextBox)GameControl.ReadOnlyControl!).BorderStyle = BorderStyle.FixedSingle;
            OxControlHelper.AlignByBaseLine(GameControl.Control, GameLabel);
            SetKeyUpHandler(GameControl.Control);
            FirstFocusControl = GameControl.Control;
        }

        private void SelectGameHandler(object? sender, EventArgs e) => SelectGame();

        private void SelectGame()
        {
            if (OwnerDAO is null)
                return;

            Game initialGame = new()
            {
                Name = OwnerDAO.OriginalName
            };

            if (DataManager.SelectItem(out Game? newSelected, MainPanel, initialGame, Filter))
                SelectedGame = newSelected;
        }

        private void SetGameControlValue()
        {
            GameControl!.Value = SelectedGame is not null
                ? SelectedGame.FullTitle()
                : string.Empty;
        }

        protected override OxWidth ContentWidth => OxWh.W400;
        protected override OxWidth ContentHeight => SynchronizeButton!.Bottom | OxWh.W2;

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
}
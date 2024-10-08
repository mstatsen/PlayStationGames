using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Dialogs;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using OxDAOEngine.Grid;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class RelatedGameEditor : ListItemEditor<RelatedGame, GameField, Game>
    {
        private OxLabel? GameLabel;
        private IControlAccessor? GameControl;
        private OxButton? GameSelectButton;
        private OxButton? SynchronizeButton;

        private Game? selectedGame;
        public Game? SelectedGame
        {
            get => selectedGame;
            set
            {
                selectedGame = value;
                SetGameControlValue();
                firstSet = selectedGame == null;
                SynchronizeButton!.Enabled = selectedGame != null;

                if (firstSet)
                {
                    SelectGame();

                    if (selectedGame == null)
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
                GameSelectButton.Right + 4, 
                SynchronizeHandler);
            GameSelectButton.SizeChanged += SelecButtonSizeChangedHandler;
        }

        private void SelecButtonSizeChangedHandler(object? sender, EventArgs e) => 
            SynchronizeButton!.Left = GameSelectButton!.Right + 4;

        private OxButton CreateButton(string text, Bitmap icon, string toolTipText, int left, EventHandler clickHandler)
        {
            OxButton button = new(text, icon)
            {
                Parent = this,
                Top = GameControl!.Bottom + 4,
                Left = left,
                Font = new Font(Styles.FontFamily, Styles.DefaultFontSize, FontStyle.Bold),
                ToolTipText = toolTipText
            };
            button.SetContentSize(140, 20);
            button.Click += clickHandler;
            return button;
        }

        private void SynchronizeHandler(object? sender, EventArgs e)
        {
            ItemsSynchronizer<GameField, Game>.SynchronizeItems(ParentItem!, SelectedGame!, MainPanel);
        }

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
            GameControl.Left = GameLabel.Right + 8;
            GameControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            GameControl.ReadOnly = true;
            GameControl.Width = MainPanel.ContentContainer.Width - GameControl.Left - 8;
            GameControl.Height = 56;
            GameControl.Control.BackColor = MainPanel.BackColor;
            ((OxTextBox)GameControl.ReadOnlyControl!).BorderStyle = BorderStyle.FixedSingle;
            OxControlHelper.AlignByBaseLine(GameControl.Control, GameLabel);
        }

        private void SelectGameHandler(object? sender, EventArgs e) => SelectGame();

        private void SelectGame()
        {
            if (ParentItem == null)
                return;

            Game initialGame = new()
            {
                Name = ParentItem.OriginalName
            };

            if (DataManager.SelectItem(out Game? newSelected, MainPanel, initialGame, Filter))
                SelectedGame = newSelected;
        }

        private void SetGameControlValue()
        {
            GameControl!.Value = SelectedGame != null
                ? SelectedGame.FullTitle()
                : string.Empty;
        }

        protected override int ContentWidth => 400;
        protected override int ContentHeight => SynchronizeButton!.Bottom + 2;

        protected override RelatedGame CreateNewItem() => new();

        protected override void FillControls(RelatedGame item) => 
            SelectedGame = DataManager.Item<GameField, Game>(GameField.Id, item.GameId);

        protected override void GrabControls(RelatedGame item)
        {
            if (SelectedGame == null)
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
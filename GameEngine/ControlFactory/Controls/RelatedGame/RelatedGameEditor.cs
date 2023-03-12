using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Dialogs;
using OxXMLEngine.ControlFactory.Accessors;
using OxXMLEngine.ControlFactory.Controls;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Fields;
using OxXMLEngine.Grid;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class RelatedGameEditor : ListItemEditor<RelatedGame, GameField, Game>
    {
        private OxLabel? GameLabel;
        private IControlAccessor? GameControl;
        private IControlAccessor? SameTrophysetControl;
        private OxButton? GameSelectButton;

        private Game? selectedGame;
        public Game? SelectedGame 
        { 
            get => selectedGame;
            set
            {
                selectedGame = value;
                SetGameControlValue();

                if (selectedGame == null)
                {
                    SelectGame();

                    if (selectedGame == null)
                        DialogResult = DialogResult.Cancel;
                }
            }
        }
        protected override string Title => "Related Game";


        protected override void CreateControls()
        {
            CreateGameControl();
            CreateGameSelectButton();
            CreateSameTrophysetControl();
        }

        private void CreateGameControl()
        {
            GameLabel = new OxLabel()
            {
                Parent = this,
                Left = 8,
                Text = "Game"
            };

            GameControl = Context.Builder.Accessor("RelatedGameNae", FieldType.Memo, true);
            GameControl.Parent = this;
            GameControl.Top = 8;
            GameControl.Left = GameLabel.Right + 8;
            GameControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            GameControl.ReadOnly = true;
            GameControl.Width = MainPanel.ContentContainer.Width - GameControl.Left - 116;
            GameControl.Height *= 3;
            GameControl.Control.BackColor = MainPanel.BackColor;
            OxControlHelper.AlignByBaseLine(GameControl.Control, GameLabel);
        }

        private void CreateSameTrophysetControl()
        {
            SameTrophysetControl = Context.Builder.Accessor("RelatedGameSameTrophyset", FieldType.Boolean);
            SameTrophysetControl.Parent = this;
            SameTrophysetControl.Top = GameControl!.Bottom + 8;
            SameTrophysetControl.Left = GameControl.Left;
            SameTrophysetControl.Width = 124;
            SameTrophysetControl.Text = "Same trophyset";
        }

        private void CreateGameSelectButton()
        {
            GameSelectButton = new OxButton("Select...", null)
            {
                Parent = this,
                Top = 8,
                Left = GameControl!.Right + 8,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Font = new Font(Styles.FontFamily, Styles.DefaultFontSize, FontStyle.Bold)
            };
            GameSelectButton.SetContentSize(100, 28);
            GameSelectButton.Click += GameSelectButtonClickHandler;
        }

        private void SelectGame()
        {
            if (DataManager.SelectItem<GameField, Game>(out Game? newSelected, this, SelectedGame, Filter))
                SelectedGame = newSelected;
        }

        private void GameSelectButtonClickHandler(object? sender, EventArgs e) =>
            SelectGame();


        private void SetGameControlValue()
        {
            GameControl!.Value = SelectedGame != null
                ? SelectedGame.FullTitle()
                : string.Empty;
        }

        protected override int ContentWidth => 480;
        protected override int ContentHeight => SameTrophysetControl!.Bottom + 12;

        protected override RelatedGame CreateNewItem() => new();

        protected override void FillControls(RelatedGame item)
        {
            SelectedGame = DataManager.Item<GameField, Game>(GameField.Id, item.GameId);
            SameTrophysetControl!.Value = item.SameTrophyset;
        }

        protected override void GrabControls(RelatedGame item)
        {
            if (SelectedGame == null)
            {
                OxMessage.ShowError("Select the Game for relate!");
                return;
            }

            item.GameId = SelectedGame.Id;
            item.SameTrophyset = SameTrophysetControl!.BoolValue;
        }

        protected override string EmptyMandatoryField() =>
            GameControl!.IsEmpty
                ? "Releated game"
                : base.EmptyMandatoryField();
    }
}
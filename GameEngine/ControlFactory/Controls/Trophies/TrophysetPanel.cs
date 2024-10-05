using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.Data;
using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Panels;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls.Trophies
{
    public class TrophysetPanel : OxPane
    {
        private readonly ControlBuilder<GameField, Game> builder = DataManager.Builder<GameField, Game>(ControlScope.Editor);
        private readonly EnumAccessor<GameField, Game, TrophysetType> trophysetTypeControl;
        private readonly EnumAccessor<GameField, Game, Difficult> difficultControl;
        private readonly EnumAccessor<GameField, Game, CompleteTime> completeTimeControl;
        private readonly List<TrophiesPanel> trophiesPanels = new();
        private readonly TrophiesPanel AvailableTrophiesPanel;
        private readonly OxLabel trophysetTypeLabel = new()
        { 
            Left = 8,
            Text = "Type"
        };
        private readonly OxLabel difficultLabel = new()
        {
            Left = 8,
            Text = "Difficult"
        };
        private readonly OxLabel completeTimeLabel = new()
        { 
            Left = 8, 
            Text = "Complete time" 
        };

        public TrophysetPanel()
        {
            trophysetTypeControl = (EnumAccessor<GameField, Game, TrophysetType>)builder
                .Accessor(GameField.TrophysetType);
            difficultControl = (EnumAccessor<GameField, Game, Difficult>)builder
                .Accessor(GameField.Difficult);
            completeTimeControl = (EnumAccessor<GameField, Game, CompleteTime>)builder
                .Accessor(GameField.CompleteTime);
            trophysetTypeControl.Parent = this;
            trophysetTypeControl.Left = 112;
            trophysetTypeControl.Top = 8;
            trophysetTypeControl.Width = 152;
            trophysetTypeControl.ValueChangeHandler += TypeChangeHandler;
            trophysetTypeLabel.Parent = this;
            OxControlHelper.AlignByBaseLine(trophysetTypeControl.Control, trophysetTypeLabel);
            difficultControl.Parent = this;
            difficultControl.Left = 112;
            difficultControl.Top = trophysetTypeControl.Bottom + 4;
            difficultControl.Width = 64;
            difficultControl.ValueChangeHandler += DifficultChangeHandler;
            difficultLabel.Parent = this;
            OxControlHelper.AlignByBaseLine(difficultControl.Control, difficultLabel);
            completeTimeControl.Parent = this;
            completeTimeControl.Left = 112;
            completeTimeControl.Top = difficultControl.Bottom + 4;
            completeTimeControl.Width = 100;
            completeTimeControl.ValueChangeHandler += CompleteTimeChangeHandler;
            completeTimeLabel.Parent = this;
            OxControlHelper.AlignByBaseLine(completeTimeControl.Control, completeTimeLabel);

            AvailableTrophiesPanel = CreateTrophiesPanel(null);

            foreach (Account account in DataManager.FullItemsList<AccountField, Account>())
                CreateTrophiesPanel(account);

            SetMinimumSize();
        }

        private void CompleteTimeChangeHandler(object? sender, EventArgs e) =>
            ValueChanged?.Invoke(this, e);

        private void DifficultChangeHandler(object? sender, EventArgs e) =>
            ValueChanged?.Invoke(this, e);

        private void TypeChangeHandler(object? sender, EventArgs e)
        {
            if (Type == TrophysetType.NoSet)
                ClearValues();

            difficultControl.Visible = Type != TrophysetType.NoSet;
            completeTimeControl.Visible = Type != TrophysetType.NoSet;
            difficultLabel.Visible = Type != TrophysetType.NoSet;
            completeTimeLabel.Visible = Type != TrophysetType.NoSet;

            foreach (TrophiesPanel trophiesPanel in trophiesPanels)
                trophiesPanel.Visible = Type != TrophysetType.NoSet;

            SetMinimumSize();
            ValueChanged?.Invoke(this, e);
        }

        private void SetMinimumSize()
        {
            int height = 0;

            foreach (Control control in Controls)
                if (control.Visible)
                    height = Math.Max(control.Bottom, height);

            MinimumSize = new Size(
                trophiesPanels.Last().Right + 16,
                height + 6
            );
            MaximumSize = MinimumSize;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            if (trophysetTypeControl != null)
            {
                ControlPainter.ColorizeControl(trophysetTypeControl, Colors.Darker());
                ControlPainter.ColorizeControl(difficultControl, Colors.Darker());
                ControlPainter.ColorizeControl(completeTimeControl, Colors.Darker());
             
                foreach (TrophiesPanel panel in trophiesPanels)
                    panel.BaseColor = BaseColor;
            }
        }

        private TrophiesPanel CreateTrophiesPanel(Account? account)
        {
            TrophiesPanel result = new(account)
            {
                Parent = this,
                Left = 8,
                Top = (trophiesPanels.Count > 0 ? trophiesPanels.Last().Bottom + 4 : 100) + 8
            };

            trophiesPanels.Add(result);

            if (AvailableTrophiesPanel != null &&
                account != null)
                AvailableTrophiesPanel.DependedPanels.Add(result);

            return result;
        }

        public Trophyset? Value
        {
            get
            {
                Trophyset result = new()
                {
                    Type = trophysetTypeControl.EnumValue<TrophysetType>(),
                    Difficult = difficultControl.EnumValue<Difficult>(),
                    CompleteTime = completeTimeControl.EnumValue<CompleteTime>(),
                    Available = AvailableTrophiesPanel.Value
                };

                foreach (TrophiesPanel trophiesPanel in AvailableTrophiesPanel.DependedPanels)
                {
                    EarnedTrophies earnedTrophies = new()
                    {
                        AccountId = trophiesPanel.Account!.Id
                    };
                    earnedTrophies.Trophies.CopyFrom(trophiesPanel.Value);
                    result.EarnedTrophies.Add(earnedTrophies);
                }
                
                return result;
            }

            set
            {
                if (value == null)
                {
                    ClearValues();
                    return;
                }

                trophysetTypeControl.Value = value.Type;
                difficultControl.Value = value.Difficult;
                completeTimeControl.Value = value.CompleteTime;
                AvailableTrophiesPanel.Value = value.Available;

                foreach (TrophiesPanel trophiesPanel in AvailableTrophiesPanel.DependedPanels)
                    trophiesPanel.Value = value.EarnedTrophies.GetTrophies(trophiesPanel.Account!.Id).Trophies;
            }
        }

        public bool ReadOnly
        {
            get => trophysetTypeControl.ReadOnly;
            set
            {
                trophysetTypeControl.ReadOnly = value;
                difficultControl.ReadOnly = value;
                completeTimeControl.ReadOnly = value;
                AvailableTrophiesPanel!.ReadOnly = value;
            }
        }
        public TrophysetType Type 
        { 
            get => trophysetTypeControl.EnumValue<TrophysetType>();
            set => trophysetTypeControl.Value = value;
        }
        public EventHandler? ValueChanged { get; set; }

        public void ClearValues()
        {
            trophysetTypeControl.Value = TrophysetType.NoSet;
            difficultControl.Value = Difficult.Unknown;
            completeTimeControl.Value = CompleteTime.Unknown;

            foreach (TrophiesPanel trophiesPanel in trophiesPanels)
                trophiesPanel.ClearValue();
        }
    }
}

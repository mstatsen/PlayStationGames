using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Types;
using OxLibrary.Controls;
using OxLibrary.Panels;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls.Trophies
{
    public class TrophiesPanel : OxFrameWithHeader
    {
        private readonly Dictionary<TrophyType, ControlAccessor<GameField, Game>> controls = new();
        private readonly List<OxPicture> icons = new();

        public readonly Account? Account;

        public TrophiesPanel(Account? account, bool forDLC)
        {
            Account = account;
            CreateControls(forDLC);
        }

        public List<TrophiesPanel> DependedPanels { get; internal set; } = new();

        private void ApplyConstraints(TrophyList? constraints)
        {
            foreach (var item in controls)
            {
                int maxValue = constraints == null ? 200 : constraints.GetTrophyCount(item.Key);
                item.Value.MaximumValue = maxValue;
                item.Value.Enabled = maxValue > 0;
            }
        }

        private bool readOnly = false;
        public bool ReadOnly
        { 
            get => readOnly;
            set => SetReadOnly(value);
        }

        private void SetReadOnly(bool value)
        {
            readOnly = value;

            foreach (IControlAccessor accessor in controls.Values)
                accessor.ReadOnly = readOnly;
        }

        protected override void PrepareColors()
        {
            base.PrepareColors();

            foreach (IControlAccessor accessor in controls.Values)
                ControlPainter.ColorizeControl(
                    accessor.Control,
                    Colors.Darker());

            foreach (OxPicture icon in icons)
                ControlPainter.ColorizeControl(
                    icon,
                    BaseColor);
        }

        private void CreateIcon(TrophyType type, int left)
        {
            OxPicture icon = new()
            {
                Parent = this,
                Image = trophyTypeHelper.Icon(type),
                Left = left,
                Top = 6
            };
            icon.SetContentSize(24, 24);
            icons.Add(icon);
        }

        private void CreateControls(bool forDLC)
        {
            Text = Account == null ? "Available trophies": Account.Name;
            ControlBuilder<GameField, Game> builder = DataManager.Builder<GameField, Game>(ControlScope.Editor);
            CreateIcon(TrophyType.Platinum, 8);
            string forDLCPrefix = forDLC ? "DLC:" : string.Empty;
            CheckBoxAccessor<GameField, Game> platinumControl = new(
                builder.Context($"{forDLCPrefix}{Text}_platinum", FieldType.Boolean, null)
            )
            { 
                Parent = this,
                Left = icons[0].Right,
                Top = 6,
                Width = 16
            };
            platinumControl.ValueChangeHandler += ApplyConstraintsToDependedPanelsHandler;
            controls.Add(TrophyType.Platinum, platinumControl);
            int calcedLeft = 64;

            foreach (TrophyType type in trophyTypeHelper.CountingTrophies)
            {
                CreateIcon(type, calcedLeft);
                calcedLeft += 24;
                NumericAccessor<GameField, Game> accessor = new(builder.Context($"{forDLCPrefix}{Text}_{type}", FieldType.Integer, null))
                {
                    Parent = this,
                    Left = calcedLeft,
                    Top = 6,
                    Width = 30,
                    Height = 20,
                    ShowStepButtons = false
                };
                accessor.ValueChangeHandler += ApplyConstraintsToDependedPanelsHandler;
                calcedLeft += 40;
                controls.Add(type, accessor);
            }
            SetContentSize(256, 32);
        }

        private void ApplyConstraintsToDependedPanels()
        {
            foreach (TrophiesPanel panel in DependedPanels)
                panel.ApplyConstraints(Value);
        }

        private void ApplyConstraintsToDependedPanelsHandler(object? sender, EventArgs e) => 
            ApplyConstraintsToDependedPanels();

        public TrophyList? Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        private readonly TrophyTypeHelper trophyTypeHelper = TypeHelper.Helper<TrophyTypeHelper>();

        private void SetValue(TrophyList? value)
        {
            controls[TrophyType.Platinum].Value = value != null && value.Platinum > 0;

            foreach(TrophyType type in trophyTypeHelper.CountingTrophies)
                controls[type].Value = value == null ? 0 : value.GetTrophyCount(type);

            ApplyConstraintsToDependedPanels();
        }

        private TrophyList GetValue()
        {
            TrophyList value = new()
            {
                { TrophyType.Platinum, controls[TrophyType.Platinum].BoolValue ? 1 : 0 }
            };

            foreach (TrophyType type in trophyTypeHelper.CountingTrophies)
                value.Add(type, controls[type].IntValue);

            return value;
        }

        internal void ClearValue()
        {
            controls[TrophyType.Platinum].Value = false;

            foreach (TrophyType type in trophyTypeHelper.CountingTrophies)
                controls[type].MaximumValue = 0;
        }
    }
}

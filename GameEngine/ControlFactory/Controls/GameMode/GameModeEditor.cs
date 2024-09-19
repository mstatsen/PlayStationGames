using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.ControlFactory.Initializers;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class GameModeEditor : ListItemEditor<GameMode, GameField, Game>
    {
        private IControlAccessor PlayModeControl = default!;

        public override void RenewData()
        {
            base.RenewData();

            if (ExistingItems != null)
                playModeInitializer.ExistingModes = new ListDAO<GameMode>(ExistingItems);

            PlayModeControl.RenewControl(true);
        }

        public GameModeEditor() => 
            InitializeComponent();

        private readonly PlayModeInitializer playModeInitializer = new();
        private void CreatePlayModeControl()
        {
            PlayModeControl = Context.Builder.EnumAccessor<PlayMode>();
            PlayModeControl.Context.SetInitializer(playModeInitializer);
            PlayModeControl.Parent = this;
            PlayModeControl.Left = 12;
            PlayModeControl.Top = 12;
            PlayModeControl.Width = MainPanel.ContentContainer.Width - 24;
            PlayModeControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
        }

        protected override void CreateControls() =>
            CreatePlayModeControl();
            
        protected override void FillControls(GameMode item) =>
            PlayModeControl.Value = item.PlayMode;

        protected override void GrabControls(GameMode item) =>
            item.PlayMode = TypeHelper.Value<PlayMode>(PlayModeControl.Value);

        protected override int ContentWidth => 240;
        protected override int ContentHeight => PlayModeControl.Bottom + 12;

        protected override string EmptyMandatoryField() =>
            PlayModeControl.IsEmpty 
                ? "Play mode" 
                : base.EmptyMandatoryField();
    }
}
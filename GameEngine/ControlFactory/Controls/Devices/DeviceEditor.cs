using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class DeviceEditor : ListItemEditor<Device, GameField, Game>
    {
        private EnumAccessor<GameField, Game, DeviceType> TypeControl = default!;

        public override void RenewData()
        {
            base.RenewData();

            if (ExistingItems != null)
                TypeControl.Context.InitControl(TypeControl);
        }

        private void CreateTypeControl()
        {
            TypeControl = (EnumAccessor<GameField, Game, DeviceType>)Context.Builder
                .Accessor("Device:Type", FieldType.Enum, true);
            TypeControl.Parent = this;
            TypeControl.Left = 8;
            TypeControl.Top = 8;
            TypeControl.Width = MainPanel.ContentContainer.Width - TypeControl.Left - 8;
            TypeControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            TypeControl.Height = 24;
        }

        protected override void CreateControls() =>
            CreateTypeControl();

        protected override int ContentHeight => TypeControl.Bottom + 8;

        protected override void FillControls(Device item) => 
            TypeControl.Value = item.Type;

        protected override void GrabControls(Device item) =>
            item.Type = TypeControl.EnumValue;

        protected override string EmptyMandatoryField() =>
            TypeControl.IsEmpty
                ? "Type"
                : base.EmptyMandatoryField();
    }
}
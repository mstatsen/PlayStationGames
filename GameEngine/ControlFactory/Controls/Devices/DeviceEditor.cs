using OxLibrary;
using OxLibrary.Geometry;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data.Fields;
using PlayStationGames.GameEngine.ControlFactory.Controls.Initializers;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls;

public partial class DeviceEditor : CustomItemEditor<Device, GameField, Game>
{
    private EnumAccessor<GameField, Game, DeviceType> TypeControl = default!;

    public override Bitmap FormIcon => OxIcons.Dualshock;

    public override void RenewData()
    {
        base.RenewData();

        if (TypeControl.Context.Initializer is DeviceTypeInitializer deviceTypeInitializer)
        {
            deviceTypeInitializer.Game = OwnerDAO;
            deviceTypeInitializer.ExistingTypes.Clear();

            if (ExistingItems is not null)
                deviceTypeInitializer.ExistingTypes.AddRange(ExistingItems.Cast<Device>());

            TypeControl.RenewControl(true);
        }
    }

    private void CreateTypeControl()
    {
        TypeControl = (EnumAccessor<GameField, Game, DeviceType>)Context.Builder
            .Accessor("Device:Type", FieldType.Enum);
        TypeControl.Parent = this;
        TypeControl.Left = 8;
        TypeControl.Top = 8;
        TypeControl.Width = OxSH.Sub(FormPanel.Width, TypeControl.Left + 8);
        TypeControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
        TypeControl.Height = 24;
        SetKeyUpHandler(TypeControl.Control);
        FirstFocusControl = TypeControl.Control;
    }

    protected override void CreateControls() =>
        CreateTypeControl();

    protected override short ContentHeight => (short)(TypeControl.Bottom + 8);

    protected override void FillControls(Device item) => 
        TypeControl.Value = item.Type;

    protected override void GrabControls(Device item) =>
        item.Type = TypeControl.EnumValue;

    protected override string EmptyMandatoryField() =>
        TypeControl.IsEmpty
            ? "Type"
            : base.EmptyMandatoryField();
}
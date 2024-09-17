using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Types;
using OxLibrary.Controls;
using OxLibrary;
using PlayStationGames.ConsoleEngine.ControlFactory.Initializers;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public partial class AccessoryEditor : ListItemEditor<Accessory, ConsoleField, PSConsole>
    {
        private int PrepareControl(IControlAccessor accessor, 
            string caption, int lastBottom = -1, bool fullRow = true)
        {
            accessor.Parent = this;
            accessor.Left = 100;
            accessor.Top = lastBottom == -1 ? 8 : lastBottom + 4;
            accessor.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            if (fullRow)
            {
                accessor.Anchor |= AnchorStyles.Right;
                accessor.Width = MainPanel.ContentContainer.Width - accessor.Left - 8;
            }
            else
                accessor.Width = 64;

            accessor.Height = 24;
            accessor.Control.Tag = CreateLabel(caption, accessor);
            return accessor.Bottom;
        }

        protected override void CreateControls()
        {
            typeControl = (EnumAccessor<ConsoleField, PSConsole, AccessoryType>)Context.Builder.Accessor("AccessoryType", FieldType.Enum, ParentItem);
            joystickTypeControl = (EnumAccessor<ConsoleField, PSConsole, JoystickType>)Context.Builder.Accessor("JoystickType", FieldType.Enum, ParentItem);
            //TODO: replace with builder.Accessor
            colorControl = new ColorComboBoxAccessor<ConsoleField, PSConsole>(Context.Builder.Context(ConsoleField.Accessories));
            countControl = new NumericAccessor<ConsoleField, PSConsole>(Context.Builder.Context(ConsoleField.Accessories));
            descriptionControl = Context.Builder.Accessor("Accessory_Description", FieldType.Memo, true);

            int lastBottom = PrepareControl(typeControl, "Type");
            lastBottom = PrepareControl(joystickTypeControl, "Joystick Type", lastBottom);
            lastBottom = PrepareControl(colorControl, "Color", lastBottom);
            lastBottom = PrepareControl(countControl, "Count", lastBottom, false);
            PrepareControl(descriptionControl, "Description", lastBottom);
            descriptionControl.Height = 100;

            typeControl.ValueChangeHandler += TypeChangeHandler;
            joystickTypeControl.ValueChangeHandler += JoystickTypeChangeHandler;
        }

        private void TypeChangeHandler(object? sender, EventArgs e)
        {
            SetControlsVisible();
            RenewJoystickType();
        }

        private void RenewAccessoryType()
        {
            ((AccessoryTypeInitializer)typeControl!.Context.Initializer!).Console = ParentItem!;
            typeControl!.RenewControl(true);
        }

        private void RenewJoystickType()
        {
            if (IsJoystick())
            {
                ((JoystickTypeInitializer)joystickTypeControl!.Context.Initializer!).Console = ParentItem!;
                joystickTypeControl!.RenewControl(true);
            }
        }

        private void JoystickTypeChangeHandler(object? sender, EventArgs e) =>
            SetControlsVisible();

        private int SetControlTop(IControlAccessor? accessor, int lastBottom)
        {
            if (accessor == null)
                return lastBottom;

            accessor.Top = lastBottom + 4;
            OxControlHelper.AlignByBaseLine(accessor.Control, (OxLabel)accessor.Control.Tag);
            return accessor.Bottom;
        }

        private bool IsJoystick() => 
            typeControl?.EnumValue == AccessoryType.Joystick;

        private bool IsColored() => IsJoystick()
            && joystickTypeControl != null
            && TypeHelper.Helper<JoystickTypeHelper>().IsColored(joystickTypeControl.EnumValue);

        private void SetControlsVisible()
        {
            if (typeControl == null)
                return;

            if (joystickTypeControl == null) 
                return;

            joystickTypeControl.Visible = IsJoystick();
            ((OxLabel)joystickTypeControl.Control.Tag).Visible = joystickTypeControl.Visible;

            if (colorControl != null)
            {
                colorControl.Visible = IsColored();
                ((OxLabel)colorControl.Control.Tag).Visible = colorControl.Visible;
            }

            RecalcHeight();
        }

        private void RecalcHeight()
        {
            if (typeControl == null || 
                descriptionControl == null)
                return;

            int lastBottom = typeControl.Bottom;

            if (IsJoystick())
                lastBottom = SetControlTop(joystickTypeControl, lastBottom);

            if (IsColored())
                lastBottom = SetControlTop(colorControl, lastBottom);

            lastBottom = SetControlTop(countControl, lastBottom);
            lastBottom = SetControlTop(descriptionControl, lastBottom);
            SetContentSize(ContentWidth, lastBottom + 8);
        }

        protected override void FillControls(Accessory item)
        {
            typeControl!.Value = item.Type;
            RenewAccessoryType();
            joystickTypeControl!.Value = item.JoystickType;
            RenewJoystickType();
            colorControl!.Value = item.Color;
            countControl!.Value = item.Count;
            descriptionControl!.Value = item.Description;
            SetControlsVisible();
        }

        protected override void GrabControls(Accessory item)
        {
            item.Type = typeControl!.EnumValue;
            item.JoystickType = joystickTypeControl!.EnumValue;
            item.Color = colorControl!.StringValue;
            item.Count = countControl!.IntValue;
            item.Description = descriptionControl!.StringValue;
        }

        protected override string Title => "Accessory";

        protected override string EmptyMandatoryField() => 
            typeControl!.IsEmpty 
                ? "Type"
                : joystickTypeControl!.IsEmpty
                    ? "Joystick Type"
                    : base.EmptyMandatoryField();

        protected override int ContentHeight => descriptionControl!.Bottom + 8;

        private EnumAccessor<ConsoleField, PSConsole, AccessoryType>? typeControl;
        private EnumAccessor<ConsoleField, PSConsole, JoystickType>? joystickTypeControl;
        private IControlAccessor? colorControl;
        private IControlAccessor? countControl;
        private IControlAccessor? descriptionControl;
    }
}
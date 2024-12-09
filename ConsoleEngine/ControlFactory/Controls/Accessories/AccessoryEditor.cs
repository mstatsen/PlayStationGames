using OxLibrary;
using OxLibrary.Controls;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Context;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Types;
using PlayStationGames.ConsoleEngine.ControlFactory.Initializers;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;


namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public partial class AccessoryEditor : CustomItemEditor<Accessory, ConsoleField, PSConsole>
    {
        public override Bitmap? FormIcon => OxIcons.Dualshock;

        private int PrepareControl(IControlAccessor accessor, 
            string caption = "", int lastBottom = -1, bool fullRow = false)
        {
            bool withoutLabel = caption.Equals(string.Empty);
            accessor.Parent = this;
            accessor.Left = withoutLabel ? 12 : 100;
            accessor.Top = lastBottom is -1 ? 8 : lastBottom + 4;
            accessor.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            if (fullRow)
            {
                accessor.Anchor |= AnchorStyles.Right;
                accessor.Width = FormPanel.Width - accessor.Left - 8;
            }
            else
                accessor.Width = 120;

            accessor.Height = 24;

            if (!caption.Equals(string.Empty))
                accessor.Control.Tag = CreateLabel(caption, accessor);

            return accessor.Bottom;
        }

        protected override void CreateControls()
        {
            typeControl = Context.Accessor("Accessory:Type", FieldType.Enum, OwnerDAO);
            joystickTypeControl = Context.Accessor("Joystick:Type", FieldType.Enum, OwnerDAO);

            IBuilderContext<ConsoleField, PSConsole> context = Context.Builder.Context(ConsoleField.Accessories);

            colorControl = Context.Accessor("Accessory:Color", FieldType.Color);
            withCoverControl = Context.Accessor("Accessory:WithCover", FieldType.Boolean);
            withCoverControl.Text = "With cover";
            ((OxCheckBox)withCoverControl.Control).CheckAlign = ContentAlignment.MiddleLeft;
            nameControl = Context.Accessor("Accessory:Name", FieldType.String);
            modelCodeControl = Context.Accessor("Accessory:ModelCode", FieldType.String); ;
            coverColorControl = Context.Accessor("Accessory:CoverColor", FieldType.Color);
            withStickCoversControl = Context.Accessor("Accessory:WithStickCovers", FieldType.Boolean);
            withStickCoversControl.Text = "With stick covers";
            ((OxCheckBox)withStickCoversControl.Control).CheckAlign = ContentAlignment.MiddleLeft;
            countControl = Context.Accessor("Accessory:Count", FieldType.Integer);
            descriptionControl = Context.Accessor("Accessory:Description", FieldType.Memo, true);

            int lastBottom = PrepareControl(typeControl, "Type", fullRow: true);
            lastBottom = PrepareControl(joystickTypeControl, "Joystick Type", lastBottom, true);
            PrepareControl(nameControl, "Name", lastBottom, true);
            PrepareControl(modelCodeControl, "Model code", lastBottom, true);
            lastBottom = PrepareControl(colorControl, "Color", lastBottom);
            lastBottom = PrepareControl(withCoverControl, lastBottom: lastBottom);
            lastBottom = PrepareControl(coverColorControl, "Cover color", lastBottom);
            coverColorControl.Left += 24;
            ((OxLabel)coverColorControl.Control.Tag).Left += 24;
            lastBottom = PrepareControl(withStickCoversControl, lastBottom: lastBottom, fullRow: true);
            lastBottom = PrepareControl(countControl, "Count", lastBottom);
            countControl.Width = 64;
            PrepareControl(descriptionControl, "Description", lastBottom, true);
            descriptionControl.Height = 100;

            typeControl.ValueChangeHandler += TypeChangeHandler;
            joystickTypeControl.ValueChangeHandler += JoystickTypeChangeHandler;
            withCoverControl.ValueChangeHandler += WithCoverChangeHandler;
        }

        private void WithCoverChangeHandler(object? sender, EventArgs e) => 
            SetControlsVisible();

        private void TypeChangeHandler(object? sender, EventArgs e)
        {
            SetControlsVisible();
            RenewJoystickType();
        }

        private void RenewAccessoryType()
        {
            ((AccessoryTypeInitializer)typeControl.Context.Initializer!).Console = OwnerDAO!;
            typeControl.RenewControl(true);
        }

        private void RenewJoystickType()
        {
            if (IsJoystick())
            {
                ((JoystickTypeInitializer)joystickTypeControl.Context.Initializer!).Console = OwnerDAO!;
                joystickTypeControl.RenewControl(true);
            }
        }

        private void JoystickTypeChangeHandler(object? sender, EventArgs e) =>
            SetControlsVisible();

        private static int SetControlTop(IControlAccessor? accessor, int lastBottom, bool useControl = true) =>
            useControl ? SetControlTop(accessor, lastBottom) : lastBottom;

        private static int SetControlTop(IControlAccessor? accessor, int lastBottom)
        {
            if (accessor is null)
                return lastBottom;

            accessor.Top = lastBottom + 4;
            OxControlHelper.AlignByBaseLine(accessor.Control, (OxLabel)accessor.Control.Tag);
            return accessor.Bottom;
        }

        private bool IsJoystick() => 
            AccessoryType is AccessoryType.Joystick;

        private readonly JoystickTypeHelper joystickTypeHelper = TypeHelper.Helper<JoystickTypeHelper>();
        private readonly AccessoryTypeHelper typeHelper = TypeHelper.Helper<AccessoryTypeHelper>();

        private bool IsColored() => IsJoystick()
            && joystickTypeHelper.IsColored(JoystickType);

        private AccessoryType AccessoryType => typeControl.EnumValue<AccessoryType>();
        private JoystickType JoystickType => joystickTypeControl.EnumValue<JoystickType>();

        private void SetControlsVisible()
        {
            if (typeControl is null 
                || joystickTypeControl is null)
                return;

            bool isJoystick = IsJoystick();
            joystickTypeControl.Visible = isJoystick;
            ((OxLabel)joystickTypeControl.Control.Tag).Visible = joystickTypeControl.Visible;
            bool namedControVisible = typeHelper.Named(AccessoryType, JoystickType);
            nameControl.Visible = namedControVisible;
            ((OxLabel)nameControl.Control.Tag).Visible = namedControVisible;
            bool modelCodeControVisible = typeHelper.SupportModelCode(AccessoryType, JoystickType);
            modelCodeControl.Visible = modelCodeControVisible;
            ((OxLabel)modelCodeControl.Control.Tag).Visible = modelCodeControVisible;
            colorControl.Visible = IsColored();
            ((OxLabel)colorControl.Control.Tag).Visible = colorControl.Visible;
            withCoverControl.Visible = isJoystick;
            coverColorControl.Visible = isJoystick && withCoverControl.BoolValue;
            ((OxLabel)coverColorControl.Control.Tag).Visible = isJoystick && withCoverControl.BoolValue;
            withStickCoversControl.Visible = isJoystick
                && joystickTypeHelper.WithSticks(JoystickType);

            RecalcHeight();
        }

        private void RecalcHeight()
        {
            if (typeControl is null
                || descriptionControl is null)
                return;

            bool named = typeHelper.Named(AccessoryType, JoystickType);
            bool modelCodeSupported = typeHelper.SupportModelCode(AccessoryType, JoystickType);
            int lastBottom = typeControl.Bottom;

            lastBottom = SetControlTop(joystickTypeControl, lastBottom, IsJoystick());
            lastBottom = SetControlTop(nameControl, lastBottom, named);
            lastBottom = SetControlTop(modelCodeControl, lastBottom, modelCodeSupported);

            if (IsJoystick())
            {
                lastBottom = SetControlTop(colorControl, lastBottom, IsColored());
                lastBottom = SetControlTop(withCoverControl, lastBottom);
                lastBottom = SetControlTop(coverColorControl, lastBottom, withCoverControl.BoolValue);
                lastBottom = SetControlTop(withStickCoversControl, lastBottom, joystickTypeHelper.WithSticks(JoystickType));
            }

            lastBottom = SetControlTop(countControl, lastBottom);
            lastBottom = SetControlTop(descriptionControl, lastBottom);
            Size = new OxSize(
                (short)ContentWidth,
                (short)(lastBottom + 8)
            );
        }

        protected override void FillControls(Accessory item)
        {
            typeControl.Value = item.Type;
            RenewAccessoryType();
            joystickTypeControl.Value = item.JoystickType;
            nameControl.Value = item.Name;
            modelCodeControl.Value = item.ModelCode;
            RenewJoystickType();
            colorControl.Value = item.Color;
            withCoverControl.Value = item.WithCover;
            coverColorControl.Value = item.CoverColor;
            withStickCoversControl.Value = item.WithStickCovers;
            countControl.Value = item.Count;
            descriptionControl.Value = item.Description;
            SetControlsVisible();
        }

        protected override void GrabControls(Accessory item)
        {
            item.Type = typeControl.EnumValue<AccessoryType>();
            item.Name = nameControl.StringValue;
            item.ModelCode = modelCodeControl.StringValue;
            item.JoystickType = JoystickType;
            item.Color = colorControl.StringValue;
            item.WithCover = withCoverControl.BoolValue;
            item.CoverColor = coverColorControl.StringValue;
            item.WithStickCovers = withStickCoversControl.BoolValue;
            item.Count = countControl.IntValue;
            item.Description = descriptionControl.StringValue;
        }

        protected override string EmptyMandatoryField() => 
            typeControl.IsEmpty 
                ? "Type"
                : joystickTypeControl.IsEmpty
                    ? "Joystick Type"
                    : base.EmptyMandatoryField();

        protected override short ContentHeight => (short)(descriptionControl.Bottom + 8);

        private IControlAccessor typeControl = default!;
        private IControlAccessor joystickTypeControl = default!;
        private IControlAccessor nameControl = default!;
        private IControlAccessor modelCodeControl = default!;
        private IControlAccessor colorControl = default!;
        private IControlAccessor countControl = default!;
        private IControlAccessor descriptionControl = default!;
        private IControlAccessor withCoverControl = default!;
        private IControlAccessor coverColorControl = default!;
        private IControlAccessor withStickCoversControl = default!;
    }
}
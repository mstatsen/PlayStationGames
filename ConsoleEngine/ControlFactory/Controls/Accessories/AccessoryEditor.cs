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
using OxDAOEngine.ControlFactory.Context;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public partial class AccessoryEditor : ListItemEditor<Accessory, ConsoleField, PSConsole>
    {
        private int PrepareControl(IControlAccessor accessor, 
            string caption = "", int lastBottom = -1, bool fullRow = false)
        {
            bool withoutLabel = caption == string.Empty;
            accessor.Parent = this;
            accessor.Left = withoutLabel ? 12 : 100;
            accessor.Top = lastBottom == -1 ? 8 : lastBottom + 4;
            accessor.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            if (fullRow)
            {
                accessor.Anchor |= AnchorStyles.Right;
                accessor.Width = MainPanel.ContentContainer.Width - accessor.Left - 8;
            }
            else
                accessor.Width = 120;

            accessor.Height = 24;

            if (caption != string.Empty)
                accessor.Control.Tag = CreateLabel(caption, accessor);

            return accessor.Bottom;
        }

        protected override void CreateControls()
        {
            typeControl = (EnumAccessor<ConsoleField, PSConsole, AccessoryType>)Context.Builder.Accessor("AccessoryType", FieldType.Enum, ParentItem);
            joystickTypeControl = (EnumAccessor<ConsoleField, PSConsole, JoystickType>)Context.Builder.Accessor("JoystickType", FieldType.Enum, ParentItem);

            IBuilderContext<ConsoleField, PSConsole> context = Context.Builder.Context(ConsoleField.Accessories);

            colorControl = new(context);
            withCoverControl = new(context)
            {
                Text = "With cover",
                CheckAlign = ContentAlignment.MiddleLeft
            };
            nameControl = new TextAccessor<ConsoleField, PSConsole>(context);
            modelCodeControl = new TextAccessor<ConsoleField, PSConsole>(context);
            coverColorControl = new(context);
            withStickCoversControl = new(context)
            {
                Text = "With stick covers",
                CheckAlign = ContentAlignment.MiddleLeft
            };
            countControl = new(context);
            descriptionControl = Context.Builder.Accessor("Accessory_Description", FieldType.Memo, true);

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
            ((AccessoryTypeInitializer)typeControl.Context.Initializer!).Console = ParentItem!;
            typeControl.RenewControl(true);
        }

        private void RenewJoystickType()
        {
            if (IsJoystick())
            {
                ((JoystickTypeInitializer)joystickTypeControl.Context.Initializer!).Console = ParentItem!;
                joystickTypeControl.RenewControl(true);
            }
        }

        private void JoystickTypeChangeHandler(object? sender, EventArgs e) =>
            SetControlsVisible();

        private int SetControlTop(IControlAccessor? accessor, int lastBottom, bool useControl = true) => 
            useControl ? SetControlTop(accessor, lastBottom) : lastBottom;

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

        private readonly JoystickTypeHelper joystickTypeHelper = TypeHelper.Helper<JoystickTypeHelper>();
        private readonly AccessoryTypeHelper typeHelper = TypeHelper.Helper<AccessoryTypeHelper>();

        private bool IsColored() => IsJoystick()
            && joystickTypeHelper.IsColored(joystickTypeControl.EnumValue);

        private void SetControlsVisible()
        {
            if (typeControl == null)
                return;

            if (joystickTypeControl == null) 
                return;

            bool isJoystick = IsJoystick();
            joystickTypeControl.Visible = isJoystick;
            ((OxLabel)joystickTypeControl.Control.Tag).Visible = joystickTypeControl.Visible;
            bool namedControVisible = typeHelper.Named(typeControl.EnumValue, joystickTypeControl.EnumValue);
            nameControl.Visible = namedControVisible;
            ((OxLabel)nameControl.Control.Tag).Visible = namedControVisible;
            bool modelCodeControVisible = typeHelper.SupportModelCode(typeControl.EnumValue, joystickTypeControl.EnumValue);
            modelCodeControl.Visible = modelCodeControVisible;
            ((OxLabel)modelCodeControl.Control.Tag).Visible = modelCodeControVisible;
            colorControl.Visible = IsColored();
            ((OxLabel)colorControl.Control.Tag).Visible = colorControl.Visible;
            withCoverControl.Visible = isJoystick;
            coverColorControl.Visible = isJoystick && withCoverControl.BoolValue;
            ((OxLabel)coverColorControl.Control.Tag).Visible = isJoystick && withCoverControl.BoolValue;
            withStickCoversControl.Visible = isJoystick
                && joystickTypeHelper.WithSticks(joystickTypeControl.EnumValue);

            RecalcHeight();
        }

        private void RecalcHeight()
        {
            if (typeControl == null || 
                descriptionControl == null)
                return;

            bool named = typeHelper.Named(typeControl.EnumValue, joystickTypeControl.EnumValue);
            bool modelCodeSupported = typeHelper.SupportModelCode(typeControl.EnumValue, joystickTypeControl.EnumValue);
            int lastBottom = typeControl.Bottom;

            lastBottom = SetControlTop(joystickTypeControl, lastBottom, IsJoystick());
            lastBottom = SetControlTop(nameControl, lastBottom, named);
            lastBottom = SetControlTop(modelCodeControl, lastBottom, modelCodeSupported);

            if (IsJoystick())
            {
                lastBottom = SetControlTop(colorControl, lastBottom, IsColored());
                lastBottom = SetControlTop(withCoverControl, lastBottom);
                lastBottom = SetControlTop(coverColorControl, lastBottom, withCoverControl.BoolValue);
                lastBottom = SetControlTop(withStickCoversControl, lastBottom, joystickTypeHelper.WithSticks(joystickTypeControl.EnumValue));
            }

            lastBottom = SetControlTop(countControl, lastBottom);
            lastBottom = SetControlTop(descriptionControl, lastBottom);
            SetContentSize(ContentWidth, lastBottom + 8);
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
            item.Type = typeControl.EnumValue;
            item.Name = nameControl.StringValue;
            item.ModelCode = modelCodeControl.StringValue;
            item.JoystickType = joystickTypeControl.EnumValue;
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

        protected override int ContentHeight => descriptionControl.Bottom + 8;

        private EnumAccessor<ConsoleField, PSConsole, AccessoryType> typeControl = default!;
        private EnumAccessor<ConsoleField, PSConsole, JoystickType> joystickTypeControl = default!;
        private IControlAccessor nameControl = default!;
        private IControlAccessor modelCodeControl = default!;
        private ColorComboBoxAccessor<ConsoleField, PSConsole> colorControl = default!;
        private NumericAccessor<ConsoleField, PSConsole> countControl = default!;
        private IControlAccessor descriptionControl = default!;
        private CheckBoxAccessor<ConsoleField, PSConsole> withCoverControl = default!;
        private ColorComboBoxAccessor<ConsoleField, PSConsole> coverColorControl = default!;
        private CheckBoxAccessor<ConsoleField, PSConsole> withStickCoversControl = default!;
    }
}
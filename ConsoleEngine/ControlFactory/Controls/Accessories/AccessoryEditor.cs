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
            nameControl = new(context);
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

            if (typeHelper.Named(typeControl.EnumValue, joystickTypeControl.EnumValue))
            {
                nameControl.Visible = true;
                ((OxLabel)nameControl.Control.Tag).Visible = true;
            }
            else
            {
                nameControl.Visible = false;
                ((OxLabel)nameControl.Control.Tag).Visible = false;
            }

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
        int lastBottom = typeControl.Bottom;

            if (IsJoystick())
            {
                lastBottom = SetControlTop(joystickTypeControl, lastBottom);

                if (named)
                    lastBottom = SetControlTop(nameControl, lastBottom);

                if (IsColored())
                    lastBottom = SetControlTop(colorControl, lastBottom);

                lastBottom = SetControlTop(withCoverControl, lastBottom);

                if (withCoverControl.BoolValue)
                    lastBottom = SetControlTop(coverColorControl, lastBottom);

                if (joystickTypeHelper.WithSticks(joystickTypeControl.EnumValue))
                    lastBottom = SetControlTop(withStickCoversControl, lastBottom);
            }
            else
                if (named)
                    lastBottom = SetControlTop(nameControl, lastBottom);

            lastBottom = SetControlTop(countControl, lastBottom);
            lastBottom = SetControlTop(descriptionControl, lastBottom);
            SetContentSize(ContentWidth, lastBottom + 8);
        }

        protected override void FillControls(Accessory item)
        {
            typeControl.Value = item.Type;
            RenewAccessoryType();
            nameControl.Value = item.Name;
            joystickTypeControl.Value = item.JoystickType;
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
        private TextAccessor<ConsoleField, PSConsole> nameControl = default!;
        private ColorComboBoxAccessor<ConsoleField, PSConsole> colorControl = default!;
        private NumericAccessor<ConsoleField, PSConsole> countControl = default!;
        private IControlAccessor descriptionControl = default!;
        private CheckBoxAccessor<ConsoleField, PSConsole> withCoverControl = default!;
        private ColorComboBoxAccessor<ConsoleField, PSConsole> coverColorControl = default!;
        private CheckBoxAccessor<ConsoleField, PSConsole> withStickCoversControl = default!;
    }
}
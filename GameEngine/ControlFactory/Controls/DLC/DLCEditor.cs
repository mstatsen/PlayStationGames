using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using OxLibrary.Panels;
using OxLibrary;
using OxLibrary.Controls;
using OxDAOEngine.ControlFactory;
using PlayStationGames.GameEngine.ControlFactory.Accessors;
using PlayStationGames.GameEngine.ControlFactory.Controls.Trophies;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class DLCEditor : ListItemEditor<DLC, GameField, Game>
    {
        private IControlAccessor NameControl = default!;
        private IControlAccessor AcquiredControl = default!;
        private IControlAccessor ImageControl = default!;
        private TrophysetAccessor TrophysetControl = default!;

        public override Bitmap FormIcon => OxIcons.Dlc;

        private readonly OxFrameWithHeader BaseGroup = new()
        {
            Text = "Information"
        };

        private readonly OxFrameWithHeader TrophysetGroup = new()
        {
            Text = "Trophyset"
        };

        protected override void CreateControls()
        {
            BaseGroup.Parent = this;
            BaseGroup.Margins.SetSize(OxSize.Extra);
            NameControl = Context.Accessor("DLC:Name", FieldType.ShortMemo, true);
            NameControl.Top = 8;
            NameControl.Left = 66;
            NameControl.Width = 208;
            NameControl.Height = 42;
            NameControl.Parent = BaseGroup;
            OxControlHelper.AlignByBaseLine(
                NameControl.Control,
                new OxLabel()
                {
                    Parent = BaseGroup,
                    Left = 8,
                    Text = "Name",
                    Font = new(Styles.DefaultFont, FontStyle.Bold)
                });
            FirstFocusControl = NameControl.Control;

            ImageControl = Context.Accessor("DLC:Image", FieldType.Image);
            ImageControl.Top = NameControl.Bottom + 8;
            ImageControl.Left = 66;
            ImageControl.Width = 208;
            ImageControl.Height = 112;
            ImageControl.Parent = BaseGroup;
            ((OxPictureContainer)ImageControl.Control).HiddenBorder = true;
            SetKeyUpHandler(ImageControl.Control);

            AcquiredControl = Context.Accessor("DLC:Acquired", FieldType.Boolean);
            AcquiredControl.Top = ImageControl.Bottom + 8;
            AcquiredControl.Left = 8;
            ((OxCheckBox)AcquiredControl.Control).AutoSize = true;
            AcquiredControl.Parent = BaseGroup;
            AcquiredControl.RenewControl();
            SetKeyUpHandler(AcquiredControl.Control);

            TrophysetControl = (TrophysetAccessor)Context.Accessor("DLC:Trophyset", FieldType.Custom);
            TrophysetControl.Parent = TrophysetGroup;
            TrophysetControl.ValueChangeHandler += TrophysetValueChange;
            TrophysetGroup.Parent = this;
            TrophysetGroup.Margins.SetSize(OxSize.Extra);
            TrophysetGroup.Margins.LeftOx = OxSize.None;
            RecalcSize();
        }

        private void TrophysetValueChange(object? sender, EventArgs e) =>
            RecalcSize();

        protected override void RecalcSize()
        {
            BaseGroup.SetContentSize(
                282,
                AcquiredControl.Bottom + 24
            );
            TrophysetGroup.Left = BaseGroup.Right;
            TrophysetGroup.SetContentSize(
                TrophysetControl.Width,
                TrophysetControl.Height + 12
            );
            SetContentSize(
                TrophysetGroup.Right,
                Math.Max(BaseGroup.Bottom, TrophysetGroup.Bottom)
            );
        }

        protected override void PrepareReadonly()
        {
            NameControl.ReadOnly = ReadOnly;
            AcquiredControl.ReadOnly = ReadOnly;
            ImageControl.ReadOnly = ReadOnly;
            TrophysetControl.ReadOnly = ReadOnly;
        }

        protected override void PrepareControlColors()
        {
            base.PrepareControlColors();
            BaseGroup.BaseColor = new OxColorHelper(BaseColor).Lighter();
            TrophysetGroup.BaseColor = BaseGroup.BaseColor;
            ControlPainter.ColorizeControl(
                NameControl,
                BaseColor
            );
            ((OxPictureContainer)ImageControl.Control).BaseColor = BaseGroup.BaseColor;
            ((TrophysetPanel)TrophysetControl.Control).BaseColor = TrophysetGroup.BaseColor;
        }

        protected override void FillControls(DLC item)
        {
            NameControl.Value = item.Name;
            ImageControl.Value = item.Image;
            AcquiredControl.Value = item.Acquired;
            TrophysetControl.Value = item.Trophyset;
        }

        protected override void GrabControls(DLC item)
        {
            item.Name = NameControl.StringValue;
            item.Image = (Bitmap?)ImageControl.Value;
            item.Acquired = AcquiredControl.BoolValue;
            item.Trophyset.CopyFrom(TrophysetControl.DAOValue<Trophyset>());
        }

        protected override string EmptyMandatoryField() =>
            NameControl.IsEmpty ? "DLC name" : base.EmptyMandatoryField();
    }
}
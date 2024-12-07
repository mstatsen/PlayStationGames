using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data.Fields;
using OxLibrary;
using OxLibrary.Forms;
using PlayStationGames.GameEngine.ControlFactory.Controls.Initializers;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class TagEditor : CustomItemEditor<Tag, GameField, Game>
    {
        private ComboBoxAccessor<GameField, Game> NameControl = default!;

        public override Bitmap FormIcon => OxIcons.Tag;

        public override void RenewData()
        {
            base.RenewData();

            if (NameControl.Context.Initializer is TagNameInitializer tagInitalizer)
            {
                tagInitalizer.ExistingTags.Clear();

                if (ExistingItems is not null)
                    tagInitalizer.ExistingTags.AddRange(ExistingItems.Cast<Tag>());

                NameControl.Context.InitControl(NameControl);
            }
        }

        private void CreateNameControl()
        {
            NameControl = (ComboBoxAccessor<GameField, Game>)Context.Builder
                .Accessor("Tag:Name", FieldType.Extract, true);
            NameControl.Parent = this;
            NameControl.Left = 8;
            NameControl.Top = 8;
            NameControl.Width =
                OxWh.Int(
                    OxWh.Sub(
                        OxWh.Sub(FormPanel.Width, NameControl.Left), 
                        OxWh.W8
                    )
                );
            NameControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            NameControl.Height = 24;
            SetKeyUpHandler(NameControl.Control);
            FirstFocusControl = NameControl.Control;
        }

        protected override void CreateControls() =>
            CreateNameControl();

        protected override OxWidth ContentHeight => OxWh.Add(NameControl.Bottom, OxWh.W8);

        protected override void FillControls(Tag item)
        {
            if (!item.Name.Equals(string.Empty) 
                && NameControl.ComboBox.Items.IndexOf(item.Name) < 0)
                NameControl.ComboBox.Items.Insert(0, item.Name);

            NameControl.Value = item.Name;
        }

        protected override void GrabControls(Tag item) =>
            item.Name = NameControl.StringValue;

        protected override string EmptyMandatoryField() =>
            NameControl.IsEmpty
                ? "Tag"
                : base.EmptyMandatoryField();

        public override bool CanOKClose()
        {
            if (ExistingItems is null)
                return true;

            if (NameControl.Value is not null 
                && ExistingItems.Find(t => ((Tag)t).Name.Equals(NameControl.Value.ToString())) is not null)
            {
                OxMessage.ShowError("Game already contains this tag.", this);
                return false;
            }

            return base.CanOKClose();
        }
    }
}
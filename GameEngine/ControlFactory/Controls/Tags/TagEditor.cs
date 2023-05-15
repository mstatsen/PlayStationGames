using OxXMLEngine.ControlFactory.Accessors;
using OxXMLEngine.ControlFactory.Controls;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class TagEditor : ListItemEditor<Tag, GameField, Game>
    {
        private ComboBoxAccessor<GameField, Game> NameControl = default!;

        public override void RenewData()
        {
            base.RenewData();

            if (ExistingItems != null)
                NameControl.Context.InitControl(NameControl);
        }

        protected override string Title => "Tag";

        private void CreateNameControl()
        {
            NameControl = (ComboBoxAccessor<GameField, Game>)Context.Builder
                .Accessor("TagName", FieldType.Extract, true);
            NameControl.Parent = this;
            NameControl.Left = 8;
            NameControl.Top = 8;
            NameControl.Width = MainPanel.ContentContainer.Width - NameControl.Left - 8;
            NameControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            NameControl.Height = 24;
        }

        protected override void CreateControls() =>
            CreateNameControl();

        protected override int ContentHeight => NameControl.Bottom + 8;

        protected override void FillControls(Tag item)
        {
            if (item.Name != string.Empty &&
                NameControl.ComboBox.Items.IndexOf(item.Name) < 0)
                NameControl.ComboBox.Items.Insert(0, item.Name);

            NameControl.Value = item.Name;
        }

        protected override void GrabControls(Tag item) =>
            item.Name = NameControl.StringValue;

        protected override string EmptyMandatoryField() =>
            NameControl.IsEmpty
                ? "Tag"
                : base.EmptyMandatoryField();
    }
}
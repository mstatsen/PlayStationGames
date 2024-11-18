using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data.Fields;
using OxLibrary;
using OxLibrary.Dialogs;
using PlayStationGames.GameEngine.ControlFactory.Controls.Initializers;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class SeriesEditor : CustomItemEditor<Series, GameField, Game>
    {
        private ComboBoxAccessor<GameField, Game> NameControl = default!;

        public override Bitmap FormIcon => OxIcons.Series;

        public override void RenewData()
        {
            base.RenewData();

            if (NameControl.Context.Initializer is SeriesNameInitializer seriesInitalizer)
            {
                seriesInitalizer.ExistingSeries.Clear();

                if (ExistingItems is not null)
                    seriesInitalizer.ExistingSeries.AddRange(ExistingItems.Cast<Series>());

                NameControl.Context.InitControl(NameControl);
            }
        }

        private void CreateNameControl()
        {
            NameControl = (ComboBoxAccessor<GameField, Game>)Context.Builder
                .Accessor("Series:Name", FieldType.Extract, true);
            NameControl.Parent = this;
            NameControl.Left = 8;
            NameControl.Top = 8;
            NameControl.Width = MainPanel.ContentContainer.Width - NameControl.Left - 8;
            NameControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            NameControl.Height = 24;
            NameControl.RenewControl();
            SetKeyUpHandler(NameControl.Control);
            FirstFocusControl = NameControl.Control;
            
        }

        protected override void CreateControls() =>
            CreateNameControl();

        protected override int ContentHeight => NameControl.Bottom + 8;

        protected override void FillControls(Series item)
        {
            if (item.Name != string.Empty &&
                NameControl.ComboBox.Items.IndexOf(item.Name) < 0)
                NameControl.ComboBox.Items.Insert(0, item.Name);

            NameControl.Value = item.Name;
        }

        protected override void GrabControls(Series item) =>
            item.Name = NameControl.StringValue;

        protected override string EmptyMandatoryField() =>
            NameControl.IsEmpty
                ? "Series"
                : base.EmptyMandatoryField();

        public override bool CanOKClose()
        {
            if (ExistingItems is null)
                return true;

            if (NameControl.Value is not null &&
                ExistingItems.Find(t => ((Series)t).Name.Equals(NameControl.Value.ToString())) is not null)
            {
                OxMessage.ShowError("Game already contains this series.", this);
                return false;
            }

            return base.CanOKClose();
        }
    }
}
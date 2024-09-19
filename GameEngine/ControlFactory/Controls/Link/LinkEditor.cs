﻿using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class LinkEditor : ListItemEditor<Link, GameField, Game>
    {
        private ComboBoxAccessor<GameField, Game> NameControl = default!;
        private IControlAccessor UrlControl = default!;

        public override void RenewData()
        {
            base.RenewData();

            if (ExistingItems != null)
                NameControl.Context.InitControl(NameControl);
        }

        private void CreateNameControl()
        {
            NameControl = (ComboBoxAccessor<GameField, Game>)Context.Builder
                .Accessor("LinkName", FieldType.Extract, true);
            NameControl.Parent = this;
            NameControl.Left = 60;
            NameControl.Top = 8;
            NameControl.Width = MainPanel.ContentContainer.Width - NameControl.Left - 8;
            NameControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            NameControl.Height = 24;
            CreateLabel("Name", NameControl);
        }

        private void CreateUrlControl()
        {
            UrlControl = Context.Builder.Accessor("LinkUrl", FieldType.Memo, true);
            UrlControl.Parent = this;
            UrlControl.Left = 60;
            UrlControl.Top = NameControl.Bottom + 8;
            UrlControl.Width = MainPanel.ContentContainer.Width - UrlControl.Left - 8;
            UrlControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            UrlControl.Height = 90;
            CreateLabel("URL", UrlControl);
        }

        protected override void CreateControls()
        {
            CreateNameControl();
            CreateUrlControl();
        }

        protected override int ContentHeight => UrlControl.Bottom + 8;

        protected override void FillControls(Link item)
        {
            if (item.Name != string.Empty &&
                NameControl.ComboBox.Items.IndexOf(item.Name) < 0)
                NameControl.ComboBox.Items.Insert(0, item.Name);

            NameControl.Value = item.Name;
            UrlControl.Value = item.Url;
        }

        protected override void GrabControls(Link item)
        {
            item.Name = NameControl.StringValue;
            item.Url = UrlControl.StringValue;
        }

        protected override string EmptyMandatoryField() =>
            NameControl.IsEmpty
                ? "Link name"
                : UrlControl.IsEmpty
                    ? "Url"
                    : base.EmptyMandatoryField();
    }
}
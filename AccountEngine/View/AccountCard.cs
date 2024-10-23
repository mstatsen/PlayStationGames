using OxDAOEngine.ControlFactory;
using OxDAOEngine.View;
using OxLibrary;
using OxLibrary.Panels;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.View
{
    public class AccountCard : ItemCard<AccountField, Account, AccountFieldGroup>
    {
        protected override int CardWidth => 400;
        protected override int CardHeight => 200;

        public AccountCard(ItemViewMode viewMode) : base(viewMode) 
        {
            trophiesPanel = new()
            {
                Parent = this,
                Text = "PSN Level"
            };
            trophiesPanel.Header.SetContentSize(1, 18);
            trophiesPanel.SetContentSize(140, 168);
            trophiesPanel.Paddings.SetSize(OxSize.Large);
            trophiesPanel.Top = 1;
            trophiesPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PrepareColors();
        }

        protected override void PrepareLayouts()
        {
            FillAvatarLayout();
            FillBaseLayouts();
            FillUsedAndOwnsLayout();
            FillLinksLayout();
        }

        private void FillAvatarLayout()
        {
            if (Item == null)
                return;

            ControlLayout<AccountField> avatarLayout = Layouter.AddFromTemplate(AccountField.Avatar);
            avatarLayout.CaptionVariant = ControlCaptionVariant.None;
            avatarLayout.Width = 80;
            avatarLayout.Height = 80;
        }

        private void FillBaseLayouts()
        {
            ClearLayoutTemplate();
            baseLayouts.Clear();
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            Layouter.Template.Left = Layouter[AccountField.Avatar]!.Right + 8;
            Layouter.Template.Top = 12;
            baseLayouts.Add(Layouter.AddFromTemplate(AccountField.Type));
            Layouter.Template.FontStyle = FontStyle.Regular;

            if (Item!.Login != string.Empty)
                baseLayouts.Add(Layouter.AddFromTemplate(AccountField.Login, -8));

            baseLayouts.Add(Layouter.AddFromTemplate(AccountField.Country,-8));
        }

        private void FillUsedAndOwnsLayout()
        {
            if (Item == null
                || (Item.ConsolesCount == 0
                    && Item.GamesCount == 0))
                return;

            ClearLayoutTemplate();
            Layouter.Template.Left = Layouter[AccountField.Avatar]!.Left;
            Layouter.Template.Top = Layouter[AccountField.Avatar]!.Bottom-2;
            Layouter.Template.FontStyle = FontStyle.Italic;
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            Layouter.AddFromTemplate(AccountField.UsesAndOwns);
        }

        private void FillLinksLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Width = 120;
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            Layouter.Template.BackColor = BaseColor;
            Layouter.AddFromTemplate(AccountField.Links, 12);
        }

        protected override void AlignControls()
        {
            Layouter.AlignLabels(baseLayouts);

            foreach (AccountField field in baseLayouts.Fields)
                Layouter.PlacedControl(field)!.Control.Left -= 8;

            trophiesPanel.Left = ContentContainer.Width - trophiesPanel.Width - 1;
        }

        protected override void ClearLayouts() =>
            baseLayouts.Clear();

        protected override void PrepareColors()
        {
            base.PrepareColors();
            SetPaneBaseColor(trophiesPanel, Colors.Darker());
        }

        protected override string GetTitle() =>
            Item != null ? Item.Name : string.Empty;

        private readonly OxFrameWithHeader trophiesPanel;
        private readonly ControlLayouts<AccountField> baseLayouts = new();
    }
}
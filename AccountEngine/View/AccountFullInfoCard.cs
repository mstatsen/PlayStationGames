using OxLibrary.Panels;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.View;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data;

namespace PlayStationGames.AccountEngine.View
{
    public class AccountFullInfoCard : ItemInfo<AccountField, Account, AccountFieldGroup>
    {
        private ControlLayouts<AccountField> FillAccountLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = AccountPanel;

            ControlLayout<AccountField> avatarLayout = Layouter.AddFromTemplate(AccountField.Avatar);
            avatarLayout.CaptionVariant = ControlCaptionVariant.None;
            avatarLayout.Left = 10;
            avatarLayout.Width = 80;
            avatarLayout.Height = 80;

            Layouter.Template.Left = avatarLayout.Right + 90;

            ControlLayout<AccountField> countryLayout = Layouter.AddFromTemplate(AccountField.Country);
            countryLayout.Top = 12;

            return new ControlLayouts<AccountField>()
            {
                avatarLayout,
                countryLayout
            };
        }

        protected override void PrepareLayouts()
        {
            LayoutsLists.Add(AccountPanel, FillAccountLayout());
            LayoutsLists.Add(AuthPanel, FillAuthLayout());
            LayoutsLists.Add(LinksPanel, FillLinksLayout());
            LayoutsLists.Add(PropertyPanel, FillPropertyLayout());
        }

        private ControlLayouts<AccountField> FillPropertyLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = PropertyPanel;

            return new ControlLayouts<AccountField>()
            {
            };
        }

        private ControlLayouts<AccountField> FillLinksLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = LinksPanel;
            ClearLayoutTemplate();
            Layouter.Template.Top = 8;
            Layouter.Template.Left = 4;
            Layouter.Template.Width = 120;
            Layouter.Template.BackColor = BaseColor;
            Layouter.Template.Anchors = AnchorStyles.Left | AnchorStyles.Top;
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;

            return new ControlLayouts<AccountField>()
            {
                Layouter.AddFromTemplate(AccountField.PSNProfilesLink),
                Layouter.AddFromTemplate(AccountField.StrategeLink)
            };
        }

        private ControlLayouts<AccountField> FillAuthLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = AuthPanel;

            return new ControlLayouts<AccountField>()
            {
                Layouter.AddFromTemplate(AccountField.Login),
                Layouter.AddFromTemplate(AccountField.Password)
            };
        }

        protected override string GetTitle() =>
            Item == null ? "Unknown Account" : Item.Name;

        protected override void PreparePanels()
        {
            PreparePanel(PropertyPanel, "Property");
            PreparePanel(LinksPanel, "Links");
            PreparePanel(AuthPanel, "Authentification");
            PreparePanel(AccountPanel, string.Empty);
        }

        private readonly OxPanel AccountPanel = new(new Size(200, 90));
        private readonly OxPanel AuthPanel = new(new Size(200, 90));
        private readonly OxPanel LinksPanel = new(new Size(200, 90));
        private readonly OxPanel PropertyPanel = new(new Size(200, 90));

        public AccountFullInfoCard() : base() { }
    }
}
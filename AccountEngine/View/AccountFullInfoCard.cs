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

            Layouter.Template.Top = 12;
            ControlLayout<AccountField> typeLayout = Layouter.AddFromTemplate(AccountField.Type);
            typeLayout.CaptionVariant = ControlCaptionVariant.None;
            typeLayout.Left = avatarLayout.Right + 8;
            Layouter.Template.Left = avatarLayout.Right + 70;

            return new ControlLayouts<AccountField>()
            {
                avatarLayout,
                typeLayout,
                Layouter.AddFromTemplate(AccountField.Login, 2),
                Layouter.AddFromTemplate(AccountField.Country, 2)
            };
        }

        protected override void PrepareLayouts()
        {
            LayoutsLists.Add(AccountPanel, FillAccountLayout());
            LayoutsLists.Add(PropertyPanel, FillPropertyLayout());
            LayoutsLists.Add(LinksPanel, FillLinksLayout());
        }

        private ControlLayouts<AccountField> FillPropertyLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = PropertyPanel;
            Layouter.Template.Left = 90;

            ControlLayouts<AccountField> result = new();

            bool needOffsetGames = false;

            if (Item!.ConsolesCount > 0)
            {
                result.Add(Layouter.AddFromTemplate(AccountField.Consoles));
                needOffsetGames = true;
            }

            if (Item!.GamesCount > 0)
            {
                if (needOffsetGames)
                    result.Add(Layouter.AddFromTemplate(AccountField.Games, 16));
                else result.Add(Layouter.AddFromTemplate(AccountField.Games));
            }

            return result;
        }

        private ControlLayouts<AccountField> FillLinksLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Dock = DockStyle.Top;
            Layouter.Template.BackColor = BaseColor;
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            Layouter.Template.Parent = LinksPanel;

            return new ControlLayouts<AccountField>()
            {
                Layouter.AddFromTemplate(AccountField.Links)
            };
        }

        protected override string GetTitle() =>
            Item == null ? "Unknown Account" : Item.Name;

        protected override void PreparePanels()
        {
            PreparePanel(LinksPanel, "Links");
            PreparePanel(PropertyPanel, "Property");
            PreparePanel(AccountPanel, string.Empty);
        }

        private readonly OxPanel AccountPanel = new(new Size(200, 90));
        private readonly OxPanel LinksPanel = new(new Size(200, 90));
        private readonly OxPanel PropertyPanel = new(new Size(200, 90));

        public AccountFullInfoCard() : base() { }
    }
}
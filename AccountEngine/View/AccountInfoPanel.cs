using OxLibrary.Panels;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.View;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data;
using OxDAOEngine.ControlFactory.Controls.Links;
using PlayStationGames.GameEngine.Data.Fields;
using OxLibrary;

namespace PlayStationGames.AccountEngine.View
{
    public class AccountInfoPanel : ItemInfo<AccountField, Account, AccountFieldGroup>
    {
        private ControlLayouts<AccountField> FillAccountLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = AccountPanel;

            ControlLayout<AccountField> avatarLayout = Layouter.AddFromTemplate(AccountField.Avatar);
            avatarLayout.CaptionVariant = ControlCaptionVariant.None;
            avatarLayout.Left = OxWh.W10;
            avatarLayout.Width = OxWh.W80;
            avatarLayout.Height = OxWh.W80;

            Layouter.Template.Top = OxWh.W12;
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
            Layouter.Template.Left = OxWh.W90;

            ControlLayouts<AccountField> result = new();

            if (Item is null)
                return result;

            bool needOffsetGames = false;

            if (Item.ConsolesCount > 0)
            {
                result.Add(Layouter.AddFromTemplate(AccountField.Consoles));
                needOffsetGames = true;
            }

            if (Item.GamesCount > 0)
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
            Layouter.Template.Dock = OxDock.Top;
            Layouter.Template.BackColor = BaseColor;
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            Layouter.Template.Parent = LinksPanel;

            return new ControlLayouts<AccountField>()
            {
                Layouter.AddFromTemplate(AccountField.Links)
            };
        }

        protected override string GetTitle() =>
            Item is null 
                ? "Unknown Account" 
                : Item.Name;

        protected override void PreparePanels()
        {
            PreparePanel(LinksPanel, "Links");
            PreparePanel(PropertyPanel, "Property");
            PreparePanel(AccountPanel, string.Empty);
        }

        protected override void AfterControlLayout()
        {
            base.AfterControlLayout();
            AlignLinkButtons();
        }
        private void AlignLinkButtons() =>
            ((LinkButtonList)Layouter.PlacedControl(AccountField.Links)!.Control!).
                RecalcButtonsSizeAndPositions();

        private readonly OxPane AccountPanel = new();
        private readonly OxPane LinksPanel = new();
        private readonly OxPane PropertyPanel = new();

        public AccountInfoPanel() : base() { }
    }
}
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Types;
using OxDAOEngine.View;
using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Panels;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.AccountEngine.View
{
    public class AccountCard : ItemCard<AccountField, Account, AccountFieldGroup>
    {
        protected override int CardWidth => 424;
        protected override int CardHeight => 200;

        public AccountCard(ItemViewMode viewMode) : base(viewMode) 
        {
            TrophiesPanel = new()
            {
                Parent = this,
                Text = "PSN Level",
                HeaderHeight = 18,
                Top = 1,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            TrophiesPanel.SetContentSize(164, 168);
            TrophiesPanel.Paddings.SetSize(OxSize.Large);
            PrepareColors();
        }

        protected override void PrepareLayouts()
        {
            FillAvatarLayout();
            FillBaseLayouts();
            FillUsedAndOwnsLayout();
            FillLinksLayout();
            FillTrophiesLayouts();
        }

        private void FillTrophiesLayouts()
        {
            ClearLayoutTemplate();
            RenewTrophiesIcons();
            TrophiesLayouts.Clear();
            Layouter.Template.Parent = TrophiesPanel;
            Layouter.Template.Left = 94;
            Layouter.Template.WrapLabel = false;
            TrophiesLayouts.Add(Layouter.AddFromTemplate(AccountField.PlayedGames));
            TrophiesLayouts.Add(Layouter.AddFromTemplate(AccountField.PSNLevel, -10));
            TrophiesLayouts.Add(Layouter.AddFromTemplate(AccountField.TotalPoints, -10));
            TrophiesLayouts.Add(Layouter.AddFromTemplate(AccountField.CompletedGames, -10));
            TrophiesLayouts.Add(Layouter.AddFromTemplate(AccountField.TotalTrophies, -10));

            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            TrophiesLayouts.Add(Layouter.AddFromTemplate(AccountField.PlatinumCount, -10));
            TrophiesLayouts.Add(Layouter.AddFromTemplate(AccountField.GoldCount, -10));
            TrophiesLayouts.Add(Layouter.AddFromTemplate(AccountField.SilverCount, -10));
            TrophiesLayouts.Add(Layouter.AddFromTemplate(AccountField.BronzeCount, -10));
        }

        private void FillAvatarLayout()
        {
            if (Item is null)
                return;

            ControlLayout<AccountField> avatarLayout = Layouter.AddFromTemplate(AccountField.Avatar);
            avatarLayout.CaptionVariant = ControlCaptionVariant.None;
            avatarLayout.Width = 80;
            avatarLayout.Height = 80;
        }

        private void FillBaseLayouts()
        {
            ClearLayoutTemplate();
            BaseLayouts.Clear();
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            Layouter.Template.Left = Layouter[AccountField.Avatar]!.Right + 8;
            Layouter.Template.Top = 12;
            BaseLayouts.Add(Layouter.AddFromTemplate(AccountField.Type));
            Layouter.Template.FontStyle = FontStyle.Regular;

            if (!Item!.Login.Equals(string.Empty))
                BaseLayouts.Add(Layouter.AddFromTemplate(AccountField.Login, -8));

            BaseLayouts.Add(Layouter.AddFromTemplate(AccountField.Country,-8));
        }

        private void FillUsedAndOwnsLayout()
        {
            if (Item is null
                || (Item.ConsolesCount is 0
                    && Item.GamesCount is 0))
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
            Layouter.AlignLabels(BaseLayouts, 8);
            Layouter.MovePlacedControlsToLeft(TrophiesLayouts, 8);
            TrophiesPanel.Left = ContentContainer.Width - TrophiesPanel.Width - 1;
            AlignTrophiesIcons();
        }

        private void AlignTrophiesIcons()
        {
            foreach (KeyValuePair<TrophyType, OxPicture> icon in TrophiesIcons)
            {
                PlacedControl<AccountField> trophyControl =
                    Layouter.PlacedControl(trophyHelper.FieldForAccount(icon.Key))!;
                OxControlHelper.AlignByBaseLine(
                    trophyControl.Control,
                    icon.Value
                    );
                icon.Value.Left = trophyControl.Control.Left - 32;
            }
        }

        protected override void ClearLayouts() =>
            BaseLayouts.Clear();

        protected override void PrepareColors()
        {
            base.PrepareColors();
            SetPaneBaseColor(TrophiesPanel, Colors.Darker());
        }

        protected override string GetTitle() =>
            Item is not null 
                ? Item.Name 
                : string.Empty;

        private readonly TrophyTypeHelper trophyHelper = TypeHelper.Helper<TrophyTypeHelper>();

        private void RenewTrophiesIcons()
        {
            foreach (OxPicture icon in TrophiesIcons.Values)
                icon.Dispose();

            TrophiesIcons.Clear();

            foreach (TrophyType trophyType in trophyHelper.All())
                TrophiesIcons.Add(
                    trophyType,
                    new OxPicture()
                    {
                        Image = trophyHelper.Icon(trophyType),
                        Parent = TrophiesPanel,
                        Left = 0
                    }
                );
        }

        private readonly Dictionary<TrophyType, OxPicture> TrophiesIcons = new();
        private readonly OxFrameWithHeader TrophiesPanel;
        private readonly ControlLayouts<AccountField> BaseLayouts = new();
        private readonly ControlLayouts<AccountField> TrophiesLayouts = new();
    }
}
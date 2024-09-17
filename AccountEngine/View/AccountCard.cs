using OxLibrary.Controls;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.View;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;


namespace PlayStationGames.AccountEngine.View
{
    public class AccountCard : ItemCard<AccountField, Account, AccountFieldGroup>
    {
        protected override int CardWidth => 350;
        protected override int CardHeight => 260;

        public AccountCard(ItemViewMode viewMode) : base(viewMode) { }

        protected override void PrepareLayouts()
        {
            FillAvatarLayout();
            FillBaseLayouts();
            FillBottomLayouts();
        }

        private void FillAvatarLayout()
        {
            if (Item == null)
                return;

            ControlLayout<AccountField> avatarLayout = Layouter.AddFromTemplate(AccountField.Avatar);
            avatarLayout.CaptionVariant = ControlCaptionVariant.None;
            avatarLayout.Width = 108;
            avatarLayout.Height = 60;
        }

        private void FillBaseLayouts()
        {
            ClearLayoutTemplate();
            baseLayouts.Clear();
            Layouter.Template.Left = Layouter[AccountField.Avatar]!.Right + 84;
            baseLayouts.Add(Layouter.AddFromTemplate(AccountField.Login));
            baseLayouts.Add(Layouter.AddFromTemplate(AccountField.Password));
            baseLayouts.Add(Layouter.AddFromTemplate(AccountField.Country));
        }

        private void FillBottomLayouts()
        {
            ClearLayoutTemplate();
            bottomLayouts.Clear();

            if (Item == null)
                return;

            Layouter.Template.Left = Layouter[AccountField.Avatar]!.Left + 72;
            Layouter.Template.Top = Layouter[AccountField.Avatar]!.Bottom + 12;
            bottomLayouts.Add(Layouter.AddFromTemplate(AccountField.Consoles, 16));
            bottomLayouts.Add(Layouter.AddFromTemplate(AccountField.Games, 16));
        }

        protected override void AlignControls()
        {
            Layouter.AlignLabels(baseLayouts);
            Layouter.AlignLabels(bottomLayouts);

            foreach (AccountField field in baseLayouts.Fields)
                Layouter.PlacedControl(field)!.Control.Left -= 8;
        }

        protected override void ClearLayouts()
        {
            baseLayouts.Clear();
            bottomLayouts.Clear();
        }

        protected override string GetTitle() =>
            Item != null ? Item.Name : string.Empty;

        protected override void AfterLayoutControls()
        {
            base.AfterLayoutControls();
            WrapStoragesAndFolders();
        }

        private void WrapStoragesAndFolders()
        {
            OxLabel? consolesControl =
                (OxLabel?)Layouter.PlacedControl(AccountField.Consoles)?.Control;

            if (consolesControl != null)
            {
                consolesControl.MaximumSize = new Size(290, 46);
                consolesControl.TextAlign = ContentAlignment.TopLeft;
            }

            OxLabel? gamesControl =
                (OxLabel?)Layouter.PlacedControl(AccountField.Consoles)?.Control;

            if (gamesControl != null)
            {
                gamesControl.MaximumSize = new Size(290, 46);
                gamesControl.TextAlign = ContentAlignment.TopLeft;
            }
        }

        private readonly ControlLayouts<AccountField> baseLayouts = new();
        private readonly ControlLayouts<AccountField> bottomLayouts = new();
    }
}
using OxLibrary.Panels;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using OxXMLEngine.ControlFactory;
using OxXMLEngine.Data.Types;
using OxXMLEngine.View;
using OxLibrary.Controls;
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
            avatarLayout.Width = 96;
            avatarLayout.Height = 60;

            Layouter.Template.Left = avatarLayout.Right + 90;

            ControlLayout<AccountField> loginLayout = Layouter.AddFromTemplate(AccountField.Login);
            loginLayout.Top = 2;

            return new ControlLayouts<AccountField>()
            {
                avatarLayout,
                loginLayout
            };
        }

        private readonly ConsoleGenerationHelper generationHelper = 
            TypeHelper.Helper<ConsoleGenerationHelper>();


        protected override void PrepareLayouts()
        {
            LayoutsLists.Add(FillAccountLayout());
        }

        protected override string GetTitle() =>
            Item == null ? "Unknown Account" : Item.Name;

        protected override void PreparePanels()
        {
            PreparePanel(AccountPanel, string.Empty);
        }

        private readonly OxPanel AccountPanel = new(new Size(200, 90));

        public AccountFullInfoCard() : base() { }
    }
}
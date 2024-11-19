using OxLibrary.Panels;
using OxDAOEngine.Editor;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data;
using OxLibrary;

namespace PlayStationGames.AccountEngine.Editor
{
    public partial class AccountEditor : DAOEditor<AccountField, Account, AccountFieldGroup>
    {
        public AccountEditor() : base() { }

        public override Bitmap? FormIcon => OxIcons.Account;

        protected override OxPane? GroupParent(AccountFieldGroup group) => MainPanel;

        protected override void RecalcPanels()
        {
            MinimumSize = Size.Empty;
            MaximumSize = Size.Empty;
            MainPanel.SetContentSize(
                420,
                Groups[AccountFieldGroup.Games].Bottom + 15
            );
        }

        protected override void SetFrameMargin(AccountFieldGroup group, OxFrame frame)
        {
            base.SetFrameMargin(group, frame);
            frame.Margins.RightOx = OxSize.M;

            if (group is AccountFieldGroup.Games) 
                frame.Margins.BottomOx = OxSize.M;
        }

        protected override void SetPaddings()
        {
            base.SetPaddings();
            Groups[AccountFieldGroup.Consoles].Paddings.SetSize(OxSize.S);
            Groups[AccountFieldGroup.Games].Paddings.SetSize(OxSize.S);
        }
    }
}
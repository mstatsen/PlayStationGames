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

        protected override OxPane? GroupParent(AccountFieldGroup group) => MainPanel;

        protected override void RecalcPanels()
        {
            MinimumSize = new Size(0, 0);
            MaximumSize = new Size(0, 0);
            MainPanel.SetContentSize(
                420,
                Groups[AccountFieldGroup.Games].Bottom + 15
            );
        }

        protected override void SetFrameMargin(AccountFieldGroup group, OxFrame frame)
        {
            base.SetFrameMargin(group, frame);
            frame.Margins.RightOx = OxSize.Extra;

            if (group == AccountFieldGroup.Games) 
                frame.Margins.BottomOx = OxSize.Extra;
        }

        protected override void SetPaddings()
        {
            base.SetPaddings();
            Groups[AccountFieldGroup.Consoles].Paddings.SetSize(OxSize.Large);
            Groups[AccountFieldGroup.Games].Paddings.SetSize(OxSize.Large);
        }
    }
}
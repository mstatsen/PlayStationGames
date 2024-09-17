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
                400,
                Groups[AccountFieldGroup.Property].Bottom + 16
            );
        }

        protected override void SetFrameMargin(AccountFieldGroup group, OxFrame frame)
        {
            base.SetFrameMargin(group, frame);
            frame.Margins.RightOx = OxSize.Extra;

            if (group == AccountFieldGroup.Property) 
                frame.Margins.BottomOx = OxSize.Extra;
        }

        protected override void SetPaddings()
        {
            base.SetPaddings();
            Groups[AccountFieldGroup.Property].Paddings.Bottom = 6;
        }
    }
}
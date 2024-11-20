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
            MainPanel.Size = new(
                420,
                Groups[AccountFieldGroup.Games].Bottom + 15
            );
        }

        protected override void SetFrameMargin(AccountFieldGroup group, OxFrame frame)
        {
            base.SetFrameMargin(group, frame);
            frame.Margin.Right = OxSize.M;

            if (group is AccountFieldGroup.Games) 
                frame.Margin.Bottom = OxSize.M;
        }

        protected override void SetPaddings()
        {
            base.SetPaddings();
            Groups[AccountFieldGroup.Consoles].Padding.Size = OxSize.S;
            Groups[AccountFieldGroup.Games].Padding.Size = OxSize.S;
        }
    }
}
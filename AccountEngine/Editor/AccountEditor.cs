using OxLibrary;
using OxLibrary.Panels;
using OxDAOEngine.Editor;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data;


namespace PlayStationGames.AccountEngine.Editor
{
    public partial class AccountEditor : DAOEditor<AccountField, Account, AccountFieldGroup>
    {
        public AccountEditor() : base() { }

        public override Bitmap? FormIcon => OxIcons.Account;

        protected override OxPanel? GroupParent(AccountFieldGroup group) => FormPanel;

        protected override void RecalcPanels()
        {
            MinimumSize = new();
            MaximumSize = new();
            FormPanel.Size = new(
                420,
                (short)(Groups[AccountFieldGroup.Games].Bottom + 15)
            );
        }

        protected override void SetFrameMargin(AccountFieldGroup group, OxFrame frame)
        {
            base.SetFrameMargin(group, frame);
            frame.Margin.Right = 8;

            if (group is AccountFieldGroup.Games) 
                frame.Margin.Bottom = 8;
        }

        protected override void SetPaddings()
        {
            base.SetPaddings();
            Groups[AccountFieldGroup.Consoles].Padding.Size = 4;
            Groups[AccountFieldGroup.Games].Padding.Size = 4;
        }
    }
}
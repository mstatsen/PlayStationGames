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
                OxWh.W420,
                OxWh.Add(Groups[AccountFieldGroup.Games].Bottom, OxWh.W15)
            );
        }

        protected override void SetFrameMargin(AccountFieldGroup group, OxFrame frame)
        {
            base.SetFrameMargin(group, frame);
            frame.Margin.Right = OxWh.W8;

            if (group is AccountFieldGroup.Games) 
                frame.Margin.Bottom = OxWh.W8;
        }

        protected override void SetPaddings()
        {
            base.SetPaddings();
            Groups[AccountFieldGroup.Consoles].Padding.Size = OxWh.W4;
            Groups[AccountFieldGroup.Games].Padding.Size = OxWh.W4;
        }
    }
}
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Fields;
using OxLibrary;
using OxLibrary.Dialogs;
using OxLibrary.Panels;
using PlayStationGames.AccountEngine.ControlFactory.Accessors;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.GameEngine.ControlFactory.Initializers;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls.Trophies
{
    public class TrophysetAccountEditor : OxPanel
    {
        private readonly TrophysetPanel TrophysetPanel;
        private AccountAccessor<GameField, Game> AccountAccessor = default!;

        public TrophysetAccountEditor(TrophysetPanel trophysetPanel) : base()
        {
            TrophysetPanel = trophysetPanel;
            Text = "Select account";
            Paddings.SetSize(OxSize.M);
        }

        protected override void PrepareInnerControls()
        {
            base.PrepareInnerControls();
            AccountAccessor = (AccountAccessor<GameField,Game>)DataManager
                .Builder<GameField, Game>(ControlScope.Editor)
                .Accessor("Trophyset:Account", FieldType.Enum,
                    new AccountAccessorParameters()
                    {
                        UseNullable = false,
                        OnlyNullable = false
                    });
            AccountAccessor.Parent = ContentContainer;
            AccountAccessor.Dock = DockStyle.Fill;
            AccountAccessor.Context.SetInitializer(accountInitializer);
        }

        public Guid? SelectedAccountId => AccountAccessor.GuidValue;

        private readonly AccountInitializer accountInitializer = new();

        protected override void PrepareDialog(OxPanelViewer dialog)
        {
            base.PrepareDialog(dialog);

            accountInitializer.ExistingAccounts = new ListDAO<Account>();

            foreach (TrophiesPanel trophiesPanel in TrophysetPanel.VisiblePanels)
                accountInitializer.ExistingAccounts.Add(trophiesPanel.Account!);

            AccountAccessor.RenewControl(true);
            dialog.DialogButtons = OxDialogButton.OK | OxDialogButton.Cancel;
            dialog.SetContentSize(400, 52);
        }

        protected override void PrepareColors()
        {
            base.PrepareColors();

            if (AccountAccessor is not null)
                AccountAccessor.Control.BackColor = BackColor;
        }
    }
}

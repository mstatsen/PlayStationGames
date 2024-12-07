using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Fields;
using OxLibrary;
using PlayStationGames.AccountEngine.ControlFactory.Accessors;
using PlayStationGames.ConsoleEngine.ControlFactory.Initializers;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public partial class AccountEditor : CustomItemEditor<ConsoleAccount, ConsoleField, PSConsole>
    {
        protected override void FillControls(ConsoleAccount item) =>
            accountControl!.Value = item.Id;

        public override Bitmap? FormIcon => OxIcons.Account;

        protected override OxWidth ContentHeight => 
            accountControl is null 
                ? OxWh.W36
                : OxWh.Add(accountControl.Bottom, OxWh.W12);

        protected override void GrabControls(ConsoleAccount item) => 
            item.Id = accountControl!.GuidValue;

        protected override void CreateControls()
        {
            accountControl = 
                Context.Accessor(
                    "ConsoleAccount", 
                    FieldType.Custom, 
                    new AccountAccessorParameters() 
                        { 
                            UseNullable = false 
                        }
                    );

            accountControl.Parent = this;
            accountControl.Dock = DockStyle.Fill;
            accountControl.Context.SetInitializer(accountInitializer);
        }

        protected override string EmptyMandatoryField() =>
            accountControl!.IsEmpty 
                ? "Account" 
                : base.EmptyMandatoryField();

        private IControlAccessor? accountControl;
        private readonly AccountInitializer accountInitializer = new();

        protected override void SetPaddings() =>
            Padding.Size = OxWh.W8;

        public override void RenewData()
        {
            base.RenewData();

            if (accountControl is null)
                return;

            if (ExistingItems is not null)
                accountInitializer.ExistingAccounts = new ListDAO<ConsoleAccount>(ExistingItems);

            accountControl!.RenewControl(true);
        }
    }
}
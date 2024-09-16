using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxXMLEngine.ControlFactory.Accessors;
using OxXMLEngine.ControlFactory.Controls;
using OxXMLEngine.Data.Fields;
using PlayStationGames.AccountEngine.ControlFactory.Accessors;
using OxLibrary;
using OxXMLEngine.Data;
using PlayStationGames.ConsoleEngine.ControlFactory.Initializers;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public partial class AccountEditor : ListItemEditor<ConsoleAccount, ConsoleField, PSConsole>
    {
        protected override void FillControls(ConsoleAccount item) =>
            accountControl!.Value = item.Id;

        protected override int ContentHeight => 
            accountControl == null 
                ? 36 
                : accountControl.Bottom + 12;

        protected override void GrabControls(ConsoleAccount item) => 
            item.Id = accountControl!.GuidValue;

        protected override string Title => "Account";

        protected override void CreateControls()
        {
            accountControl = 
                Context.Builder.Accessor(
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
            MainPanel.Paddings.SetSize(OxSize.Extra);


        public override void RenewData()
        {
            base.RenewData();

            if (accountControl == null)
                return;

            if (ExistingItems != null)
                accountInitializer.ExistingAccounts = new ListDAO<ConsoleAccount>(ExistingItems);

            accountControl!.RenewControl(true);
        }
    }
}
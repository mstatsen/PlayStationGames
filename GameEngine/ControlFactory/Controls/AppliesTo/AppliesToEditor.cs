using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data.Fields;
using OxLibrary;
using PlayStationGames.GameEngine.ControlFactory.Controls.Initializers;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class AppliesToEditor : CustomItemEditor<Platform, GameField, Game>
    {
        private EnumAccessor<GameField, Game, PlatformType> TypeControl = default!;

        public override Bitmap FormIcon => OxIcons.Console;

        public override void RenewData()
        {
            base.RenewData();

            if (TypeControl.Context.Initializer is PlaystationPlatformTypeInitializer playstationPlatformTypeInitializer)
            {
                playstationPlatformTypeInitializer.ExistingTypes.Clear();
                playstationPlatformTypeInitializer.Game = OwnerDAO;

                if (ExistingItems is not null)
                    playstationPlatformTypeInitializer.ExistingTypes.AddRange(ExistingItems.Cast<Platform>());

                TypeControl.RenewControl(true);
            }
        }

        private void CreateTypeControl()
        {
            TypeControl = (EnumAccessor<GameField, Game, PlatformType>)Context.Builder
                .Accessor("AppliesTo:Type", FieldType.Enum);
            TypeControl.Parent = this;
            TypeControl.Left = 8;
            TypeControl.Top = 8;
            TypeControl.Width =
                OxWh.Int(
                    OxWh.Sub(
                        OxWh.Sub(FormPanel.Width, TypeControl.Left), 
                        OxWh.W8
                    )
                );
            TypeControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            TypeControl.Height = 24;
            SetKeyUpHandler(TypeControl.Control);
            FirstFocusControl = TypeControl.Control;
        }

        protected override void CreateControls() =>
            CreateTypeControl();

        protected override OxWidth ContentHeight => OxWh.Add(TypeControl.Bottom, OxWh.W8);

        protected override void FillControls(Platform item) => 
            TypeControl.Value = item.Type;

        protected override void GrabControls(Platform item) =>
            item.Type = TypeControl.EnumValue;

        protected override string EmptyMandatoryField() =>
            TypeControl.IsEmpty
                ? "Type"
                : base.EmptyMandatoryField();
    }
}
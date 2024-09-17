using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class DLCEditor : ListItemEditor<DLC, GameField, Game>
    {
        private IControlAccessor NameControl = default!;

        protected override string Title => "DLC";

        protected override void CreateControls()
        {
            NameControl = Context.Builder.Accessor("DLC_Name", FieldType.Memo);
            NameControl.Parent = this;
            NameControl.Dock = DockStyle.Fill;
        }

        protected override void FillControls(DLC item) =>
            NameControl.Value = item.Name;

        protected override void GrabControls(DLC item) =>
            item.Name = NameControl.StringValue;

        protected override string EmptyMandatoryField() =>
            NameControl.IsEmpty ? "DLC name" : base.EmptyMandatoryField();
    }
}
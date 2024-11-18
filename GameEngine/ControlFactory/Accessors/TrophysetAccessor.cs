using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Context;
using OxDAOEngine.ControlFactory.ValueAccessors;
using PlayStationGames.GameEngine.ControlFactory.Controls.Trophies;
using PlayStationGames.GameEngine.ControlFactory.ValuesAccessors;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Accessors
{
    public class TrophysetAccessor : ControlAccessor<GameField, Game>
    {
        public TrophysetAccessor(IBuilderContext<GameField, Game> context) : base(context) { }

        public override void Clear() =>
            TrophysetControl.ClearValues();

        protected override Control CreateControl() => 
            new TrophysetPanel(Context.Key is "DLC:Trophyset");

        public TrophysetPanel TrophysetControl => (TrophysetPanel)Control;

        protected override ValueAccessor CreateValueAccessor() => new TrophysetValueAccessor();

        protected override void AssignValueChangeHanlderToControl(EventHandler? value) => 
            TrophysetControl.ValueChanged += value;

        protected override void UnAssignValueChangeHanlderToControl(EventHandler? value) => 
            TrophysetControl.ValueChanged -= value;

        protected override void SetReadOnly(bool value) => 
            TrophysetControl.ReadOnly = value;

        protected override Control? CreateReadOnlyControl() => null;
    }
}

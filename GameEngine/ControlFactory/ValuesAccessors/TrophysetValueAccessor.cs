using OxDAOEngine.ControlFactory.ValueAccessors;
using PlayStationGames.GameEngine.ControlFactory.Controls.Trophies;
using PlayStationGames.GameEngine.Data;

namespace PlayStationGames.GameEngine.ControlFactory.ValuesAccessors
{
    public class TrophysetValueAccessor : ValueAccessor
    {
        TrophysetPanel TrophysetControl => (TrophysetPanel)Control;
        public override object? GetValue() =>
            TrophysetControl.Value;

        public override void SetValue(object? value) =>
            TrophysetControl.Value = (Trophyset?)value;
    }
}

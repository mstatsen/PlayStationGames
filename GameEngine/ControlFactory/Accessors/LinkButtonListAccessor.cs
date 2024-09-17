using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Context;
using OxDAOEngine.ControlFactory.ValueAccessors;
using PlayStationGames.GameEngine.ControlFactory.Controls;
using PlayStationGames.GameEngine.ControlFactory.ValueAccessors;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Accessors
{
    public class LinkButtonListAccessor : ControlAccessor<GameField, Game>
    {
        public LinkButtonListAccessor(IBuilderContext<GameField, Game> context, ButtonListDirection buttonListDirection) 
            : base(context) =>
            LinkButtonListControl.Direction = buttonListDirection;

        protected override Control CreateControl() =>
            new LinkButtonList();

        public override void Clear() =>
            LinkButtonListControl.Clear();

        public LinkButtonList LinkButtonListControl =>
            (LinkButtonList)Control;

        protected override ValueAccessor CreateValueAccessor() =>
            new LinkButtonListValueAccessor(Context);

        protected override void UnAssignValueChangeHanlderToControl(EventHandler? value) { }

        protected override void AssignValueChangeHanlderToControl(EventHandler? value) { }
    }
}
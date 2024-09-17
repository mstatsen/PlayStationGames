using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Context;
using OxDAOEngine.ControlFactory.ValueAccessors;
using OxDAOEngine.Data;
using PlayStationGames.GameEngine.ControlFactory.Controls;
using PlayStationGames.GameEngine.ControlFactory.ValueAccessors;

namespace PlayStationGames.GameEngine.ControlFactory.Accessors
{
    public class LinkButtonAccessor<TField, TDAO> : ControlAccessor<TField, TDAO>
        where TField : notnull, Enum
        where TDAO : RootDAO<TField>, new()
    {
        public LinkButtonAccessor(IBuilderContext<TField, TDAO> context) : base(context) { }

        protected override Control CreateControl() =>
            new LinkButton(string.Empty, string.Empty);

        protected override ValueAccessor CreateValueAccessor() =>
            new LinkButtonValueAccessor();

        public override bool IsEmpty =>
            base.IsEmpty || Value == null || Value.ToString() == string.Empty;

        public LinkButton ButtonControl =>
            (LinkButton)Control;

        protected override void UnAssignValueChangeHanlderToControl(EventHandler? value) { }

        protected override void AssignValueChangeHanlderToControl(EventHandler? value) { }

        public override void Clear() => 
            Value = string.Empty;
    }
}
using OxXMLEngine.ControlFactory.Context;
using OxXMLEngine.ControlFactory.ValueAccessors;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Fields;
using PlayStationGames.GameEngine.ControlFactory.Accessors;
using PlayStationGames.GameEngine.ControlFactory.Controls;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.ValueAccessors
{
    public class LinkButtonListValueAccessor : ValueAccessor
    {
        private readonly List<LinkButtonAccessor<GameField, Game>> ButtonsAccessors = new();

        private readonly IBuilderContext<GameField, Game> Context;

        public LinkButtonListValueAccessor(IBuilderContext<GameField, Game> context) : base() =>
            Context = context;

        private LinkButtonList LinkButtonList => (LinkButtonList)Control;

        public override object GetValue()
        {
            ListDAO<Link> linkList = new();

            foreach (LinkButtonAccessor<GameField, Game> accessor in ButtonsAccessors)
            {
                Link? value = accessor.DAOValue<Link>();

                if (value != null)
                    linkList.Add(value);
            }
            
            return linkList;
        }

        public override void SetValue(object? value)
        {
            if (value == null || (value is not ListDAO<Link> links))
                return;

            LinkButtonList.Clear();
            ButtonsAccessors.Clear();
            links.Sort();

            foreach (Link link in links)
            {
                LinkButtonAccessor<GameField, Game> accessor = (LinkButtonAccessor<GameField, Game>)Context.Builder
                    .Accessor("Link", FieldType.Custom, link.Url);
                accessor.Value = link;
                accessor.Parent = LinkButtonList;
                accessor.ButtonControl.BaseColor = link.LinkColor;
                ButtonsAccessors.Add(accessor);
                LinkButtonList.AddButton(accessor.ButtonControl);
            }

            return;
        }
    }
}
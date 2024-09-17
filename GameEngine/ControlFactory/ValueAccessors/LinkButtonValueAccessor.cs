using OxDAOEngine.ControlFactory.ValueAccessors;
using PlayStationGames.GameEngine.ControlFactory.Controls;
using PlayStationGames.GameEngine.Data;

namespace PlayStationGames.GameEngine.ControlFactory.ValueAccessors
{
    public class LinkButtonValueAccessor : ValueAccessor
    {
        private LinkButton LinkButton => (LinkButton)Control;
        public override object? GetValue() =>
            new Link() 
            { 
                Name = LinkButton.Text ?? string.Empty,
                Url = LinkButton.Url
            };

        public override void SetValue(object? value)
        {
            if (value == null || 
                (value is not Link link))
                return;

            LinkButton.Text = link.Name;
            LinkButton.Url = link.Url;
        }
    }
}
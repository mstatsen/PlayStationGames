using OxXMLEngine.ControlFactory.Accessors;
using OxXMLEngine.ControlFactory.Context;
using OxXMLEngine.Data;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class AccountAccessor<TField, TDAO> : ComboBoxAccessor<TField, TDAO>
        where TField : notnull, Enum
        where TDAO : RootDAO<TField>, new()
    {
        public AccountAccessor(IBuilderContext<TField, TDAO> context) : base(context) { }

        protected override void InitControl()
        {
            base.InitControl();
            ComboBox.Items.Clear();

            for (int y = DateTime.Today.Year; y > 1990; y--)
                ComboBox.Items.Add(y);
        }
    }
}

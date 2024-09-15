using OxXMLEngine.Data;
using OxXMLEngine.XML;
using PlayStationGames.AccountEngine.ControlFactory.ValueAccessors;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class ConsoleAccount : DAO
    {
        private Guid id = AccountValueAccessor.NullAccount.Id;
        public Guid Id
        {
            get => id;
            set => id = ModifyValue(id, value);
        }

        public override void Clear()
        {
            id = AccountValueAccessor.NullAccount.Id;
        }

        public override void Init() { }

        protected override void LoadData(XmlElement element) =>
            id = XmlHelper.ValueGuid(element, XmlConsts.Id);

        protected override void SaveData(XmlElement element, bool clearModified = true) =>
            XmlHelper.AppendElement(element, XmlConsts.Id, id);

        public override string ToString()
        {
            Account? account = DataManager.Item<AccountField, Account>(AccountField.Id, id);
            return account != null ? account.ToString() : AccountValueAccessor.NullAccount.Name;
        }
    }
}

using System;
using System.Xml;
using OxDAOEngine.Data;
using OxDAOEngine.XML;
using PlayStationGames.AccountEngine.ControlFactory.ValueAccessors;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class ConsoleAccount : DAO
    {
        private Guid id = AccountValueAccessor.NullAccount.Id;
        public Guid Id
        {
            get => id;
            set
            {
                id = ModifyValue(id, value);
                savedToString = null;
            }
        }

        public override void Clear()
        {
            id = AccountValueAccessor.NullAccount.Id;
            savedToString = null;
        }

        public override void Init() { }

        protected override void LoadData(XmlElement element)
        {
            id = XmlHelper.ValueGuid(element, XmlConsts.Id);

            if (State == DAOState.Coping)
                savedToString = (string?)XmlHelper.Value(element, "ToStringValue");
            else
                if (State == DAOState.Loading)
                CalcToString();
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.Id, id);

            if (State == DAOState.Coping)
                XmlHelper.AppendElement(element, "ToStringValue", savedToString);
        }

        private string? savedToString;

        private void CalcToString()
        {
            Account? account = DataManager.Item<AccountField, Account>(AccountField.Id, id);
            savedToString = account != null ? account.ToString() : AccountValueAccessor.NullAccount.Name;
        }

        public override string? ToString()
        {
            if (savedToString == null)
                CalcToString();

            return savedToString;
        }

        public override bool Equals(object? obj) =>
            obj is ConsoleAccount otherAccount
            && (base.Equals(obj)
                || Id.Equals(otherAccount.Id));

        public override int GetHashCode() => Id.GetHashCode();
    }
}

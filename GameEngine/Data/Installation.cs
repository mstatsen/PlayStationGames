using System.Xml;
using OxDAOEngine.Data;
using OxDAOEngine.XML;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data
{
    public class Installation : DAO
    {
        private Guid consoleId = Guid.Empty;
        private Guid storageId = Guid.Empty;
        private string folder = string.Empty;

        public Guid ConsoleId
        {
            get => consoleId;
            set => consoleId = ModifyValue(consoleId, value);
        }

        public Guid StorageId
        {
            get => storageId;
            set => storageId = ModifyValue(storageId, value);
        }

        public string Folder
        {
            get => folder;
            set => folder = StringValue(ModifyValue(folder, value));
        }

        public override void Clear()
        {
            ConsoleId = Guid.Empty;
            StorageId = Guid.Empty;
            Folder = string.Empty;
        }

        public override void Init() { }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.ConsoleId, ConsoleId);

            if (StorageId != Guid.Empty)
                XmlHelper.AppendElement(element, XmlConsts.StorageId, StorageId);

            XmlHelper.AppendElement(element, XmlConsts.Folder, Folder, true);
        }

        protected override void LoadData(XmlElement element)
        {
            consoleId = XmlHelper.ValueGuid(element, XmlConsts.ConsoleId);
            storageId = XmlHelper.ValueGuid(element, XmlConsts.StorageId);
            folder = XmlHelper.Value(element, XmlConsts.Folder);
        }

        public override bool Equals(object? obj) =>
            obj is Installation otherInstallation
            && (base.Equals(obj)
                || (ConsoleId.Equals(otherInstallation.ConsoleId)
                    && StorageId.Equals(otherInstallation.StorageId)
                    && Folder.Equals(otherInstallation.Folder)));

        public override int GetHashCode() => 
            HashCode.Combine(ConsoleId, StorageId, Folder);

        public override string? ToString()
        {
            PSConsole? console = DataManager.Item<ConsoleField, PSConsole>(ConsoleField.Id, ConsoleId);

            if (console == null)
                return null;

            string folderName = console.Folders.CheckName(Folder);

            if (folderName != string.Empty)
                folderName = $"/{folderName}";

            return console == null
                ? base.ToString() 
                : $"{console.Name}/{console.Storages.StorageName(StorageId)}{folderName}";
        }
    }
}
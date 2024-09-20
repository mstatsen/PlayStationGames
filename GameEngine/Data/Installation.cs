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
        private int size = 0;

        public int Size
        {
            get => size;
            set => size = ModifyValue(size, value);
        }

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
            Size = 0;
        }

        public override void Init() { }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.ConsoleId, ConsoleId);

            if (StorageId != Guid.Empty)
                XmlHelper.AppendElement(element, XmlConsts.StorageId, StorageId);

            XmlHelper.AppendElement(element, XmlConsts.Folder, Folder, true);

            if (Size > 0)
                XmlHelper.AppendElement(element, XmlConsts.Size, Size);

        }

        protected override void LoadData(XmlElement element)
        {
            consoleId = XmlHelper.ValueGuid(element, XmlConsts.ConsoleId);
            storageId = XmlHelper.ValueGuid(element, XmlConsts.StorageId);
            folder = XmlHelper.Value(element, XmlConsts.Folder);
            size = XmlHelper.ValueInt(element, XmlConsts.Size);
        }

        public override bool Equals(object? obj) =>
            obj is Installation otherInstallation
            && (base.Equals(obj)
                || (ConsoleId.Equals(otherInstallation.ConsoleId)
                    && StorageId.Equals(otherInstallation.StorageId)
                    && Folder.Equals(otherInstallation.Folder))
                    && Size.Equals(otherInstallation.Size));

        public override int GetHashCode() => 
            HashCode.Combine(ConsoleId, StorageId, Folder, Size);

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
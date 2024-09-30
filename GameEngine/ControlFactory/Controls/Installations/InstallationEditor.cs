using OxLibrary.Controls;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using OxLibrary;
using PlayStationGames.ConsoleEngine.ControlFactory.Initializers;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class InstallationEditor : ListItemEditor<Installation, GameField, Game>
    {
        private ExtractAccessor<ConsoleField, PSConsole> consoleControl = default!;
        private IControlAccessor storageControl = default!;
        private IControlAccessor folderControl = default!;
        private IControlAccessor sizeControl = default!;
        private OxLabel storageLabel = default!;
        private OxLabel folderLabel = default!;
        private OxLabel sizeLabel = default!;
        private OxLabel sizeLabel2 = default!;

        private static ControlBuilder<ConsoleField, PSConsole> ConsoleBuilder =>
            DataManager.Builder<ConsoleField, PSConsole>(ControlScope.Editor);

        protected override void CreateControls()
        {
            consoleControl = (ExtractAccessor<ConsoleField, PSConsole>)ConsoleBuilder[ConsoleField.Console];
            storageControl = ConsoleBuilder.Accessor("StorageSelector", FieldType.Extract);
            folderControl = ConsoleBuilder.Accessor("FolderSelector", FieldType.Extract);
            consoleControl.ValueChangeHandler += (s, e) => SetConsoleValueInControl();
            sizeControl = ConsoleBuilder.Accessor("InstallationSize", FieldType.Integer);

            int lastBottom = PrepareControl(consoleControl);
            lastBottom = PrepareControl(storageControl, lastBottom);
            lastBottom = PrepareControl(folderControl, lastBottom);
            PrepareControl(sizeControl, lastBottom, false);
            SetConsoleValueInControl();

            CreateLabel("Console", consoleControl);
            storageLabel = CreateLabel("Storage", storageControl);
            folderLabel = CreateLabel("Folder", folderControl);
            sizeLabel = CreateLabel("Size", sizeControl);
            sizeLabel2 = CreateLabel("Mb", sizeControl, true);

            ((NumericAccessor<ConsoleField, PSConsole>)sizeControl).MaximumValue = 1000000;
        }

        private void SetConsoleValueInControl()
        {
            storageControl.Clear();
            folderControl.Clear();

            if (consoleControl.IsEmpty || 
                (consoleControl.Value is not PSConsole console))
            {
                storageControl.Value = null;
                storageControl.Enabled = false;
                folderControl.Value = null;
                folderControl.Enabled = false;
                sizeControl.Value = null;
                sizeControl.Enabled = false;
            }
            else
            {
                SimpleFilter<ConsoleField, PSConsole> filter = 
                    new(ConsoleField.Id, FilterOperation.Equals, console.Id);

                storageControl.Enabled = true;
                IFilteredInitializer<ConsoleField, PSConsole>? storageInitializer =
                    (IFilteredInitializer<ConsoleField, PSConsole>?)storageControl.Context.Initializer;

                if (storageInitializer != null)
                    storageInitializer.Filter = filter;

                storageControl.RenewControl(true);
                    
                IFilteredInitializer<ConsoleField, PSConsole>? folderInitializer =
                    (IFilteredInitializer<ConsoleField, PSConsole>?)folderControl.Context.Initializer;
                folderControl.Enabled = true;

                if (folderInitializer != null)
                    folderInitializer.Filter = filter;

                folderControl.RenewControl(true);
                sizeControl.Enabled = true;
            }
        }

        protected override void FillControls(Installation item)
        {
            PSConsole? console = DataManager.Item<ConsoleField, PSConsole>(ConsoleField.Id, item.ConsoleId);
            consoleControl.Value = console;

            if (console != null)
            {
                storageControl.Value = console.Storages.GetById(item.StorageId);
                folderControl.Value = console.Folders.GetByName(item.Folder);
                sizeControl.Value = item.Size;
            }
        }

        protected override void GrabControls(Installation item)
        {
            PSConsole? console = consoleControl?.DAOValue<PSConsole>();

            if (console == null)
                return;

            item.ConsoleId = console.Id;
            Storage? storage = storageControl.DAOValue<Storage>();

            if (storage != null)
            {
                item.StorageId = storage.Id;
                Folder? folder = folderControl.DAOValue<Folder>();

                item.Folder = folder == null || folderControl.IsEmpty
                    ? string.Empty
                    : folder.Name;
            }

            item.Size = sizeControl.IntValue;
        }

        private int PrepareControl(IControlAccessor accessor, int lastBottom = -1, bool fullRow = true)
        {
            accessor.Parent = this;
            accessor.Left = 80;
            accessor.Top = lastBottom == -1 ? 8 : lastBottom + 4;
            accessor.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            if (fullRow)
            {
                accessor.Anchor |= AnchorStyles.Right;
                accessor.Width = MainPanel.ContentContainer.Width - accessor.Left - 8;
            }
            else accessor.Width = 120;

            accessor.Height = 24;
            return accessor.Bottom;
        }

        protected override string EmptyMandatoryField() =>
            consoleControl != null && consoleControl.IsEmpty
                ? "Console"
                : storageControl != null && storageControl.IsEmpty
                    ? "Storage"
                    : base.EmptyMandatoryField();

        protected override int ContentHeight => sizeControl.Bottom + 8;

        public override void RenewData()
        {
            RenewConsoleControl();
            SetStorageAndFolderVisible();
        }

        private void SetStorageAndFolderVisible()
        {
            bool storageAvailable = false;
            bool folderAvailable = false;
            ConsoleGenerationHelper generationHelper = TypeHelper.Helper<ConsoleGenerationHelper>();
            ConsoleInitializer? consoleInitializer  =
                (ConsoleInitializer?)consoleControl?.Context.Initializer;

            if (consoleInitializer != null)
                foreach (object console in consoleInitializer.AvailableItems)
                    if (console is PSConsole psConsole)
                    {
                        storageAvailable |= generationHelper.StorageSupport(psConsole.Generation);
                        folderAvailable |= generationHelper.FolderSupport(psConsole.Generation);
                    }

            storageControl.Visible = storageAvailable;
            storageLabel.Visible = storageAvailable;
            folderControl.Visible = folderAvailable;
            folderLabel.Visible = folderAvailable;

            int lastBottom = consoleControl!.Bottom;

            if (folderControl.Visible)
                lastBottom = folderControl.Bottom;
            else
            if (storageControl.Visible)
                lastBottom = storageControl.Bottom;

            PrepareControl(sizeControl, lastBottom, false);
            OxControlHelper.AlignByBaseLine(sizeControl.Control, sizeLabel);
            OxControlHelper.AlignByBaseLine(sizeControl.Control, sizeLabel2);
        }

        private void RenewConsoleControl()
        {
            ConsoleInitializer? consoleInitializer =
                (ConsoleInitializer?)consoleControl.Context.Initializer;

            if (consoleInitializer != null)
            {
                consoleInitializer.Filter = Game.AvailableConsoleFilter(Context.Builder);

                if (ExistingItems != null)
                {
                    ListDAO<PSConsole> consolesList = new();

                    foreach (object existingItem in ExistingItems)
                        if (existingItem is Installation installation)
                        {
                            PSConsole? console = DataManager.Item<ConsoleField, PSConsole>(ConsoleField.Id, installation.ConsoleId);

                            if (console != null)
                                consolesList.Add(console);
                        }
                    
                    consoleInitializer.ExistingConsoles = consolesList;
                }
            }

            consoleControl.RenewControl(true);
        }
    }
}
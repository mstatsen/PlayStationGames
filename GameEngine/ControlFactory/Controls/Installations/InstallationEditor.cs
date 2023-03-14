using OxLibrary.Controls;
using OxXMLEngine.ControlFactory;
using OxXMLEngine.ControlFactory.Accessors;
using OxXMLEngine.ControlFactory.Controls;
using OxXMLEngine.ControlFactory.Initializers;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Filter;
using OxXMLEngine.Data.Types;
using OxXMLEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class InstallationEditor : ListItemEditor<Installation, GameField, Game>
    {
        protected override string Title => "Installation";

        private ExtractAccessor<ConsoleField, PSConsole> consoleControl = default!;
        private IControlAccessor storageControl = default!;
        private IControlAccessor folderControl = default!;
        private OxLabel storageLabel = default!;
        private OxLabel folderLabel = default!;

        private static ControlBuilder<ConsoleField, PSConsole> ConsoleBuilder =>
            DataManager.Builder<ConsoleField, PSConsole>(ControlScope.Editor);

        protected override void CreateControls()
        {
            consoleControl = (ExtractAccessor<ConsoleField, PSConsole>)ConsoleBuilder[ConsoleField.Console];
            storageControl = ConsoleBuilder.Accessor("StorageSelector", FieldType.Extract);
            folderControl = ConsoleBuilder.Accessor("FolderSelector", FieldType.Extract);
            consoleControl.ValueChangeHandler += OnConsoleChange;

            int lastBottom = PrepareControl(consoleControl);
            lastBottom = PrepareControl(storageControl, lastBottom);
            PrepareControl(folderControl, lastBottom);
            SetConsoleValueInControl();

            CreateLabel("Console", consoleControl);
            storageLabel = CreateLabel("Storage", storageControl);
            folderLabel = CreateLabel("Folder", folderControl);
        }

        private void OnConsoleChange(object? sender, EventArgs e) =>
            SetConsoleValueInControl();

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
            }
        }

        protected override void GrabControls(Installation item)
        {
            PSConsole? console = consoleControl?.DAOValue<PSConsole>();

            if (console != null)
            {
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
            }
        }

        private int PrepareControl(IControlAccessor accessor, int lastBottom = -1)
        {
            accessor.Parent = this;
            accessor.Left = 80;
            accessor.Top = lastBottom == -1 ? 8 : lastBottom + 4;
            accessor.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            accessor.Width = MainPanel.ContentContainer.Width - accessor.Left - 8;
            accessor.Height = 24;
            return accessor.Bottom;
        }

        protected override string EmptyMandatoryField() =>
            consoleControl != null && consoleControl.IsEmpty
                ? "Console"
                : storageControl != null && storageControl.IsEmpty
                    ? "Storage"
                    : base.EmptyMandatoryField();

        protected override int ContentHeight =>
            (folderControl != null && folderControl.Visible
            ? folderControl.Bottom
                : storageControl != null && storageControl.Visible
                    ? storageControl.Bottom
                    : consoleControl != null 
                        ? consoleControl.Bottom
                        : 26)
            + 8;

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
            ExtractInitializer<ConsoleField, PSConsole>? consoleInitializer  =
                (ExtractInitializer<ConsoleField, PSConsole>?)consoleControl?.Context.Initializer;

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
        }

        private void RenewConsoleControl()
        {
            IFilteredInitializer<ConsoleField, PSConsole>? consoleInitializer =
                (IFilteredInitializer<ConsoleField, PSConsole>?)consoleControl.Context.Initializer;

            if (consoleInitializer != null)
                consoleInitializer.Filter = Game.AvailableConsoleFilter(Context.Builder);

            consoleControl.RenewControl(true);
        }
    }
}
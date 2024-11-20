using OxLibrary;
using OxLibrary.Controls;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.ControlFactory.Initializers;
using OxDAOEngine.Data.Filter.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public partial class InstallationEditor : CustomItemEditor<Installation, GameField, Game>
    {
        private ExtractAccessor<ConsoleField, PSConsole> consoleControl = default!;
        private IControlAccessor storageControl = default!;
        private IControlAccessor folderControl = default!;
        private IControlAccessor sizeControl = default!;
        private OxLabel storageLabel = default!;
        private OxLabel folderLabel = default!;
        private OxLabel sizeLabel = default!;
        private OxLabel sizeLabel2 = default!;
        public override Bitmap FormIcon => OxIcons.Installation;

        private static ControlBuilder<ConsoleField, PSConsole> ConsoleBuilder =>
            DataManager.Builder<ConsoleField, PSConsole>(ControlScope.Editor);

        protected override void CreateControls()
        {
            consoleControl = (ExtractAccessor<ConsoleField, PSConsole>)ConsoleBuilder[ConsoleField.Console];
            storageControl = ConsoleBuilder.Accessor("Installation:Storage", FieldType.Extract);
            folderControl = ConsoleBuilder.Accessor("Installation:Folder", FieldType.Extract);
            consoleControl.ValueChangeHandler += (s, e) => SetConsoleValueInControl();
            sizeControl = ConsoleBuilder.Accessor("Installation:Size", FieldType.Integer);
            PrepareControlColors();

            int lastBottom = PrepareControl(consoleControl);
            lastBottom = PrepareControl(storageControl, lastBottom, 110);
            lastBottom = PrepareControl(folderControl, lastBottom, 110);
            PrepareControl(sizeControl, lastBottom, 110, false);
            CreateLabels();
            sizeControl.MaximumValue = 1000000;
            SetConsoleValueInControl();
            SetKeyUpHandler(consoleControl.Control);
            SetKeyUpHandler(storageControl.Control);
            SetKeyUpHandler(folderControl.Control);
            SetKeyUpHandler(sizeControl.Control);

            FirstFocusControl = consoleControl.Control;
        }

        private void CreateLabels()
        {
            CreateLabel("Console", consoleControl);
            storageLabel = CreateLabel("Storage", storageControl);
            folderLabel = CreateLabel("Folder", folderControl);
            sizeLabel = CreateLabel("Size", sizeControl);
            sizeLabel2 = CreateLabel("Mb", sizeControl, true);

            storageLabel.Left = 38;
            folderLabel.Left = 38;
            sizeLabel.Left = 38;
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
                storageLabel.Enabled = false;
                folderControl.Value = null;
                folderControl.Enabled = false;
                folderLabel.Enabled = false;
                sizeControl.Value = null;
                sizeControl.Enabled = false;
                sizeLabel.Enabled = false;
            }
            else
            {
                Filter<ConsoleField, PSConsole> filter = new(FilterConcat.AND);
                filter.AddFilter(ConsoleField.Id, FilterOperation.Equals, console.Id);

                storageControl.Enabled = true;
                storageLabel.Enabled = true;
                IFilteredInitializer<ConsoleField, PSConsole>? storageInitializer =
                    (IFilteredInitializer<ConsoleField, PSConsole>?)storageControl.Context.Initializer;

                if (storageInitializer is not null)
                    storageInitializer.Filter = filter;

                storageControl.RenewControl(true);
                storageControl.Control.BackColor = consoleControl.Control.BackColor;

                IFilteredInitializer<ConsoleField, PSConsole>? folderInitializer =
                    (IFilteredInitializer<ConsoleField, PSConsole>?)folderControl.Context.Initializer;
                folderControl.Enabled = true;
                folderLabel.Enabled = true;

                if (folderInitializer is not null)
                    folderInitializer.Filter = filter;

                folderControl.RenewControl(true);
                folderControl.Control.BackColor = consoleControl.Control.BackColor;
                sizeControl.Enabled = true;
                sizeLabel.Enabled = true;
            }
        }

        protected override void FillControls(Installation item)
        {
            PSConsole? console = DataManager.Item<ConsoleField, PSConsole>(ConsoleField.Id, item.ConsoleId);
            consoleControl.Value = console;

            if (console is not null)
            {
                storageControl.Value = console.Storages.GetById(item.StorageId);
                folderControl.Value = console.Folders.GetByName(item.Folder);
                sizeControl.Value = item.Size;
            }
        }

        protected override void GrabControls(Installation item)
        {
            PSConsole? console = consoleControl?.DAOValue<PSConsole>();

            if (console is null)
                return;

            item.ConsoleId = console.Id;
            Storage? storage = storageControl.DAOValue<Storage>();

            if (storage is not null)
            {
                item.StorageId = storage.Id;
                Folder? folder = folderControl.DAOValue<Folder>();

                item.Folder = folder is null 
                    || folderControl.IsEmpty
                    ? string.Empty
                    : folder.Name;
            }

            item.Size = sizeControl.IntValue;
        }

        private int PrepareControl(IControlAccessor accessor, int lastBottom = -1, int left = 80, bool fullRow = true)
        {
            accessor.Parent = this;
            accessor.Left = left;
            accessor.Top = lastBottom is -1 ? 8 : lastBottom + 4;
            accessor.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            if (fullRow)
            {
                accessor.Anchor |= AnchorStyles.Right;
                accessor.Width = MainPanel.WidthInt - accessor.Left - 8;
            }
            else accessor.Width = 120;

            accessor.Height = 24;
            return accessor.Bottom;
        }

        protected override string EmptyMandatoryField() =>
            consoleControl is not null 
            && consoleControl.IsEmpty
                ? "Console"
                : storageControl is not null 
                    && storageControl.IsEmpty
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

            if (consoleInitializer is not null)
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

            int lastBottom = 
                folderControl.Visible 
                    ? folderControl.Bottom
                    : storageControl.Visible
                        ? storageControl.Bottom
                        : consoleControl!.Bottom;

            PrepareControl(sizeControl, lastBottom, 110, false);
            OxControlHelper.AlignByBaseLine(sizeControl.Control, sizeLabel);
            OxControlHelper.AlignByBaseLine(sizeControl.Control, sizeLabel2);
        }

        private void RenewConsoleControl()
        {
            ConsoleInitializer? consoleInitializer =
                (ConsoleInitializer?)consoleControl.Context.Initializer;

            if (consoleInitializer is not null)
            {
                consoleInitializer.Filter = Game.AvailableConsoleFilter(Context.Builder);

                if (ExistingItems is not null)
                {
                    ListDAO<PSConsole> consolesList = new();

                    foreach (object existingItem in ExistingItems)
                        if (existingItem is Installation installation)
                        {
                            PSConsole? console = DataManager.Item<ConsoleField, PSConsole>(ConsoleField.Id, installation.ConsoleId);

                            if (console is not null)
                                consolesList.Add(console);
                        }
                    
                    consoleInitializer.ExistingConsoles = consolesList;
                }
            }

            consoleControl.RenewControl(true);
        }
    }
}
using OxLibrary;
using OxLibrary.Controls;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Editor;
using PlayStationGames.ConsoleEngine.ControlFactory.Controls;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Editor
{
    public class ConsoleWorker : DAOWorker<ConsoleField, PSConsole, ConsoleFieldGroup>
    {
        public ConsoleWorker() : base() { }

        protected override void PrepareStyles()
        {
            base.PrepareStyles();
            ControlPainter.ColorizeControl(installationsButton, Editor.MainPanel.BaseColor);
            installationsWorker.BaseColor = Editor.MainPanel.BaseColor;
        }

        protected override bool SyncFieldValues(ConsoleField field, bool byUser)
        {
            if (field == ConsoleField.Firmware)
            {
                if (Firmware.Equals(FirmwareType.Official))
                {
                    Builder.SetVisible(ConsoleField.FirmwareName, false);
                    Builder[ConsoleField.FirmwareVersion].Top = Builder[ConsoleField.FirmwareName].Top;
                }
                else
                {
                    Builder.SetVisible(ConsoleField.FirmwareName, true);
                    Builder[ConsoleField.FirmwareVersion].Top = Builder[ConsoleField.FirmwareName].Bottom
                        + Generator.Offset(ConsoleField.FirmwareVersion) + 2;
                }
            }

            return 
                field is ConsoleField.Generation or
                ConsoleField.Firmware;
        }

        private readonly ConsoleGenerationHelper generationHelper = TypeHelper.Helper<ConsoleGenerationHelper>();

        protected override bool SetGroupsAvailability(bool afterSyncValues = false)
        {
            Editor.Groups[ConsoleFieldGroup.Folders].Visible = generationHelper.FolderSupport(Generation);
            Editor.Groups[ConsoleFieldGroup.Storages].Visible = generationHelper.StorageSupport(Generation);
            Editor.Groups[ConsoleFieldGroup.Accounts].Visible = generationHelper.MaxAccountsCount(Generation, Firmware) > 0;
            Editor.Groups[ConsoleFieldGroup.Games].Visible = generationHelper.StorageSupport(Generation);
            return true;
        }

        protected override EditorLayoutsGenerator<ConsoleField, PSConsole, ConsoleFieldGroup> 
            CreateLayoutsGenerator(FieldGroupFrames<ConsoleField, ConsoleFieldGroup> frames, 
                ControlLayouter<ConsoleField, PSConsole> layouter) =>
                new ConsoleEditorLayoutsGenerator(frames, layouter);

        private readonly OxButton installationsButton = new("Install / uninstall", null)
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
            Font = new Font(Styles.FontFamily, Styles.DefaultFontSize)
        };

        private readonly OxLabel installedGamesLabel = new()
        {
            Text = "Installed games count",
            Font = new Font(Styles.FontFamily, Styles.DefaultFontSize)
        };

        protected override void AfterLayoutControls()
        {
            base.AfterLayoutControls();

            Editor.Groups[ConsoleFieldGroup.Games].SizeChanged -= GamesGroupSizeChangedHandler;
            Editor.Groups[ConsoleFieldGroup.Games].SizeChanged += GamesGroupSizeChangedHandler;
            Control? firmwareVersionControl = Layouter.PlacedControl(ConsoleField.FirmwareVersion)?.Control;
            installedGamesLabel.Parent = Editor.Groups[ConsoleFieldGroup.Games];
            installedGamesLabel.Left = 8;
            installationsButton.Parent = Editor.Groups[ConsoleFieldGroup.Games];
            installationsButton.SetContentSize(installationsButton.Parent.Width / 3, 38);
            installationsButton.Dock = DockStyle.Right;
            installationsButton.Click -= InstallationsClickHandler;
            installationsButton.Click += InstallationsClickHandler;
            installationsWorker.Renew(Item);
            RenewInstalledGamesLabel();
            Builder.Control<AccessoriesControl>(ConsoleField.Accessories).ParentItem = Item;
        }

        private void RenewInstalledGamesLabel() => 
            installedGamesLabel.Text = $"Installed games count: {installationsWorker.InstalledGamesCount}";

        private void GamesGroupSizeChangedHandler(object? sender, EventArgs e)
        {
            installationsButton.SetContentSize(installationsButton.Parent.Width / 3, 38);
            installedGamesLabel.Top = (installationsButton.Parent.Height - installedGamesLabel.Height) / 2;
        }

        private void InstallationsClickHandler(object? sender, EventArgs e)
        {
            if (Item == null)
                return;

            installationsWorker.Show(Editor);
            RenewInstalledGamesLabel();
        }

        private readonly InstallationsWorker installationsWorker = new();

        protected override List<List<ConsoleField>> LabelGroups =>
            new()
            {
                new List<ConsoleField> {
                    ConsoleField.Name,
                    ConsoleField.Generation,
                    ConsoleField.Firmware
                },
                new List<ConsoleField> {
                    ConsoleField.Model,
                    ConsoleField.ModelCode,
                    ConsoleField.FirmwareName,
                    ConsoleField.FirmwareVersion
                }
            };

        public ConsoleGeneration Generation =>
            Builder.Value<ConsoleGeneration>(ConsoleField.Generation);

        public FirmwareType Firmware =>
            Builder.Value<FirmwareType>(ConsoleField.Firmware);

        protected override void BeforeGrabControls()
        {
            base.BeforeGrabControls();

            if (!generationHelper.FolderSupport(Generation))
                Builder[ConsoleField.Folders].Clear();

            if (!generationHelper.StorageSupport(Generation))
                Builder[ConsoleField.Storages].Clear();
        }

        protected override void AfterGrabControls()
        {
            base.AfterGrabControls();
            installationsWorker.Save();
        }
    }
}
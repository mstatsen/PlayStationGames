using OxLibrary;
using OxLibrary.Controls;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Editor;
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

        protected override bool SyncFieldValue(ConsoleField field, bool byUser)
        {
            if (field is ConsoleField.Firmware)
            {
                if (Firmware is FirmwareType.Official)
                {
                    Builder.SetVisible(ConsoleField.FirmwareName, false);
                    Builder[ConsoleField.FirmwareVersion].Top = Builder[ConsoleField.FirmwareName].Top;
                }
                else
                {
                    Builder.SetVisible(ConsoleField.FirmwareName, true);
                    Builder[ConsoleField.FirmwareVersion].Top = 
                        Builder[ConsoleField.FirmwareName].Bottom
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

        private readonly OxButton installationsButton = new("Install / uninstall", OxIcons.Install)
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
            Font = Styles.DefaultFont
        };

        private readonly OxLabel installedGamesLabel = new()
        {
            Text = "Installed games count",
            Font = Styles.DefaultFont
        };

        protected override void AfterLayoutControls()
        {
            base.AfterLayoutControls();

            Editor.Groups[ConsoleFieldGroup.Games].SizeChanged -= GamesGroupSizeChangedHandler;
            Editor.Groups[ConsoleFieldGroup.Games].SizeChanged += GamesGroupSizeChangedHandler;
            Control firmwareVersionControl = Builder.Control<OxTextBox>(ConsoleField.FirmwareVersion);
            installedGamesLabel.Parent = Editor.Groups[ConsoleFieldGroup.Games];
            installedGamesLabel.Left = 8;
            installationsButton.Parent = Editor.Groups[ConsoleFieldGroup.Games];
            installationsButton.Size = new(
                OxWh.Div(installationsButton.Parent.Width, OxWh.W3), 
                OxWh.W38);
            installationsButton.Dock = OxDock.Right;
            installationsButton.Click -= InstallationsClickHandler;
            installationsButton.Click += InstallationsClickHandler;
            installationsWorker.Renew(Item);
            RenewInstalledGamesLabel();
        }

        private void RenewInstalledGamesLabel() => 
            installedGamesLabel.Text =
                installationsWorker.InstalledGamesCount > 0
                    ? $"{installationsWorker.InstalledGamesCount} installed game{(installationsWorker.InstalledGamesCount > 1 ? "s" : string.Empty)}"
                    : "No installed games";

        private void GamesGroupSizeChangedHandler(object? sender, EventArgs e)
        {
            installationsButton.Size = new(
                OxWh.Div(installationsButton.Parent!.Width, OxWh.W3), 
                OxWh.W38
            );
            installedGamesLabel.Top = (installationsButton.Parent.Height - installedGamesLabel.Height) / 2;
        }

        private void InstallationsClickHandler(object? sender, EventArgs e)
        {
            if (Item is null)
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
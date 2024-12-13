using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Handlers;
using OxLibrary.Geometry;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Editor;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Editor;

public class ConsoleWorker : DAOWorker<ConsoleField, PSConsole, ConsoleFieldGroup>
{
    public ConsoleWorker() : base() { }

    protected override void PrepareStyles()
    {
        base.PrepareStyles();
        ControlPainter.ColorizeControl(installationsButton, Editor.BaseColor);
        installationsWorker.BaseColor = Editor.BaseColor;
    }

    protected override bool SyncFieldValue(ConsoleField field, bool byUser)
    {
        if (field is ConsoleField.Firmware)
        {
            Builder.SetVisible(ConsoleField.FirmwareName, Firmware is FirmwareType.Official);
            Builder[ConsoleField.FirmwareVersion].Top =
                OxSh.Short(
                    Firmware is FirmwareType.Official
                        ? Builder[ConsoleField.FirmwareName].Top
                        : Builder[ConsoleField.FirmwareName].Bottom
                            + Generator.Offset(ConsoleField.FirmwareVersion)
                            + 2
                );
        }

        return 
            field is ConsoleField.Generation or
            ConsoleField.Firmware;
    }

    private readonly ConsoleGenerationHelper generationHelper = TypeHelper.Helper<ConsoleGenerationHelper>();

    protected override bool SetGroupsAvailability(bool afterSyncValues = false)
    {
        Editor.Groups[ConsoleFieldGroup.Folders].SetVisible(generationHelper.FolderSupport(Generation));
        Editor.Groups[ConsoleFieldGroup.Storages].SetVisible(generationHelper.StorageSupport(Generation));
        Editor.Groups[ConsoleFieldGroup.Accounts].SetVisible(generationHelper.MaxAccountsCount(Generation, Firmware) > 0);
        Editor.Groups[ConsoleFieldGroup.Games].SetVisible(generationHelper.StorageSupport(Generation));
        return true;
    }

    protected override EditorLayoutsGenerator<ConsoleField, PSConsole, ConsoleFieldGroup> 
        CreateLayoutsGenerator(FieldGroupPanels<ConsoleField, ConsoleFieldGroup> frames, 
            ControlLayouter<ConsoleField, PSConsole> layouter) =>
            new ConsoleEditorLayoutsGenerator(frames, layouter);

    private readonly OxButton installationsButton = new("Install / uninstall", OxIcons.Install)
    {
        Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
        Font = OxStyles.DefaultFont
    };

    private readonly OxLabel installedGamesLabel = new()
    {
        Text = "Installed games count",
        Font = OxStyles.DefaultFont
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
            OxSh.Third(installationsButton.Parent.Width),
            38);
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

    private void GamesGroupSizeChangedHandler(object sender, OxSizeChangedEventArgs args)
    {
        installationsButton.Size = new(
            OxSh.Third(installationsButton.Parent!.Width), 
            38
        );
        installedGamesLabel.Top = OxSh.CenterOffset(installationsButton.Parent.Height, installedGamesLabel.Height);
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
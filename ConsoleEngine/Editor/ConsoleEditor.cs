using OxLibrary;
using OxLibrary.Panels;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine.Editor;
using OxDAOEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Editor
{
    public partial class ConsoleEditor : DAOEditor<ConsoleField, PSConsole, ConsoleFieldGroup>
    {
        public ConsoleEditor() : base() { }

        public override Bitmap? FormIcon => OxIcons.Console;

        public OxPane PanelLeft = new();
        public OxPane PanelRight = new();

        protected override void PreparePanels()
        {
            PrepareParentPanel(PanelRight, MainPanel, OxDock.Fill);
            PanelRight.Width = OxWh.W450;
            PrepareParentPanel(PanelLeft, MainPanel);
        }

        protected override OxPane? GroupParent(ConsoleFieldGroup group) => 
            group switch
            {
                ConsoleFieldGroup.Base or
                ConsoleFieldGroup.GenerationAndModel or
                ConsoleFieldGroup.Firmware or
                ConsoleFieldGroup.Accounts or
                ConsoleFieldGroup.Storages =>
                    PanelLeft,
                ConsoleFieldGroup.Folders or
                ConsoleFieldGroup.Games or
                ConsoleFieldGroup.Accessories =>
                    PanelRight,
                _ => null,
            };

        private readonly ConsoleGenerationHelper generationHelper = TypeHelper.Helper<ConsoleGenerationHelper>();

        protected override void RecalcPanels()
        {
            if (CreatingProcess)
                return;
            
            MinimumSize = Size.Empty;
            MaximumSize = Size.Empty;
            PanelLeft.Width = CalcedWidth(PanelLeft);
            ConsoleGeneration generation = ((ConsoleWorker)Worker).Generation;
            FirmwareType firmware = ((ConsoleWorker)Worker).Firmware;

            if (generationHelper.FolderSupport(generation))
            {
                Groups[ConsoleFieldGroup.Games].Dock = OxDock.Bottom;
                Groups[ConsoleFieldGroup.Accessories].Dock = OxDock.Bottom;
            }
            else
            {
                Groups[ConsoleFieldGroup.Accessories].Dock = OxDock.Fill;
                Groups[ConsoleFieldGroup.Games].Dock = OxDock.Top;
            }

            SetFrameMargin(ConsoleFieldGroup.Accessories, Groups[ConsoleFieldGroup.Accessories]);
            SetFrameMargin(ConsoleFieldGroup.Games, Groups[ConsoleFieldGroup.Games]);
            MainPanel.Size = new(
                PanelLeft.Width 
                    | TypeHelper.Helper<ConsoleFieldGroupHelper>().
                            GroupWidth(ConsoleFieldGroup.Folders),
                    (generationHelper.StorageSupport(generation)
                        ? Groups[ConsoleFieldGroup.Storages].Bottom
                        : generationHelper.MaxAccountsCount(generation, firmware) > 0 
                            && !generationHelper.FolderSupport(generation)
                                ? Groups[ConsoleFieldGroup.Accounts].Bottom
                                : Groups[ConsoleFieldGroup.Firmware].Bottom | OxWh.W140) 
                        | OxWh.W13
            );
        }

        protected override void SetPaddings()
        {
            base.SetPaddings();
            Groups[ConsoleFieldGroup.Games].Padding.Size = OxWh.W4;
            Groups[ConsoleFieldGroup.Accounts].Padding.Right = OxWh.W2;
            Groups[ConsoleFieldGroup.Storages].Padding.Right = OxWh.W2;
            Groups[ConsoleFieldGroup.Folders].Padding.Right = OxWh.W2;
            Groups[ConsoleFieldGroup.Accessories].Padding.Right = OxWh.W2;
        }

        protected override void SetFrameMargin(ConsoleFieldGroup group, OxFrame frame)
        {
            base.SetFrameMargin(group, frame);
            frame.Margin.Size = OxWh.W8;
            frame.Margin.Left =
                group is ConsoleFieldGroup.Folders or
                    ConsoleFieldGroup.Games or
                    ConsoleFieldGroup.Accessories
                        ? OxWh.W0
                        : OxWh.W8;

            frame.Margin.Top = group switch
            {
                ConsoleFieldGroup.GenerationAndModel or
                ConsoleFieldGroup.Firmware or
                ConsoleFieldGroup.Accounts or
                ConsoleFieldGroup.Storages =>
                    OxWh.W0,
                ConsoleFieldGroup.Games when Groups[ConsoleFieldGroup.Games].Dock is OxDock.Bottom =>
                    OxWh.W0,
                ConsoleFieldGroup.Accessories when Groups[ConsoleFieldGroup.Games].Dock is OxDock.Bottom
                    || generationHelper.StorageSupport(((ConsoleWorker)Worker).Generation) =>
                    OxWh.W0,
                _ => OxWh.W8
            };
        }
    }
}
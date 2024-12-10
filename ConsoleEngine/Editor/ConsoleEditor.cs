using OxLibrary;
using OxLibrary.Panels;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine.Editor;
using OxDAOEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data.Types;
using OxLibrary.Geometry;

namespace PlayStationGames.ConsoleEngine.Editor
{
    public partial class ConsoleEditor : DAOEditor<ConsoleField, PSConsole, ConsoleFieldGroup>
    {
        public ConsoleEditor() : base() { }

        public override Bitmap? FormIcon => OxIcons.Console;

        public OxPanel PanelLeft = new();
        public OxPanel PanelRight = new();

        protected override void PreparePanels()
        {
            PrepareParentPanel(PanelRight, FormPanel, OxDock.Fill);
            PanelRight.Width = 450;
            PrepareParentPanel(PanelLeft, FormPanel);
        }

        protected override OxPanel? GroupParent(ConsoleFieldGroup group) => 
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
            
            MinimumSize = new();
            MaximumSize = new();
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
            FormPanel.Size = new(
                PanelLeft.Width
                + TypeHelper.Helper<ConsoleFieldGroupHelper>().GroupWidth(ConsoleFieldGroup.Folders),
                OxSH.Add(
                    generationHelper.StorageSupport(generation)
                        ? Groups[ConsoleFieldGroup.Storages].Bottom
                        : generationHelper.MaxAccountsCount(generation, firmware) > 0
                            && !generationHelper.FolderSupport(generation)
                                ? Groups[ConsoleFieldGroup.Accounts].Bottom
                                : Groups[ConsoleFieldGroup.Firmware].Bottom + 140,
                    13
                )
            );
        }

        protected override void SetPaddings()
        {
            base.SetPaddings();
            Groups[ConsoleFieldGroup.Games].Padding.Size = 4;
            Groups[ConsoleFieldGroup.Accounts].Padding.Right = 2;
            Groups[ConsoleFieldGroup.Storages].Padding.Right = 2;
            Groups[ConsoleFieldGroup.Folders].Padding.Right = 2;
            Groups[ConsoleFieldGroup.Accessories].Padding.Right = 2;
        }

        protected override void SetFrameMargin(ConsoleFieldGroup group, OxFrame frame)
        {
            base.SetFrameMargin(group, frame);
            frame.Margin.Size = 8;
            frame.Margin.Left =
                OxSH.Short(
                    group is ConsoleFieldGroup.Folders
                          or ConsoleFieldGroup.Games
                          or ConsoleFieldGroup.Accessories
                        ? 0
                        : 8
                );
            frame.Margin.Top = group switch
            {
                ConsoleFieldGroup.GenerationAndModel or
                ConsoleFieldGroup.Firmware or
                ConsoleFieldGroup.Accounts or
                ConsoleFieldGroup.Storages =>
                    0,
                ConsoleFieldGroup.Games when Groups[ConsoleFieldGroup.Games].Dock is OxDock.Bottom =>
                    0,
                ConsoleFieldGroup.Accessories when Groups[ConsoleFieldGroup.Games].Dock is OxDock.Bottom
                    || generationHelper.StorageSupport(((ConsoleWorker)Worker).Generation) =>
                    0,
                _ => 8
            };
        }
    }
}
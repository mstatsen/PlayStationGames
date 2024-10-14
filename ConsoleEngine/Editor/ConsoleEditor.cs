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

        public OxPanel PanelLeft = new();
        public OxPanel PanelRight = new();

        protected override void PreparePanels()
        {
            PrepareParentPanel(PanelRight, MainPanel, DockStyle.Fill);
            PanelRight.Width = 450;
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
            MinimumSize = new Size(0, 0);
            MaximumSize = new Size(0, 0);
            PanelLeft.Width = CalcedWidth(PanelLeft);
            ConsoleGeneration generation = ((ConsoleWorker)Worker).Generation;
            FirmwareType firmware = ((ConsoleWorker)Worker).Firmware;

            DockStyle gamesDock = generationHelper.FolderSupport(generation)
                ? DockStyle.Bottom
                : DockStyle.Top;

            DockStyle accessoriesDock = generationHelper.FolderSupport(generation)
                ? DockStyle.Bottom
                : DockStyle.Fill;

            Groups[ConsoleFieldGroup.Accessories].Dock = accessoriesDock;
            Groups[ConsoleFieldGroup.Games].Dock = gamesDock;

            SetFrameMargin(ConsoleFieldGroup.Accessories, Groups[ConsoleFieldGroup.Accessories]);
            SetFrameMargin(ConsoleFieldGroup.Games, Groups[ConsoleFieldGroup.Games]);
            MainPanel.SetContentSize(
                PanelLeft.Width + TypeHelper.Helper<ConsoleFieldGroupHelper>().GroupWidth(ConsoleFieldGroup.Folders),
                (generationHelper.StorageSupport(generation)
                    ? Groups[ConsoleFieldGroup.Storages].Bottom
                    : generationHelper.MaxAccountsCount(generation, firmware) > 0 &&
                        !generationHelper.FolderSupport(generation)
                            ? Groups[ConsoleFieldGroup.Accounts].Bottom
                            : Groups[ConsoleFieldGroup.Firmware].Bottom + 140) + 13
            );
        }

        protected override void SetPaddings()
        {
            base.SetPaddings();
            Groups[ConsoleFieldGroup.Games].Paddings.SetSize(OxSize.Large);
            Groups[ConsoleFieldGroup.Accounts].Paddings.RightOx = OxSize.Medium;
            Groups[ConsoleFieldGroup.Storages].Paddings.RightOx = OxSize.Medium;
            Groups[ConsoleFieldGroup.Folders].Paddings.RightOx = OxSize.Medium;
            Groups[ConsoleFieldGroup.Accessories].Paddings.RightOx = OxSize.Medium;
        }

        protected override void SetFrameMargin(ConsoleFieldGroup group, OxFrame frame)
        {
            base.SetFrameMargin(group, frame);
            frame.Margins.SetSize(OxSize.Extra);
            frame.Margins.LeftOx =
                group is ConsoleFieldGroup.Folders or
                    ConsoleFieldGroup.Games or
                    ConsoleFieldGroup.Accessories
                        ? OxSize.None 
                        : OxSize.Extra;

            frame.Margins.TopOx = group switch
            {
                ConsoleFieldGroup.GenerationAndModel or
                ConsoleFieldGroup.Firmware or
                ConsoleFieldGroup.Accounts or
                ConsoleFieldGroup.Storages =>
                    OxSize.None,
                ConsoleFieldGroup.Games when Groups[ConsoleFieldGroup.Games].Dock == DockStyle.Bottom =>
                    OxSize.None,
                ConsoleFieldGroup.Accessories when Groups[ConsoleFieldGroup.Games].Dock == DockStyle.Bottom
                    || generationHelper.StorageSupport(((ConsoleWorker)Worker).Generation) =>
                    OxSize.None,
                _ => OxSize.Extra,
            };
        }
    }
}
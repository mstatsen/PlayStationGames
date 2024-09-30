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

        public OxPanel PanelLeft = new();
        public OxPanel PanelRight = new();

        protected override void PreparePanels()
        {
            PrepareParentPanel(PanelRight, MainPanel, DockStyle.Fill);
            PanelRight.Width = 450;
            PrepareParentPanel(PanelLeft, MainPanel);
        }

        protected override OxPane? GroupParent(ConsoleFieldGroup group)
        {
            return group switch
            {
                ConsoleFieldGroup.Base or
                ConsoleFieldGroup.Accounts or
                ConsoleFieldGroup.Storages => 
                    PanelLeft,
                ConsoleFieldGroup.Folders or
                ConsoleFieldGroup.Accessories => 
                    PanelRight,
                _ => null,
            };
        }

        protected override void RecalcPanels()
        {
            MinimumSize = new Size(0, 0);
            MaximumSize = new Size(0, 0);
            PanelLeft.Width = CalcedWidth(PanelLeft);
            ConsoleGenerationHelper generationHelper = TypeHelper.Helper<ConsoleGenerationHelper>();
            ConsoleGeneration generation = ((ConsoleWorker)Worker).Generation;
            FirmwareType firmware = ((ConsoleWorker)Worker).Firmware;
            Groups[ConsoleFieldGroup.Accessories].Dock = generationHelper.FolderSupport(generation)
                ? DockStyle.Bottom
                : DockStyle.Fill;
            SetFrameMargin(ConsoleFieldGroup.Accessories, Groups[ConsoleFieldGroup.Accessories]);
            MainPanel.SetContentSize(
                PanelLeft.Width + TypeHelper.Helper<ConsoleFieldGroupHelper>().GroupWidth(ConsoleFieldGroup.Folders),
                (generationHelper.StorageSupport(generation)
                    ? Groups[ConsoleFieldGroup.Storages].Bottom
                    : generationHelper.MaxAccountsCount(generation, firmware) > 0 &&
                        !generationHelper.FolderSupport(generation)
                            ? Groups[ConsoleFieldGroup.Accounts].Bottom
                            : Groups[ConsoleFieldGroup.Base].Height + 140) + 13
            );
        }

        protected override void SetPaddings()
        {
            base.SetPaddings();
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
                ConsoleFieldGroup.Accessories
                    ? OxSize.None 
                    : OxSize.Extra;

            frame.Margins.TopOx = group switch
            {
                ConsoleFieldGroup.Accounts or
                ConsoleFieldGroup.Storages =>
                    OxSize.None,
                ConsoleFieldGroup.Accessories when Groups[ConsoleFieldGroup.Accessories].Dock == DockStyle.Bottom =>
                    OxSize.None,
                _ => OxSize.Extra,
            };
        }
    }
}
using OxLibrary;
using OxLibrary.Panels;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxXMLEngine.Editor;

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
            PanelRight.Width = 300;
            PrepareParentPanel(PanelLeft, MainPanel);
        }

        protected override OxPane? GroupParent(ConsoleFieldGroup group)
        {
            return group switch
            {
                ConsoleFieldGroup.Base or
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
            MainPanel.SetContentSize(
                PanelLeft.Width + (PanelRight.Visible ? 300 : 0),
                Groups[ConsoleFieldGroup.Storages].Visible ? 434 : 200
            );

            if (Groups[ConsoleFieldGroup.Folders].Visible)
                Groups[ConsoleFieldGroup.Accessories].Dock = DockStyle.Bottom;
            else Groups[ConsoleFieldGroup.Accessories].Dock = DockStyle.Fill;

            SetFrameMargin(ConsoleFieldGroup.Accessories, Groups[ConsoleFieldGroup.Accessories]);
        }

        protected override void SetPaddings()
        {
            base.SetPaddings();
            Groups[ConsoleFieldGroup.Storages].Paddings.RightOx = OxSize.Medium;
            Groups[ConsoleFieldGroup.Folders].Paddings.RightOx = OxSize.Medium;
            Groups[ConsoleFieldGroup.Accessories].Paddings.RightOx = OxSize.Medium;
        }

        protected override void SetFrameMargin(ConsoleFieldGroup group, OxFrame frame)
        {
            base.SetFrameMargin(group, frame);
            frame.Margins.SetSize(OxSize.Extra);
            frame.Margins.LeftOx =
                group == ConsoleFieldGroup.Folders || 
                group == ConsoleFieldGroup.Accessories
                    ? OxSize.None 
                    : OxSize.Extra;

            switch (group)
            {
                case ConsoleFieldGroup.Storages:
                case ConsoleFieldGroup.Accessories when Groups[ConsoleFieldGroup.Accessories].Dock == DockStyle.Bottom:
                    frame.Margins.TopOx = OxSize.None;
                    break;
                default:
                    frame.Margins.TopOx = OxSize.Extra;
                    break;
            }
        }
    }
}
using OxLibrary;
using OxLibrary.Panels;
using OxDAOEngine.Editor;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Editor
{
    public partial class GameEditor : DAOEditor<GameField, Game, GameFieldGroup>
    {
        public GameEditor() : base() { }

        public override Bitmap? FormIcon => OxIcons.Game;

        public OxPane PanelTop = new();
        public OxPane PanelLeft = new();
        public OxPane PanelMiddle = new();
        public OxPane PanelRight = new();

        protected override void PreparePanels()
        {
            PrepareParentPanel(PanelTop, MainPanel, OxDock.Top, false);
            PrepareParentPanel(PanelRight, PanelTop);
            PrepareParentPanel(PanelMiddle, PanelTop);
            PrepareParentPanel(PanelLeft, PanelTop);
        }

        protected override OxPane? GroupParent(GameFieldGroup group) => 
            group switch
            {
                GameFieldGroup.Base or 
                GameFieldGroup.RelatedGames or 
                GameFieldGroup.DLC or 
                GameFieldGroup.Emulator => 
                    PanelLeft,
                GameFieldGroup.Installations or 
                GameFieldGroup.Genre or
                GameFieldGroup.Tags => 
                    PanelMiddle,
                GameFieldGroup.Links or
                GameFieldGroup.Trophyset =>
                    PanelRight,
                GameFieldGroup.ReleaseBase => 
                    MainPanel,
                _ => null,
            };

        protected override void SetFrameMargin(GameFieldGroup group, OxFrame frame)
        {
            base.SetFrameMargin(group, frame);

            if (GroupParents[PanelRight].Contains(frame))
                frame.Margin.Right = OxWh.W8;

            if (group is GameFieldGroup.ReleaseBase)
            {
                frame.Margin.Right = OxWh.W8;
                frame.Margin.Bottom = OxWh.W8;
            }
        }

        protected override void SetPaddings()
        {
            Groups[GameFieldGroup.ReleaseBase].Padding.Horizontal = OxWh.W4;
            Groups[GameFieldGroup.DLC].Padding.Right = OxWh.W2;
            Groups[GameFieldGroup.RelatedGames].Padding.Right = OxWh.W2;
            Groups[GameFieldGroup.Links].Padding.Right = OxWh.W2;
            Groups[GameFieldGroup.Installations].Padding.Right = OxWh.W2;
            Groups[GameFieldGroup.Trophyset].Padding.Right = OxWh.W2;
            Groups[GameFieldGroup.Trophyset].Padding.Right = OxWh.W2;
        }

        protected override void RecalcPanels()
        {
            MinimumSize = new();
            MaximumSize = new();
            PanelTop.Height = 
                OxWh.Max(
                    OxWh.Max(
                        CalcedHeight(PanelLeft),
                        CalcedHeight(PanelMiddle)
                    ),
                    CalcedHeight(PanelRight)
                );

            PanelLeft.Width = CalcedWidth(PanelLeft);
            PanelMiddle.Width = CalcedWidth(PanelMiddle);
            PanelRight.Width = CalcedWidth(PanelRight);
            MainPanel.Size = new(
                PanelRight.Right,
                PanelTop.Height 
                | (Groups[GameFieldGroup.ReleaseBase].Visible
                        ? Groups[GameFieldGroup.ReleaseBase].Height
                        : OxWh.W8
                  ) 
                  | OxWh.W12
            );
        }
    }
}
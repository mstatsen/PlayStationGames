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
            PrepareParentPanel(PanelTop, MainPanel, DockStyle.Top, false);
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
                frame.Margin.Right = OxSize.M;

            if (group is GameFieldGroup.ReleaseBase)
            {
                frame.Margin.Right = OxSize.M;
                frame.Margin.Bottom = OxSize.M;
            }
        }

        protected override void SetPaddings()
        {
            Groups[GameFieldGroup.ReleaseBase].Padding.Horizontal = OxSize.S;
            Groups[GameFieldGroup.DLC].Padding.Right = OxSize.XS;
            Groups[GameFieldGroup.RelatedGames].Padding.Right = OxSize.XS;
            Groups[GameFieldGroup.Links].Padding.Right = OxSize.XS;
            Groups[GameFieldGroup.Installations].Padding.Right = OxSize.XS;
            Groups[GameFieldGroup.Trophyset].Padding.Right = OxSize.XS;
            Groups[GameFieldGroup.Trophyset].Padding.Right = OxSize.XS;
        }

        protected override void RecalcPanels()
        {
            MinimumSize = Size.Empty;
            MaximumSize = Size.Empty;
            PanelTop.Height = Math.Max(
                Math.Max(
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
                PanelTop.Height +
                    (Groups[GameFieldGroup.ReleaseBase].Visible
                        ? Groups[GameFieldGroup.ReleaseBase].CalcedHeight
                        : 8) + 12
            );
        }
    }
}
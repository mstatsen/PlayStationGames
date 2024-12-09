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

        public OxPanel PanelTop = new();
        public OxPanel PanelLeft = new();
        public OxPanel PanelMiddle = new();
        public OxPanel PanelRight = new();

        protected override void PreparePanels()
        {
            PrepareParentPanel(PanelTop, FormPanel, OxDock.Top, false);
            PrepareParentPanel(PanelRight, PanelTop);
            PrepareParentPanel(PanelMiddle, PanelTop);
            PrepareParentPanel(PanelLeft, PanelTop);
        }

        protected override OxPanel? GroupParent(GameFieldGroup group) => 
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
                    FormPanel,
                _ => null,
            };

        protected override void SetFrameMargin(GameFieldGroup group, OxFrame frame)
        {
            base.SetFrameMargin(group, frame);

            if (GroupParents[PanelRight].Contains(frame))
                frame.Margin.Right = 8;

            if (group is GameFieldGroup.ReleaseBase)
            {
                frame.Margin.Right = 8;
                frame.Margin.Bottom = 8;
            }
        }

        protected override void SetPaddings()
        {
            Groups[GameFieldGroup.ReleaseBase].Padding.Horizontal = 4;
            Groups[GameFieldGroup.DLC].Padding.Right = 2;
            Groups[GameFieldGroup.RelatedGames].Padding.Right = 2;
            Groups[GameFieldGroup.Links].Padding.Right = 2;
            Groups[GameFieldGroup.Installations].Padding.Right = 2;
            Groups[GameFieldGroup.Trophyset].Padding.Right = 2;
            Groups[GameFieldGroup.Trophyset].Padding.Right = 2;
        }

        protected override void RecalcPanels()
        {
            MinimumSize = new();
            MaximumSize = new();
            PanelTop.Height = 
                Math.Max(
                    Math.Max(
                        CalcedHeight(PanelLeft),
                        CalcedHeight(PanelMiddle)
                    ),
                    CalcedHeight(PanelRight)
                );

            PanelLeft.Width = CalcedWidth(PanelLeft);
            PanelMiddle.Width = CalcedWidth(PanelMiddle);
            PanelRight.Width = CalcedWidth(PanelRight);
            FormPanel.Size = new(
                PanelRight.Right,
                (short)
                    (PanelTop.Height
                        + (Groups[GameFieldGroup.ReleaseBase].Visible
                            ? Groups[GameFieldGroup.ReleaseBase].Height
                            : 8
                        ) 
                      + 12
                    )
            );
        }
    }
}
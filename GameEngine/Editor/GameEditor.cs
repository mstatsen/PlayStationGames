using OxLibrary;
using OxLibrary.Panels;
using OxDAOEngine.Data;
using OxDAOEngine.Editor;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Editor
{
    public partial class GameEditor : DAOEditor<GameField, Game, GameFieldGroup>
    {
        public GameEditor() : base() { }

        public OxPanel PanelTop = new();
        public OxPanel PanelLeft = new();
        public OxPanel PanelMiddle = new();
        public OxPanel PanelRight = new();

        protected override void PreparePanels()
        {
            PrepareParentPanel(PanelTop, MainPanel, DockStyle.Top, false);
            PrepareParentPanel(PanelRight, PanelTop);
            PrepareParentPanel(PanelMiddle, PanelTop);
            PrepareParentPanel(PanelLeft, PanelTop);
        }

        protected override void SetGroupCaptions()
        {
            base.SetGroupCaptions();
            Account? defaultAccount = DataManager.ListController<AccountField, Account>().FullItemsList.Find((a) => a.DefaultAccount);
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
                GameFieldGroup.Link or
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
                frame.Margins.RightOx = OxSize.Extra;

            if (group == GameFieldGroup.ReleaseBase)
            {
                frame.Margins.RightOx = OxSize.Extra;
                frame.Margins.BottomOx = OxSize.Extra;
            }
        }

        protected override void SetPaddings()
        {
            Groups[GameFieldGroup.ReleaseBase].Paddings.HorizontalOx = OxSize.Large;
            Groups[GameFieldGroup.DLC].Paddings.RightOx = OxSize.Medium;
            Groups[GameFieldGroup.RelatedGames].Paddings.RightOx = OxSize.Medium;
            Groups[GameFieldGroup.Link].Paddings.RightOx = OxSize.Medium;
            Groups[GameFieldGroup.Installations].Paddings.RightOx = OxSize.Medium;
        }

        protected override void RecalcPanels()
        {
            MinimumSize = new Size(0, 0);
            MaximumSize = new Size(0, 0);
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

            MainPanel.SetContentSize(
                PanelRight.Right,
                PanelTop.Height +
                    (Groups[GameFieldGroup.ReleaseBase].Visible
                        ? Groups[GameFieldGroup.ReleaseBase].CalcedHeight
                        : 8) + 12
            );
        }
    }
}
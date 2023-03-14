using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Panels;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Types;
using OxXMLEngine.Summary;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Summary
{
    internal class GamesSummary : OxCard, ISummaryPanel
    {
        public GamesSummary() : base() { }

        protected override void AfterCreated()
        {
            base.AfterCreated();
            int maxBottom = 0;

            foreach (OxLabel label in LevelValues.Values)
                maxBottom = Math.Max(maxBottom, label.Bottom);

            SetContentSize(new Size(250, maxBottom + 20));
            Dock = DockStyle.Top;
            Text = "General";
        }

        protected override void PrepareInnerControls()
        {
            base.PrepareInnerControls();
            PrepareLevelValues();
        }

        private OxLabel CreateLabel(int left, int top, FontStyle fontStyle) =>
            new()
            {
                Parent = ContentContainer,
                Left = left,
                Top = top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(Styles.FontFamily, Styles.DefaultFontSize + 1, fontStyle),
                AutoSize = true
            };

        private OxLabel CreateLevelValueControl(LevelValueType valueType, int top)
        {
            top += SummaryConsts.VerticalSpace / 3;
            OxLabel captionLabel = CreateLabel(
                SummaryConsts.HorizontalSpace * 3,
                top,
                FontStyle.Italic
            );
            captionLabel.Text = TypeHelper.Name(valueType);
            OxLabel valueLabel = CreateLabel(
                captionLabel.Right + SummaryConsts.HorizontalSpace * 2,
                top,
                FontStyle.Bold
            );

            OxControlHelper.AlignByBaseLine(valueLabel, captionLabel);
            LevelValues.Add(valueType, valueLabel);
            return valueLabel;
        }

        private void PrepareLevelValues()
        {
            int lastBottom = 20;
            LevelValueTypeGroup group = LevelValueTypeGroup.Points;

            foreach (LevelValueType valueType in TypeHelper.All<LevelValueType>())
            {
                LevelValueTypeGroup valueGroup = TypeHelper.Helper<LevelValueTypeHelper>().Group(valueType);

                if (group != valueGroup)
                {
                    lastBottom += 20;
                    group = valueGroup;
                }

                lastBottom = CreateLevelValueControl(valueType, lastBottom).Bottom;
            }

            int maxLeft = 0;

            foreach (OxLabel label in LevelValues.Values)
                maxLeft = Math.Max(maxLeft, label.Left);

            foreach (OxLabel label in LevelValues.Values)
                label.Left = maxLeft;
        }

        public void AlignAccessors() { }

        public void ClearAccessors() { }

        public void CalcPanelSize()
        {
            if (Header != null)
                Header.SetContentSize(SummaryConsts.CardWidth, SummaryConsts.CardHeaderHeight);
        }

        public void FillAccessors()
        {
            PSNLevelCalculator levelCalculator = new(DataManager.FullItemsList<GameField, Game>());

            foreach (var item in LevelValues)
                item.Value.Text = levelCalculator.Value(item.Key);
        }

        private readonly Dictionary<LevelValueType, OxLabel> LevelValues = new();
    }
}
using OxLibrary.Controls;
using OxDAOEngine.ControlFactory;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;
using OxDAOEngine.Data.Types;

namespace PlayStationGames.GameEngine.Editor
{
    public class TrophiesControlsHelper
    {
        private readonly ControlBuilder<GameField, Game> Builder;
        private readonly GameEditorLayoutsGenerator Generator;

        public TrophiesControlsHelper(ControlBuilder<GameField, Game> controlBuiler, 
            GameEditorLayoutsGenerator generator)
        {
            Builder = controlBuiler;
            Generator = generator;
        }

        private readonly GameFieldHelper fieldHelper = TypeHelper.Helper<GameFieldHelper>();

        public void ClearTrophiesControlsConstraints()
        {
            foreach (GameField field in fieldHelper.TrophiesFields)
                Builder.ClearValueConstraints(field);
        }

        public void CalcTrophiesControls()
        {
            TrophysetType? trophiesAvailableValue =
                Builder.Value<TrophysetType>(GameField.TrophysetType);
            bool trophiesAvailable = trophiesAvailableValue != null
                && !trophiesAvailableValue.Equals(TrophysetType.NoSet);

            if (!trophiesAvailable)
                Builder[GameField.AvailablePlatinum].Value = false;

            Builder[GameField.AvailableGold].MaximumValue = trophiesAvailable ? 200 : 0;
            Builder[GameField.AvailableSilver].MaximumValue = trophiesAvailable ? 200 : 0;
            Builder[GameField.AvailableBronze].MaximumValue = trophiesAvailable ? 200 : 0;

            Builder[GameField.EarnedPlatinum].Enabled = Builder[GameField.AvailablePlatinum].BoolValue;

            foreach (var item in DislbledIfOtherZero)
                Builder[item.Key].Enabled
                    = Builder[item.Value].IntValue > 0;

            Builder[GameField.AvailableFromDLC].MaximumValue = Builder[GameField.AvailableGold].IntValue
                + Builder[GameField.AvailableSilver].IntValue
                + Builder[GameField.AvailableBronze].IntValue;

            Builder[GameField.EarnedGold].MaximumValue = Builder[GameField.AvailableGold].IntValue;
            Builder[GameField.EarnedSilver].MaximumValue = Builder[GameField.AvailableSilver].IntValue;
            Builder[GameField.EarnedBronze].MaximumValue = Builder[GameField.AvailableBronze].IntValue;

            int totalEarnedTrophies =
                Builder[GameField.EarnedGold].IntValue
                + Builder[GameField.EarnedSilver].IntValue
                + Builder[GameField.EarnedBronze].IntValue;

            Builder[GameField.EarnedFromDLC].MaximumValue = Math.Min(
                Builder[GameField.AvailableFromDLC].IntValue,
                totalEarnedTrophies);

            if (!Builder[GameField.EarnedPlatinum].Enabled)
                Builder[GameField.EarnedPlatinum].Value = false;

            foreach (GameField field in ZeroIfDisabled)
                if (!Builder[field].Enabled)
                    Builder[field].Value = 0;

            if (!Builder[GameField.Difficult].Enabled)
                Builder[GameField.Difficult].Value = Difficult.Unknown;

            if (!Builder[GameField.CompleteTime].Enabled)
                Builder[GameField.CompleteTime].Value = CompleteTime.ctUnknown;
        }

        public void ClearUnusedCaptions()
        {
            ((OxLabel)Builder[GameField.EarnedPlatinum].Control.Tag).Top -= 4;
            Builder[GameField.EarnedFromDLC].Text = string.Empty;
            Builder[GameField.AvailablePlatinum].Text = string.Empty;
            Builder[GameField.EarnedPlatinum].Text = string.Empty;
        }

        public void AlignLabels()
        {
            int minLabelLeft = int.MaxValue;

            foreach (GameField field in FieldsWithLabel)
                minLabelLeft = Math.Min(minLabelLeft,
                    ((OxLabel)Builder.Control(field).Tag).Left);

            foreach (GameField field in FieldsWithLabel)
                ((OxLabel)Builder.Control(field).Tag).Left = minLabelLeft;
        }

        private int SetTrophiesPairControlsVisible(GameField mainField, GameField dependField, bool visible, int lastBottom, bool needOffset)
        {
            Builder.SetVisible(mainField, visible);
            Builder.SetVisible(dependField, visible);

            if (visible)
            {
                if (needOffset)
                    lastBottom += Generator.Offset(mainField);

                Builder[mainField].Top = lastBottom;
                Builder[dependField].Top = lastBottom;
                lastBottom = Builder[mainField].Bottom;
            }

            return lastBottom;
        }

        public void SetTrophiesControlsVisible(bool verified)
        {
            int lastBottom = Generator.Top(GameField.AvailablePlatinum) 
                - Generator.Offset(GameField.AvailablePlatinum);

            bool visibleControlExist =
                Builder.Value<TrophysetType>(GameField.TrophysetType) != TrophysetType.NoSet
                && (!verified || Builder[GameField.AvailablePlatinum].BoolValue);

            lastBottom = SetTrophiesPairControlsVisible(
                GameField.AvailablePlatinum,
                GameField.EarnedPlatinum,
                visibleControlExist,
                lastBottom,
                false);

            foreach (var item in DislbledIfOtherZero)
            {
                bool currentVisible =
                    Builder.Value<TrophysetType>(GameField.TrophysetType) != TrophysetType.NoSet
                    && (!verified || Builder[item.Value].IntValue != 0);
                lastBottom = SetTrophiesPairControlsVisible(
                    item.Value,
                    item.Key,
                    currentVisible,
                    lastBottom,
                    visibleControlExist);
                visibleControlExist |= currentVisible;
            }
        }

        private readonly List<GameField> FieldsWithLabel = 
            new()
            {
                GameField.EarnedPlatinum,
                GameField.EarnedGold,
                GameField.EarnedSilver,
                GameField.EarnedBronze,
                GameField.EarnedFromDLC
            };

        private readonly List<GameField> ZeroIfDisabled = 
            new()
            {
                GameField.EarnedGold,
                GameField.EarnedSilver,
                GameField.EarnedBronze,
                GameField.EarnedFromDLC
            };

        private readonly Dictionary<GameField, GameField> DislbledIfOtherZero =
            new()
            {
                [GameField.EarnedGold] = GameField.AvailableGold,
                [GameField.EarnedSilver] = GameField.AvailableSilver,
                [GameField.EarnedBronze] = GameField.AvailableBronze,
                [GameField.EarnedFromDLC] = GameField.AvailableFromDLC
            };
    }
}
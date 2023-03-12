using OxLibrary.Controls;
using OxXMLEngine.ControlFactory;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Editor
{
    public class TrophiesControlsHelper
    {
        private readonly ControlBuilder<GameField, Game> controlBuilder;
        public TrophiesControlsHelper(ControlBuilder<GameField, Game> controlBuiler) =>
            controlBuilder = controlBuiler;

        public void ClearTrophiesControlsConstraints()
        {
            controlBuilder.ClearValueConstraints(GameField.EarnedGold);
            controlBuilder.ClearValueConstraints(GameField.EarnedSilver);
            controlBuilder.ClearValueConstraints(GameField.EarnedBronze);
            controlBuilder.ClearValueConstraints(GameField.EarnedFromDLC);
            controlBuilder.ClearValueConstraints(GameField.EarnedNet);
            controlBuilder.ClearValueConstraints(GameField.AvailableGold);
            controlBuilder.ClearValueConstraints(GameField.AvailableSilver);
            controlBuilder.ClearValueConstraints(GameField.AvailableBronze);
            controlBuilder.ClearValueConstraints(GameField.AvailableFromDLC);
            controlBuilder.ClearValueConstraints(GameField.AvailableNet);
        }

        public void CalcTrophiesControls()
        {
            TrophysetAccessibility? trophiesAvailableValue = 
                (TrophysetAccessibility?)controlBuilder.Value(GameField.TrophysetAccess);
            bool trophiesAvailable = trophiesAvailableValue != null 
                && !trophiesAvailableValue.Equals(TrophysetAccessibility.NoSet);

            if (!trophiesAvailable)
                controlBuilder[GameField.AvailablePlatinum].Value = false;

            controlBuilder[GameField.AvailableGold].MaximumValue = trophiesAvailable ? 200 : 0;
            controlBuilder[GameField.AvailableSilver].MaximumValue = trophiesAvailable ? 200 : 0;
            controlBuilder[GameField.AvailableBronze].MaximumValue = trophiesAvailable ? 200 : 0;

            controlBuilder[GameField.EarnedPlatinum].Enabled = controlBuilder[GameField.AvailablePlatinum].BoolValue;

            foreach (KeyValuePair<GameField, GameField> item in DislbledIfOtherZero)
                controlBuilder[item.Key].Enabled
                    = controlBuilder[item.Value].IntValue > 0;

            controlBuilder[GameField.AvailableFromDLC].MaximumValue = controlBuilder[GameField.AvailableGold].IntValue
                + controlBuilder[GameField.AvailableSilver].IntValue
                + controlBuilder[GameField.AvailableBronze].IntValue;
            controlBuilder[GameField.AvailableNet].MaximumValue = controlBuilder[GameField.AvailableFromDLC].MaximumValue;

            controlBuilder[GameField.EarnedGold].MaximumValue = controlBuilder[GameField.AvailableGold].IntValue;
            controlBuilder[GameField.EarnedSilver].MaximumValue = controlBuilder[GameField.AvailableSilver].IntValue;
            controlBuilder[GameField.EarnedBronze].MaximumValue = controlBuilder[GameField.AvailableBronze].IntValue;

            int totalEarnedTrophies =
                controlBuilder[GameField.EarnedGold].IntValue
                + controlBuilder[GameField.EarnedSilver].IntValue
                + controlBuilder[GameField.EarnedBronze].IntValue;

            controlBuilder[GameField.EarnedFromDLC].MaximumValue = Math.Min(
                controlBuilder[GameField.AvailableFromDLC].IntValue,
                totalEarnedTrophies);
            controlBuilder[GameField.EarnedNet].MaximumValue = Math.Min(
                controlBuilder[GameField.AvailableNet].IntValue,
                totalEarnedTrophies);

            if (!controlBuilder[GameField.EarnedPlatinum].Enabled)
                controlBuilder[GameField.EarnedPlatinum].Value = false;

            foreach (GameField field in ZeroIfDisabled)
                if (!controlBuilder[field].Enabled)
                    controlBuilder[field].Value = 0;

            if (!controlBuilder[GameField.Difficult].Enabled)
                controlBuilder[GameField.Difficult].Value = DifficultRank.Unknown;

            if (!controlBuilder[GameField.CompleteTime].Enabled)
                controlBuilder[GameField.CompleteTime].Value = CompleteTime.ctUnknown;
        }

        public void ClearUnusedCaptions()
        {
            ((OxLabel)controlBuilder[GameField.EarnedPlatinum].Control.Tag).Top -= 4;
            controlBuilder[GameField.EarnedFromDLC].Text = string.Empty;
            controlBuilder[GameField.AvailablePlatinum].Text = string.Empty;
            controlBuilder[GameField.EarnedPlatinum].Text = string.Empty;
        }

        public void AlignLabels()
        {
            int minLabelLeft = int.MaxValue;

            foreach (GameField field in FieldsWithLabel)
                minLabelLeft = Math.Min(minLabelLeft,
                    ((OxLabel)controlBuilder.Control(field).Tag).Left);

            foreach (GameField field in FieldsWithLabel)
                ((OxLabel)controlBuilder.Control(field).Tag).Left = minLabelLeft;
        }

        private readonly List<GameField> FieldsWithLabel = 
            new()
            {
                GameField.EarnedPlatinum,
                GameField.EarnedGold,
                GameField.EarnedSilver,
                GameField.EarnedBronze,
                GameField.EarnedFromDLC,
                GameField.EarnedNet
            };

        private readonly List<GameField> ZeroIfDisabled = 
            new()
            {
                GameField.EarnedGold,
                GameField.EarnedSilver,
                GameField.EarnedBronze,
                GameField.EarnedFromDLC,
                GameField.EarnedNet
            };

        private readonly Dictionary<GameField, GameField> DislbledIfOtherZero =
            new()
            {
                [GameField.EarnedGold] = GameField.AvailableGold,
                [GameField.EarnedSilver] = GameField.AvailableSilver,
                [GameField.EarnedBronze] = GameField.AvailableBronze,
                [GameField.EarnedFromDLC] = GameField.AvailableFromDLC,
                [GameField.EarnedNet] = GameField.AvailableNet
            };
    }
}
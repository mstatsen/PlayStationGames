using OxDAOEngine.ControlFactory.Filter;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Settings;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Filter
{
    public class GameQuickFilterLayouter : IQuickFilterLayouter<GameField>
    {
        public GameField TextFilterContainer => GameField.Name;
        public int FieldWidth(GameField field) => 
            field switch
            {
                GameField.Difficult or 
                GameField.Pegi or 
                GameField.AvailablePlatinum or 
                GameField.Year => 
                    56,
                GameField.Format => 
                    72,
                GameField.Genre => 
                    160,
                GameField.CompleteTime => 
                    80,
                GameField.TrophysetType => 
                    148,
                _ => 
                    100,
            };

        public bool IsLastLayoutForOneRow(GameField field, GameField lastField) => 
            field switch
            {
                GameField.Format => lastField == GameField.Platform,
                GameField.Genre => lastField == GameField.ScreenView,
                _ => false,
            };

        public string FieldCaption(GameField field, QuickFilterVariant variant)
        {
            List<GameField> quickFilterFields = SettingsManager.DAOSettings<GameField>()
                .QuickFilterFields.Fields;

            switch (field)
            {
                case GameField.ScreenView:
                    if (variant == QuickFilterVariant.Export ||
                        (variant == QuickFilterVariant.Base &&
                            quickFilterFields.Contains(GameField.ScreenView) &&
                            quickFilterFields.Contains(GameField.Genre)))
                        return "Genre";
                    break;

                case GameField.CompleteTime:
                    if (variant == QuickFilterVariant.Export ||
                        (variant == QuickFilterVariant.Base &&
                        quickFilterFields.Contains(GameField.CompleteTime)))
                        return "Full time";
                    break;

                case GameField.TrophysetType:
                    if (variant == QuickFilterVariant.Export ||
                        (variant == QuickFilterVariant.Base &&
                        quickFilterFields.Contains(GameField.TrophysetType)))
                        return "Trophyset";
                    break;
            }

            return string.Empty;
        }
    }
}

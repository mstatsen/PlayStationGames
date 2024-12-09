using OxDAOEngine.ControlFactory.Filter;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Settings;
using OxLibrary;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Filter
{
    public class GameQuickFilterLayouter : IQuickFilterLayouter<GameField>
    {
        public GameField TextFilterContainer => GameField.Name;
        public short FieldWidth(GameField field) => 
            field switch
            {
                GameField.Difficult or 
                GameField.Pegi or 
                GameField.AvailablePlatinum or
                GameField.Licensed or
                GameField.SinglePlayer or
                GameField.Multiplayer or
                GameField.Installed or
                GameField.Year =>
                    56,
                GameField.Format =>
                    72,
                GameField.Genre =>
                    160,
                GameField.CompleteTime =>
                    80,
                GameField.Owner =>
                    138,
                GameField.TrophysetType =>
                    118,
                _ =>
                    100,
            };

        public bool IsLastLayoutForOneRow(GameField field, GameField lastField) => 
            field switch
            {
                GameField.Format => lastField is GameField.Platform,
                GameField.Genre => lastField is GameField.ScreenView,
                _ => false,
            };

        public string FieldCaption(GameField field, QuickFilterVariant variant)
        {
            List<GameField> quickFilterFields = SettingsManager.DAOSettings<GameField>()
                .QuickFilterFields.Fields;

            switch (field)
            {
                case GameField.ScreenView:
                    if (variant is QuickFilterVariant.Export 
                        || (variant is QuickFilterVariant.Base 
                            && quickFilterFields.Contains(GameField.ScreenView) 
                            && quickFilterFields.Contains(GameField.Genre)))
                        return "Genre";
                    break;

                case GameField.SinglePlayer:
                    return "Single";

                case GameField.CompleteTime:
                    if (variant is QuickFilterVariant.Export 
                        || (variant is QuickFilterVariant.Base 
                            && quickFilterFields.Contains(GameField.CompleteTime)))
                        return "Full time";
                    break;

                case GameField.TrophysetType:
                    if (variant is QuickFilterVariant.Export 
                        || (variant is QuickFilterVariant.Base 
                            && quickFilterFields.Contains(GameField.TrophysetType)))
                        return "Trophyset";
                    break;

                case GameField.AvailablePlatinum:
                    return "Platinum";
            }

            return string.Empty;
        }
    }
}
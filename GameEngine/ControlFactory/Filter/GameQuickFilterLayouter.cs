﻿using OxXMLEngine.ControlFactory.Filter;
using OxXMLEngine.Data.Filter;
using OxXMLEngine.Settings;
using PlayStationGames.GameEngine.Data.Fields;
using System.Collections.Generic;

namespace PlayStationGames.GameEngine.ControlFactory.Filter
{
    public class GameQuickFilterLayouter : IQuickFilterLayouter<GameField>
    {
        public GameField TextFilterContainer => GameField.Name;
        public int FieldWidth(GameField field) => 
            field switch
            {
                GameField.Progress or 
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
                GameField.TrophysetAccess => 
                    148,
                _ => 
                    100,
            };

        public bool IsLastLayoutForOneRow(GameField field, GameField lastField) => 
            field switch
            {
                GameField.Progress => lastField == GameField.Status,
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

                case GameField.TrophysetAccess:
                    if (variant == QuickFilterVariant.Export ||
                        (variant == QuickFilterVariant.Base &&
                        quickFilterFields.Contains(GameField.TrophysetAccess)))
                        return "Trophyset";
                    break;
            }

            return string.Empty;
        }
    }
}

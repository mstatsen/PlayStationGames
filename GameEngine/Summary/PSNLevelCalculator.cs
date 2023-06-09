﻿using OxXMLEngine.Data;
using OxXMLEngine.Data.Extract;
using OxXMLEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Summary
{
    public class PSNLevelCalculator
    {
        private readonly RootListDAO<GameField, Game> Games;

        private int GetEarnedPoints(GameField field)
        {
            int earnedPoints = 0;

            FieldExtractor<GameField, Game> extractor = new(Games.Distinct(Game.CheckUniqueTrophyset));

            foreach (object extractItem in extractor.Extract(field, false))
                earnedPoints += (int)extractItem;

            return earnedPoints;
        }

        private int GetEarnedPointsNew() =>
            GetEarnedPoints(GameField.EarnedPoints);


        private int GetEarnedPointsOld() =>
            GetEarnedPoints(GameField.EarnedPointsOld);

        private static string GetPSNLevel(List<LevelInfo> levels, int earnedPoints)
        {
            int intLevel = 0;
            LevelInfo lastLevelInfo = levels[0];
            earnedPoints += lastLevelInfo.LevelUpPoint;

            foreach (LevelInfo levelInfo in levels)
                while (earnedPoints > levelInfo.LevelUpPoint)
                {
                    lastLevelInfo = levelInfo;
                    earnedPoints -= levelInfo.LevelUpPoint;
                    intLevel++;

                    if (intLevel > levelInfo.Level)
                        break;
                }

            float percent = 0;

            if (earnedPoints > 0)
                percent = earnedPoints * 100  / (float)lastLevelInfo.LevelUpPoint;

            return $"{intLevel} ({(int)percent}%)";
        }

        public PSNLevelCalculator(RootListDAO<GameField, Game> games) =>
            Games = games;

        private string TrophiesCount(TrophyType type) => 
            new FieldExtractor<GameField, Game>(Games.Distinct(Game.CheckUniqueTrophyset))
                .Sum(TypeHelper.Helper<TrophyTypeHelper>().EarnedGameField(type), null)
                .ToString();

        private string PSNLevel() =>
            GetPSNLevel(LevelInfo.NewLevels, GetEarnedPointsNew());

        private string PSNLevelOld() =>
            GetPSNLevel(LevelInfo.OldLevels, GetEarnedPointsOld());

        private string Progress()
        {
            if (Games.Count == 0)
                return "N/A";

            int progress = 0;

            foreach (Game game in Games)
                progress += DAO.IntValue(game[GameField.Progress]);

            return $"{progress / Games.Count}%";

        }

        public string Value(LevelValueType valueType) => 
            valueType switch
            {
                LevelValueType.Points => 
                    GetEarnedPointsNew().ToString(),
                LevelValueType.Level => 
                    PSNLevel(),
                LevelValueType.LevelOld => 
                    PSNLevelOld(),
                LevelValueType.CountPlatinum => 
                    TrophiesCount(TrophyType.Platinum),
                LevelValueType.CountGold => 
                    TrophiesCount(TrophyType.Gold),
                LevelValueType.CountSilver => 
                    TrophiesCount(TrophyType.Silver),
                LevelValueType.CountBronze => 
                    TrophiesCount(TrophyType.Bronze),
                LevelValueType.CountFromDLC => 
                    TrophiesCount(TrophyType.FromDLC),
                LevelValueType.CountNet => 
                    TrophiesCount(TrophyType.Net),
                LevelValueType.Progress => 
                    Progress(),
                _ => 
                    string.Empty,
            };
    }
}
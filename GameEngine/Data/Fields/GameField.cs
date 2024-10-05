namespace PlayStationGames.GameEngine.Data.Fields
{
    public enum GameField
    {
        Id,
        Name,
        Licensed,
        Owner,
        Region,
        Language,
        Code,
        Image,
        Edition,
        Series,
        CriticScore,
        PlatformFamily,
        Platform,
        Source,
        Format,
        Developer,
        Publisher,
        Year,
        Pegi,
        ReleasePlatforms,
        Genre,
        ScreenView,
        Dlcs,
        Links,
        Installations,
        RelatedGames,
        GameModes,
        EmulatorType,
        EmulatorROMs,
        Verified,
        Tags,
        Trophyset,

        //calced fields

        TrophysetType,
        Difficult,
        CompleteTime,
        AvailablePlatinum,
        AvailableGold,
        AvailableSilver,
        AvailableBronze,

        FullGenre,

        //Links
        StrategeLink,
        PSNProfilesLink,

        //system
        Field,
    };
}
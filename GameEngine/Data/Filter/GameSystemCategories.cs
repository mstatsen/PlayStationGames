using OxXMLEngine.Data;
using OxXMLEngine.Data.Filter;
using OxXMLEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;
using System.Collections.Generic;

namespace PlayStationGames.GameEngine.Data.Filter
{
    public static class GameSystemCategories
    {
        private static Category<GameField, Game> Root =>
            new Category<GameField, Game>("All Games")
                .AddChild(Verifying())
                .AddChild(BadFilling())
                .AddChild(Availabitity())
                .AddChild(Progress())
                .AddChild(SeveralCopies())
                .AddChild(GameModes())
                .AddChild(ReleasedOn())
                .AddChild(Critic());

        public static Categories<GameField, Game> Categories
        {
            get
            {
                Categories<GameField, Game> result = new();
                result.AddRange(Root.Childs);
                return result;
            }
        }

        private static Category<GameField, Game> Verifying() =>
            new Category<GameField, Game>("Verifying")
                .AddChild(
                    new Category<GameField, Game>("Verified")
                        .AddFilterEquals(GameField.Verified, true)
                )
                .AddChild(
                    new Category<GameField, Game>("Unverified")
                        .AddFilterEquals(GameField.Verified, false)
                );

        private static Category<GameField, Game> Critic() =>
            new Category<GameField, Game>("Critic")
                .AddChild(
                    new Category<GameField, Game>("75-100")
                        .AddFilterGreater(GameField.CriticScore, 74)
                )
                .AddChild(
                    new Category<GameField, Game>("50-74")
                        .AddFilterGreater(GameField.CriticScore, 49, FilterConcat.AND)
                        .AddFilterLower(GameField.CriticScore, 75, FilterConcat.AND)
                )
                .AddChild(
                    new Category<GameField, Game>("0-49")
                        .AddFilterLower(GameField.CriticScore, 50)
                );

        private static Category<GameField, Game> GameModes()
        {
            Category<GameField, Game> modesCategory = new("Games modes");
            Category<GameField, Game> multiplayerCategory = new ("Multiplayer", FiltrationType.BaseOnChilds);
            PlayModeHelper playModeHelper = TypeHelper.Helper<PlayModeHelper>();
            PlayModeGroupHelper playModeGroupHelper = TypeHelper.Helper<PlayModeGroupHelper>();

            foreach (PlayModeGroup group in TypeHelper.All<PlayModeGroup>())
            {
                PlayModeList modes = playModeGroupHelper.Modes(group);
                Category<GameField, Game> groupCategory = new(
                    playModeGroupHelper.Name(group),
                    modes.Count == 1 ? FiltrationType.StandAlone : FiltrationType.BaseOnChilds);

                Category<GameField, Game> parentCategory = playModeGroupHelper.IsMultiplayer(group)
                    ? multiplayerCategory
                    : modesCategory;
                parentCategory.AddChild(groupCategory);

                if (modes.Count == 1)
                    groupCategory.AddFilter(GameField.GameModes, new ListDAO<GameMode> { new GameMode(modes[0])});
                else
                    foreach (PlayMode mode in modes)
                        groupCategory.AddChild(
                            new Category<GameField, Game>(playModeHelper.ShortName(mode))
                                .AddFilter(GameField.GameModes, new ListDAO<GameMode> { new GameMode(mode)})
                        );
            }
            modesCategory.AddChild(multiplayerCategory);
            return modesCategory;
        }

        private static Category<GameField, Game> SeveralCopies() =>
            new Category<GameField, Game>("Several copies")
                .AddFilterNotBlank(GameField.RelatedGames);

        private static Category<GameField, Game> Progress() => 
            new Category<GameField, Game>("Progress")
                .AddChild(
                    new Category<GameField, Game>(TypeHelper.Name(Status.NotStarted))
                        .AddFilterEquals(GameField.Status, Status.NotStarted)
                )
                .AddChild(
                    new Category<GameField, Game>(TypeHelper.Name(Status.Started))
                        .AddFilterEquals(GameField.Status, Status.Started)
                        .AddChild(
                            new Category<GameField, Game>("1-25%", FiltrationType.IncludeParent)
                                .AddFilterGreater(GameField.Progress, 0, FilterConcat.AND)
                                .AddFilterLower(GameField.Progress, 26, FilterConcat.AND)
                            )
                        .AddChild(
                            new Category<GameField, Game>("26-50%", FiltrationType.IncludeParent)
                                .AddFilterGreater(GameField.Progress, 25, FilterConcat.AND)
                                .AddFilterLower(GameField.Progress, 51, FilterConcat.AND)
                            )
                        .AddChild(
                            new Category<GameField, Game>("51-75%", FiltrationType.IncludeParent)
                                .AddFilterGreater(GameField.Progress, 50, FilterConcat.AND)
                                .AddFilterLower(GameField.Progress, 76, FilterConcat.AND)
                            )
                        .AddChild(
                            new Category<GameField, Game>("76-99%", FiltrationType.IncludeParent)
                                .AddFilterGreater(GameField.Progress, 75, FilterConcat.AND)
                                .AddFilterLower(GameField.Progress, 100, FilterConcat.AND)
                            )
                        .AddChild(
                            new Category<GameField, Game>("Platinum, not 100%", FiltrationType.IncludeParent)
                                .AddFilterEquals(GameField.EarnedPlatinum, 1, FilterConcat.AND)
                                .AddFilterLower(GameField.Progress, 100, FilterConcat.AND)
                            )
                )
                .AddChild(
                    new Category<GameField, Game>(TypeHelper.Name(Status.Completed))
                        .AddFilterEquals(GameField.Status, Status.Completed)
                );

        private static Category<GameField, Game> Availabitity() => 
            new Category<GameField, Game>("Availability")
                    .AddChild(
                        new Category<GameField, Game>("In stock")
                            .AddFilterEquals(GameField.Source, Source.PSN, FilterConcat.OR)
                            .AddFilterEquals(GameField.Source, Source.Physical, FilterConcat.OR)
                            .AddFilterEquals(GameField.Source, Source.PSPlus, FilterConcat.OR)
                            .AddFilterEquals(GameField.Source, Source.PlayAtHome, FilterConcat.OR)
                            .AddChild(
                                new Category<GameField, Game>("Purchased")
                                    .AddFilterEquals(GameField.Source, Source.PSN, FilterConcat.OR)
                                    .AddFilterEquals(GameField.Source, Source.Physical, FilterConcat.OR)
                            )
                            .AddChild(
                                new Category<GameField, Game>("Digital")
                                    .AddFilterEquals(GameField.Source, Source.PSN, FilterConcat.OR)
                                    .AddFilterEquals(GameField.Source, Source.PSPlus, FilterConcat.OR)
                                    .AddFilterEquals(GameField.Source, Source.PlayAtHome, FilterConcat.OR)
                                    .AddChild(
                                        new Category<GameField, Game>("Subscribe")
                                            .AddFilterEquals(GameField.Source, Source.PSPlus, FilterConcat.OR)
                                            .AddFilterEquals(GameField.Source, Source.PlayAtHome, FilterConcat.OR)
                                    )
                            )
                    )
                    .AddChild(
                        new Category<GameField, Game>("Lost")
                            .AddFilterEquals(GameField.Source, Source.Lost)
                    );

        private static Category<GameField, Game> ReleasedOnFamily(PlatformFamily family)
        {
            Category<GameField, Game> familyCategory = new(
                TypeHelper.Name(family),
                FiltrationType.BaseOnChilds);

            foreach (PlatformType type in TypeHelper.All<PlatformType>())
                if (TypeHelper.DependsOnValue<PlatformType, PlatformFamily>(type).Equals(family))
                    familyCategory.AddChild(
                        new Category<GameField, Game>(TypeHelper.Name(type))
                            .AddFilter(
                                GameField.ReleasePlatforms,
                                new Platforms(type)
                            )
                    );

            return familyCategory;
        }

        private static Category<GameField, Game> ReleasedOn()
        {
            Category<GameField, Game> releasedOn = new Category<GameField, Game>("Released on")
                .AddChild(OnlyPlayStation);

            foreach (PlatformFamily family in TypeHelper.Actual<PlatformFamily>())
                releasedOn.AddChild(ReleasedOnFamily(family));

            return releasedOn;
        }


        private static Category<GameField, Game> OnlyPlayStation
        {
            get
            {
                Category<GameField, Game> category = new("Only on PlayStation");
                category.Filter.Root.FilterConcat = FilterConcat.AND;
                PlatformTypeHelper helper = TypeHelper.Helper<PlatformTypeHelper>();

                foreach (PlatformType type in TypeHelper.Actual<PlatformType>())
                    if (!helper.DependsOnValue<PlatformFamily>(type).Equals(PlatformFamily.Sony))
                        category.AddFilterNotContains(
                            GameField.ReleasePlatforms,
                            new Platforms(type),
                            FilterConcat.AND);

                return category;
            }
        }

        private static Category<GameField, Game> BadTrophyset()
        {
            List<GameField> trophyFields = new()
            {
                GameField.AvailablePlatinum,
                GameField.AvailableGold,
                GameField.AvailableSilver,
                GameField.AvailableBronze
            };

            Category<GameField, Game> result = new Category<GameField, Game>("Bad trophyset")
                .AddFilterEquals(GameField.TrophysetAccess,
                    TrophysetAccessibility.NoSet,
                    FilterConcat.AND);

            foreach (GameField field in trophyFields)
                result.AddFilterGreater(field, 0);

            Category<GameField, Game> badTrophyset2 = new Category<GameField, Game>()
                .AddFilterEquals(GameField.TrophysetAccess,
                    TrophysetAccessibility.Ordinary,
                    FilterConcat.AND);

            foreach (GameField field in trophyFields)
                badTrophyset2.AddFilterEquals(field, 0);

            result.Filter.Root.FilterConcat = FilterConcat.OR;
            result.Filter.Root.Add(badTrophyset2);
            return result;
        }

        private static Category<GameField, Game> BadFilling() =>
            new Category<GameField, Game>("Bad Filling",
                FiltrationType.BaseOnChilds)
                .AddChild(BadTrophyset())
                .AddChild(
                    new Category<GameField, Game>("Critic Score Empty")
                        .AddFilterBlank(GameField.CriticScore)
                        .AddFilterEquals(GameField.CriticScore, -1)
                        .AddFilterEquals(GameField.CriticScore, 0)
                )
                .AddChild(
                    new Category<GameField, Game>("Comlete time not filled")
                        .AddFilterEquals(GameField.CompleteTime, CompleteTime.ctUnknown)
                )
                .AddChild(
                    new Category<GameField, Game>("Difficult not filled")
                        .AddFilterEquals(GameField.Difficult, Difficult.Unknown)
                )
                .AddChild(
                    new Category<GameField, Game>("Genre not filled")
                        .AddFilterEquals(GameField.ScreenView, ScreenView.svOther)
                        .AddFilterBlank(GameField.Genre)
                )
                .AddChild(
                    new Category<GameField, Game>("Image is Empty")
                        .AddFilterBlank(GameField.Image))
                .AddChild(
                    new Category<GameField, Game>("Play Mode Empty")
                        .AddFilterBlank(GameField.GameModes)
                )
                .AddChild(
                    new Category<GameField, Game>("Release Info not filled")
                        .AddFilterBlank(GameField.Developer)
                        .AddFilterBlank(GameField.Publisher)
                        .AddFilterBlank(GameField.Year)
                        .AddFilterEquals(GameField.Year, -1)
                        .AddFilterEquals(GameField.Pegi, Pegi.Unknown)
                        .AddFilterBlank(GameField.ReleasePlatforms)
                )
                .AddChild(
                    new Category<GameField, Game>("Some Link Lost")
                        .AddFilterNotEquals(GameField.TrophysetAccess,
                            TrophysetAccessibility.NoSet,
                            FilterConcat.AND)
                        .AddFilterBlank(GameField.StrategeLink)
                        .AddFilterBlank(GameField.PSNProfilesLink)
                );

        private static Category<GameField, Game>? FindCategory(Category<GameField, Game> parentCategory, string name)
        {
            if (parentCategory.Name.Equals(name))
                return parentCategory;

            foreach (Category<GameField, Game> category in parentCategory.Childs)
            { 
                Category<GameField, Game>? result = FindCategory(category, name);

                if (result != null)
                    return result;
            }

            return null;
        }

        public static Category<GameField, Game> GetCategory(string name)
        {
            Category<GameField, Game>? result = FindCategory(Root, name);

            if (result == null)
                result = Root;

            return result;
        }
    }
}

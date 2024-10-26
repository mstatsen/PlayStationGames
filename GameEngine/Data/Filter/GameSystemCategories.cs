using OxDAOEngine.Data;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Data.Filter.Types;
using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Filter
{
    public static class GameSystemCategories
    {
        private static Category<GameField, Game> Root =>
            new Category<GameField, Game>("All Games")
                .AddChild(Verifying())
                .AddChild(BadFilling())
                .AddChild(Availabitity())
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

            Category<GameField, Game> result = new("Bad trophyset");

            SimpleFilter<GameField, Game> badTrophyset = new SimpleFilter<GameField, Game>(FilterConcat.AND)
                .AddFilter(GameField.TrophysetType, FilterOperation.Equals, TrophysetType.NoSet);

            foreach (GameField @field in trophyFields)
                badTrophyset.AddFilter(@field, FilterOperation.Greater, 0);

            SimpleFilter<GameField, Game> badTrophyset2 = new SimpleFilter<GameField, Game>(FilterConcat.AND)
                .AddFilter(GameField.TrophysetType, FilterOperation.NotEquals,
                    TrophysetType.NoSet);

            foreach (GameField @field in trophyFields)
                badTrophyset2.AddFilter(@field, FilterOperation.Equals, 0);

            result.Filter.Root.AddFilter(badTrophyset, FilterConcat.OR);
            result.Filter.Root.AddFilter(badTrophyset2, FilterConcat.OR);
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
                        .AddFilterEquals(GameField.CompleteTime, CompleteTime.Unknown)
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
                    new Category<GameField, Game>("Release Info not filled")
                        .AddFilterBlank(GameField.Developer)
                        .AddFilterBlank(GameField.Publisher)
                        .AddFilterBlank(GameField.Year)
                        .AddFilterEquals(GameField.Year, -1)
                        .AddFilterEquals(GameField.Pegi, Pegi.Unknown)
                        .AddFilterBlank(GameField.ReleasePlatforms)
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
            result ??= Root;
            return result;
        }
    }
}

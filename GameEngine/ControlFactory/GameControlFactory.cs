using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Context;
using OxDAOEngine.ControlFactory.Filter;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Grid;
using OxDAOEngine.View;
using PlayStationGames.GameEngine.ControlFactory.Controls;
using PlayStationGames.GameEngine.ControlFactory.Filter;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;
using PlayStationGames.GameEngine.Grid;
using PlayStationGames.GameEngine.View;
using PlayStationGames.AccountEngine.ControlFactory.Accessors;
using PlayStationGames.GameEngine.ControlFactory.Controls.Initializers;
using PlayStationGames.GameEngine.ControlFactory.Initializers;
using PlayStationGames.GameEngine.ControlFactory.Accessors;

namespace PlayStationGames.GameEngine.ControlFactory
{
    public class GameControlFactory : ControlFactory<GameField, Game>
    {
        protected override IControlAccessor? CreateOtherAccessor(IBuilderContext<GameField, Game> context) =>
                context is FieldContext<GameField, Game> fieldContext
                    ? fieldContext.Field switch
                    {
                        GameField.PlatformFamily => CreateEnumAccessor<PlatformFamily>(context),
                        GameField.Platform => CreateEnumAccessor<PlatformType>(context),
                        GameField.ScreenView => CreateEnumAccessor<ScreenView>(context),
                        GameField.Format => CreateEnumAccessor<GameFormat>(context),
                        GameField.Source => CreateEnumAccessor<Source>(context),
                        GameField.Region => CreateEnumAccessor<GameRegion>(context),
                        GameField.Language => CreateEnumAccessor<GameLanguage>(context),
                        GameField.Pegi => CreateEnumAccessor<Pegi>(context),
                        GameField.CompleteTime => CreateEnumAccessor<CompleteTime>(context),
                        GameField.Difficult => CreateEnumAccessor<Difficult>(context),
                        GameField.TrophysetType => CreateEnumAccessor<TrophysetType>(context),
                        GameField.Year => new YearAccessor<GameField, Game>(context),
                        GameField.Dlcs => CreateListAccessor<DLC, ListDAO<DLC>, DLCListControl>(context, ControlScope.Editor),
                        GameField.Tags => CreateListAccessor<Tag, ListDAO<Tag>, TagListControl>(context, ControlScope.Editor),
                        GameField.Installations => CreateListAccessor<Installation, ListDAO<Installation>, InstallationsControl>(context, ControlScope.Editor),
                        GameField.RelatedGames => CreateListAccessor<RelatedGame, RelatedGames, RelatedGamesControl>(context),
                        GameField.ReleasePlatforms => CreateListAccessor<Platform, Platforms, ReleasePlatformListControl>(context),
                        GameField.Id => CreateLabelAccessor(context),
                        GameField.Owner => CreateAccountAccessor(context),
                        GameField.Trophyset => CreateTrophysetAccessor(context),
                        _ => base.CreateOtherAccessor(context),
                    }
                    : context.Key == "DLC:Trophyset"
                        ? CreateTrophysetAccessor(context) 
                        : context.Key == "DLC:TrophysetType"
                            ? CreateEnumAccessor<TrophysetType>(context)
                            : context.Key == "DLC:Difficult"
                                ? CreateEnumAccessor<Difficult>(context)
                                : context.Key == "DLC:CompleteTime"
                                    ? CreateEnumAccessor<CompleteTime>(context)
                                    : context.Key == "Trophyset:Account"
                                        ? CreateAccountAccessor(context)
                                        : base.CreateOtherAccessor(context);

        private static IControlAccessor CreateTrophysetAccessor(IBuilderContext<GameField, Game> context) => 
            new TrophysetAccessor(context);

        private static IControlAccessor CreateAccountAccessor(IBuilderContext<GameField, Game> context)
        {
            context.AdditionalContext ??= new AccountAccessorParameters()
                {
                    UseNullable = true,
                    OnlyNullable = true
                };
            return new AccountAccessor<GameField, Game>(context);
        }

        protected override IInitializer? Initializer(IBuilderContext<GameField, Game> context)
        {
            if (context is FieldContext<GameField, Game> accessorContext)
            {
                if (!context.IsQuickFilter &&
                    accessorContext.Field == GameField.CriticScore)
                    return new NumericInitializer(-1, 100);

                if (context.IsQuickFilter &&
                    (accessorContext.Field == GameField.Year))
                {
                    object? variant = BuilderVariant(context.Builder);
                    return new ExtractInitializer<GameField, Game>(accessorContext.Field, addAnyObject: true,
                        fullExtract: variant != null && variant.Equals(QuickFilterVariant.Export));
                }

                if (accessorContext.Field == GameField.Owner)
                    return new OwnerInitializer();
            }

            if (context.Name == "TagName")
                return new TagNameInitializer(null);

            return base.Initializer(context);
        }

        public override IItemInfo<GameField, Game> CreateInfoCard() =>
            new GameFullInfoCard();

        public override GridPainter<GameField, Game> CreateGridPainter(
            GridFieldColumns<GameField> columns, GridUsage usage) => 
            usage switch
            {
                GridUsage.SelectItem or 
                GridUsage.ChooseItems =>
                    new GameSelectorGridPainter(),
                _ => 
                    new GamesGridPainter(columns),
            };

        public override IQuickFilterLayouter<GameField> CreateQuickFilterLayouter() => 
            new GameQuickFilterLayouter();

        public override IItemCard<GameField, Game> CreateCard(ItemViewMode viewMode) =>
            new GameCard(viewMode);

        protected override ItemColorer<GameField, Game> CreateItemColorer() => 
            new GameColorer();
    }
}
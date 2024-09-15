using OxXMLEngine.ControlFactory;
using OxXMLEngine.ControlFactory.Accessors;
using OxXMLEngine.ControlFactory.Context;
using OxXMLEngine.ControlFactory.Filter;
using OxXMLEngine.ControlFactory.Initializers;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Filter;
using OxXMLEngine.Grid;
using OxXMLEngine.View;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.GameEngine.ControlFactory.Accessors;
using PlayStationGames.GameEngine.ControlFactory.Controls;
using PlayStationGames.GameEngine.ControlFactory.Filter;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;
using PlayStationGames.GameEngine.Grid;
using PlayStationGames.GameEngine.View;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.ControlFactory.Accessors;

namespace PlayStationGames.GameEngine.ControlFactory
{
    public class GameControlFactory : ControlFactory<GameField, Game>
    {
        protected override IControlAccessor? CreateOtherAccessor(IBuilderContext<GameField, Game> context) => 
            context switch
            {
                FieldContext<GameField, Game> accessorContext =>
                accessorContext.Field switch
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
                    GameField.Status => CreateEnumAccessor<Status>(context),
                    GameField.TrophysetAccess => CreateEnumAccessor<TrophysetAccess>(context),
                    GameField.Year => new YearAccessor<GameField, Game>(context),
                    GameField.GameModes => CreateListAccessor<GameMode, ListDAO<GameMode>, GameModesControl>(context, ControlScope.Editor),
                    GameField.Dlcs => CreateListAccessor<DLC, ListDAO<DLC>, DLCListControl>(context, ControlScope.Editor),
                    GameField.Tags => CreateListAccessor<Tag, ListDAO<Tag>, TagListControl>(context, ControlScope.Editor),
                    GameField.Installations => CreateListAccessor<Installation, ListDAO<Installation>, InstallationsControl>(context, ControlScope.Editor),
                    GameField.Links => NewLinkAccessor(context),
                    GameField.RelatedGames => CreateListAccessor<RelatedGame, RelatedGames, RelatedGamesControl>(context),
                    GameField.ReleasePlatforms => CreateListAccessor<Platform, Platforms, ReleasePlatformListControl>(context),
                    GameField.Id => CreateLabelAccessor(context),
                    GameField.StrategeLink or GameField.PSNProfilesLink => NewLinkButtonAccessor(context),
                    GameField.Owner => CreateAccountAccessor(context),
                    _ => base.CreateOtherAccessor(context),
                },
                _ => context.Name == "Link" ? NewLinkButtonAccessor(context) : base.CreateOtherAccessor(context)
            };

        private IControlAccessor CreateAccountAccessor(IBuilderContext<GameField, Game> context)
        {
            context.AdditionalContext = new AccountAccessorParameters()
            {
                UseNullable = true,
                OnlyNullable = true
            };
            return new AccountAccessor<GameField, Game>(context);
        }

        private static IControlAccessor NewLinkButtonAccessor(IBuilderContext<GameField, Game> context) => 
            new LinkButtonAccessor<GameField, Game>(context);

        protected override IControlAccessor CreateViewAccessor(IBuilderContext<GameField, Game> context) => 
            context is FieldContext<GameField, Game> accessorContext
                ? accessorContext.Field switch
                    {
                        GameField.Links => NewLinkAccessor(context),
                        GameField.StrategeLink or GameField.PSNProfilesLink => NewLinkButtonAccessor(context),
                        _ => base.CreateViewAccessor(context),
                    }
                : context.Name == "Link" 
                    ? NewLinkButtonAccessor(context) 
                    : base.CreateViewAccessor(context);

        private static IControlAccessor NewLinkAccessor(IBuilderContext<GameField, Game> context) => 
            context.Scope switch
            {
                ControlScope.Editor =>
                    new CustomControlAccessor<GameField, Game, LinksListControl, ListDAO<Link>>(context).Init(),
                ControlScope.BatchUpdate or
                ControlScope.QuickFilter =>
                    new ButtonEditAccessor<GameField, Game, ListDAO<Link>, Link, LinksListControl>(context).Init(),
                ControlScope.FullInfoView =>
                    new LinkButtonListAccessor(context, ButtonListDirection.Horizontal),
                _ =>
                    new LinkButtonListAccessor(context, ButtonListDirection.Vertical),
            };

        protected override IInitializer? Initializer(IBuilderContext<GameField, Game> context)
        {
            if (context is FieldContext<GameField, Game> accessorContext)
            {
                if (accessorContext.Field == GameField.CriticScore)
                    return new NumericInitializer(-1, 100);

                if (context.IsQuickFilter &&
                    (accessorContext.Field == GameField.Year))
                {
                    object? variant = BuilderVariant(context.Builder);
                    return new ExtractInitializer<GameField, Game>(accessorContext.Field, true,
                        variant != null && variant.Equals(QuickFilterVariant.Export));
                }
            }

            if (context.Name == "LinkName")
                return new LinkNameInitializer(null);
            else
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
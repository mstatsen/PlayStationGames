using OxXMLEngine.ControlFactory;
using OxXMLEngine.ControlFactory.Accessors;
using OxXMLEngine.ControlFactory.Context;
using OxXMLEngine.ControlFactory.Filter;
using OxXMLEngine.ControlFactory.Initializers;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Fields;
using OxXMLEngine.Data.Filter;
using OxXMLEngine.Data.Types;
using OxXMLEngine.Grid;
using OxXMLEngine.View;
using PlayStationGames.Engine.ControlFactory.Initializers;
using PlayStationGames.GameEngine.ControlFactory.Accessors;
using PlayStationGames.GameEngine.ControlFactory.Controls;
using PlayStationGames.GameEngine.ControlFactory.Filter;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;
using PlayStationGames.GameEngine.Grid;
using PlayStationGames.GameEngine.View;

namespace PlayStationGames.GameEngine.ControlFactory
{
    public class GameControlFactory : ControlFactory<GameField, Game>
    {
        protected override IControlAccessor? CreateOtherAccessor(IBuilderContext<GameField, Game> context)
        {
            if (context is FieldContext<GameField, Game> accessorContext)
                switch (accessorContext.Field)
                {
                    case GameField.PlatformFamily:
                        return CreateEnumAccessor<PlatformFamily>(context);
                    case GameField.Platform:
                        return CreateEnumAccessor<PlatformType>(context);
                    case GameField.ScreenView:
                        return CreateEnumAccessor<ScreenView>(context);
                    case GameField.Format:
                        return CreateEnumAccessor<GameFormat>(context);
                    case GameField.Source:
                        return CreateEnumAccessor<Source>(context);
                    case GameField.Region:
                        return CreateEnumAccessor<GameRegion>(context);
                    case GameField.Language:
                        return CreateEnumAccessor<GameLanguage>(context);
                    case GameField.Pegi:
                        return CreateEnumAccessor<Pegi>(context);
                    case GameField.CompleteTime:
                        return CreateEnumAccessor<CompleteTime>(context);
                    case GameField.Difficult:
                        return CreateEnumAccessor<Difficult>(context);
                    case GameField.Status:
                        return CreateEnumAccessor<Status>(context);
                    case GameField.TrophysetAccess:
                        return CreateEnumAccessor<TrophysetAccessibility>(context);
                    case GameField.Year:
                        return new YearAccessor<GameField, Game>(context);
                    case GameField.GameModes:
                        return CreateListAccessor<GameMode, ListDAO<GameMode>, GameModesControl>(context, ControlScope.Editor);
                    case GameField.Dlcs:
                        return CreateListAccessor<DLC, ListDAO<DLC>, DLCListControl>(context, ControlScope.Editor);
                    case GameField.Installations:
                        return CreateListAccessor<Installation, ListDAO<Installation>, InstallationsControl>(context, ControlScope.Editor);
                    case GameField.Links:
                        return NewLinkAccessor(context);
                    case GameField.RelatedGames:
                        return CreateListAccessor<RelatedGame, RelatedGames, RelatedGamesControl>(context);
                    case GameField.ReleasePlatforms:
                        return CreateListAccessor<Platform, Platforms, ReleasePlatformListControl>(context);
                    case GameField.Id:
                        return CreateLabelAccessor(context);
                    case GameField.StrategeLink:
                    case GameField.PSNProfilesLink:
                        return NewLinkButtonAccessor(context);
                }
            else
                if (context.Name == "Link")
                    return NewLinkButtonAccessor(context);

            return base.CreateOtherAccessor(context);
        }

        private static IControlAccessor NewLinkButtonAccessor(IBuilderContext<GameField, Game> context) =>
            new LinkButtonAccessor<GameField, Game>(context);

        protected override IControlAccessor CreateViewAccessor(IBuilderContext<GameField, Game> context)
        {
            if (context is FieldContext<GameField, Game> accessorContext)
                switch (accessorContext.Field)
                {
                    case GameField.Links:
                        return NewLinkAccessor(context);
                    case GameField.StrategeLink:
                    case GameField.PSNProfilesLink:
                        return NewLinkButtonAccessor(context);
                }
            else
                if (context.Name == "Link")
                    return NewLinkButtonAccessor(context);

            return base.CreateViewAccessor(context);
        }

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

                if ((context.IsQuickFilter) &&
                    (accessorContext.Field == GameField.Year))
                {
                    object? variant = BuilderVariant(context.Builder);
                    return new ExtractInitializer<GameField, Game>(accessorContext.Field, true,
                        variant != null && variant.Equals(QuickFilterVariant.Export));
                }
            }

            if (context.Name == "LinkName")
                return new LinkNameInitializer(null);

            return base.Initializer(context);
        }

        public override IItemView<GameField, Game> CreateInfoCard() =>
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
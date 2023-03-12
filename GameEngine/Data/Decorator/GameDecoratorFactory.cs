using OxXMLEngine.Data.Decorator;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    public class GameDecoratorFactory : DecoratorFactory<GameField, Game>
    {
        protected override Decorator<GameField, Game> HTML(Game dao) =>
            new GameHtmlDecorator(dao);

        protected override Decorator<GameField, Game> Simple(Game dao) =>
            new GameDecorator(dao);

        protected override Decorator<GameField, Game> Table(Game dao) =>
            new GameTableDecorator(dao);

        protected override Decorator<GameField, Game> FullInfo(Game dao) =>
            new GameFullInfoDecorator(dao);

        protected override Decorator<GameField, Game> Icon(Game dao) =>
            new GameIconDecorator(dao);

        protected override Decorator<GameField, Game> Card(Game dao) =>
            new GameCardDecorator(dao);
    }
}

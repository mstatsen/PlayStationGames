using OxDAOEngine.Data.Decorator;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    internal class GameDecorator : Decorator<GameField, Game>
    {
        public GameDecorator(Game dao) : base(dao){ }

        public override object? Value(GameField field) => 
            field switch
            {
                GameField.Field => null,
                _ => Dao[field]
            };
    }
}
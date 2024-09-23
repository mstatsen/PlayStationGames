using OxLibrary;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    internal class GameIconDecorator : GameCardDecorator
    {
        public GameIconDecorator(Game game) : base(game) { }

        public override object? Value(GameField field) => 
            field switch
            {
                GameField.Image => Dao.Image ?? OxIcons.Close,
                _ => base.Value(field),
            };
    }
}
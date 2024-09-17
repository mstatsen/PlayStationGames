using OxDAOEngine.Data.Decorator;
using PlayStationGames.ConsoleEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.Data.Decorator
{
    internal class ConsoleDecorator : Decorator<ConsoleField, PSConsole>
    {
        public ConsoleDecorator(PSConsole dao) : base(dao){ }

        public override object? Value(ConsoleField field) => Dao[field];
    }
}
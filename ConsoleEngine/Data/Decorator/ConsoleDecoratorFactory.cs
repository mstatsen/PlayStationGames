using OxXMLEngine.Data.Decorator;
using PlayStationGames.ConsoleEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.Data.Decorator
{
    public class ConsoleDecoratorFactory : DecoratorFactory<ConsoleField, PSConsole>
    {
        protected override Decorator<ConsoleField, PSConsole> Simple(PSConsole dao) =>
            new ConsoleDecorator(dao);

        protected override Decorator<ConsoleField, PSConsole> Table(PSConsole dao) => 
            new ConsoleTableDecorator(dao);

        protected override Decorator<ConsoleField, PSConsole> Card(PSConsole dao) =>
            new ConsoleCardDecorator(dao);

        protected override Decorator<ConsoleField, PSConsole> FullInfo(PSConsole dao) =>
            new ConsoleFullInfoDecorator(dao);

        protected override Decorator<ConsoleField, PSConsole> HTML(PSConsole dao) =>
            new ConsoleHtmlDecorator(dao);
    }
}
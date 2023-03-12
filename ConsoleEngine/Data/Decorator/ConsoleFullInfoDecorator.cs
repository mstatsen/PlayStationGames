using PlayStationGames.ConsoleEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.Data.Decorator
{
    internal class ConsoleFullInfoDecorator : ConsoleCardDecorator
    {
        public ConsoleFullInfoDecorator(PSConsole dao) : base(dao) { }

        public override object? Value(ConsoleField field) => 
            field switch
            {
                ConsoleField.Storages => StoragesText(),
                _ => base.Value(field),
            };

        private object? StoragesText()
        {
            string result = string.Empty;

            foreach (Storage storage in Dao.Storages)
                result += $"{storage.Name} ({storage.Size} Gb, {storage.GameCount} games)\n";

            return result;
        }
    }
}
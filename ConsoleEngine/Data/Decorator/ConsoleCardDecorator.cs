using PlayStationGames.ConsoleEngine.Data.Fields;
using OxXMLEngine.Data.Types;
using OxLibrary;

namespace PlayStationGames.ConsoleEngine.Data.Decorator
{
    internal class ConsoleCardDecorator : ConsoleTableDecorator
    {
        public ConsoleCardDecorator(PSConsole dao) : base(dao){ }
        public override object? Value(ConsoleField field)
        {
            if (TypeHelper.IsTypeHelpered(Dao[field]))
                return TypeHelper.Name(Dao[field]);

            return field switch
            {
                ConsoleField.Icon =>
                    OxImageBoxer.BoxingImage(Dao.Icon, new Size(80, 33)),
                ConsoleField.Storages => 
                    NormalizeIfEmpty(Dao.Storages.ToString()),
                ConsoleField.Folders => 
                    NormalizeIfEmpty(Dao.Folders.ToString()),
                _ => base.Value(field),
            };
        }
    }
}
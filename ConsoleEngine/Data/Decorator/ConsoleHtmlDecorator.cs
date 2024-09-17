using OxLibrary;
using OxDAOEngine.Data.Decorator;
using OxDAOEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;

namespace PlayStationGames.ConsoleEngine.Data.Decorator
{
    internal class ConsoleHtmlDecorator : ConsoleDecorator
    {
        public ConsoleHtmlDecorator(PSConsole dao) : base(dao) { }

        public override object? Value(ConsoleField field) => 
            field switch
            {
                ConsoleField.Name => 
                    DecoratorHelper.NoWrap(Dao.Name),
                ConsoleField.Games =>
                    Games(),
                ConsoleField.Icon => 
                    Icon(),
                ConsoleField.Model =>
                    TypeHelper.Name(Dao.Model),
                _ => 
                    base.Value(field),
            };

        private string Games()
        {
            List<string?> gamesStringList = new();

            foreach (Game game in Dao.Games())
                gamesStringList.Add(game.ToString());

            return DecoratorHelper.ListToString(gamesStringList, ", ");
        }

        public override string Attributes(ConsoleField field)
        {
            List<string?> attributesList = new();
            switch (field)
            {
                case ConsoleField.Icon:
                    attributesList.Add(string.Format(Attr_Width, 140));
                    break;
            }

            attributesList.Add(string.Format(Attr_Align, Align(field)));

            for (int i = 0; i < attributesList.Count; i++)
                attributesList[i] = $" {attributesList[i]}";

            return DecoratorHelper.ListToString(attributesList, " ", false);
        }

        private object Icon()
        {
            string calcedImageBase64 = OxBase64.BitmapToBase64(
                (Bitmap?)OtherDecorator(DecoratorType.Card)[ConsoleField.Icon]
            );
            return $"<img src =\"data: image/png; base64, {calcedImageBase64}\" />";
        }

        private static string Align(ConsoleField field) =>
            "left";

        private const string Attr_Align = "align = {0}";
        private const string Attr_Width = "width = {0}";
    }
}
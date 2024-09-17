using OxLibrary;
using OxDAOEngine.Data.Decorator;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.Data.Decorator
{
    internal class AccountHtmlDecorator : AccountDecorator
    {
        public AccountHtmlDecorator(Account dao) : base(dao) { }

        public override object? Value(AccountField field) => 
            field switch
            {
                AccountField.Name => 
                    DecoratorHelper.NoWrap(Dao.Name),
                AccountField.Consoles =>
                    Consoles(),
                AccountField.Games =>
                    Games(),
                AccountField.Avatar => 
                    Avatar(),
                _ => 
                    base.Value(field),
            };

        private string Consoles()
        {
            List<string?> consolesStringList = new();

            /*TODO:
            foreach (Game game in Dao.Games())
                gamesStringList.Add(game.ToString());
            */

            return DecoratorHelper.ListToString(consolesStringList, ", ");
        }

        private string Games()
        {
            List<string?> gamesStringList = new();

            /*TODO:
            foreach (Game game in Dao.Games())
                gamesStringList.Add(game.ToString());
            */

            return DecoratorHelper.ListToString(gamesStringList, ", ");
        }

        public override string Attributes(AccountField field)
        {
            List<string?> attributesList = new();
            switch (field)
            {
                case AccountField.Avatar:
                    attributesList.Add(string.Format(Attr_Width, 140));
                    break;
            }

            attributesList.Add(string.Format(Attr_Align, Align(field)));

            for (int i = 0; i < attributesList.Count; i++)
                attributesList[i] = $" {attributesList[i]}";

            return DecoratorHelper.ListToString(attributesList, " ", false);
        }

        private object Avatar()
        {
            string calcedImageBase64 = OxBase64.BitmapToBase64(
                (Bitmap?)OtherDecorator(DecoratorType.Card)[AccountField.Avatar]
            );
            return $"<img src =\"data: image/png; base64, {calcedImageBase64}\" />";
        }

        private static string Align(AccountField field) =>
            "left";

        private const string Attr_Align = "align = {0}";
        private const string Attr_Width = "width = {0}";
    }
}
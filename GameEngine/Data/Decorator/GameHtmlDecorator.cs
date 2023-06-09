﻿using OxLibrary;
using OxXMLEngine.Data.Decorator;
using OxXMLEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    internal class GameHtmlDecorator : GameDecorator
    {
        public GameHtmlDecorator(Game dao) : base(dao) { }

        public override object? Value(GameField field) => 
            field switch
            {
                GameField.Name => 
                    DecoratorHelper.NoWrap(Dao.Name),
                GameField.Image => 
                    Image(),
                GameField.ReleasePlatforms => 
                    ListToString(Dao.ReleasePlatforms.StringList),
                GameField.Difficult => 
                    TypeHelper.Name(Dao.Difficult),
                GameField.CompleteTime => 
                    DecoratorHelper.NoWrap(TypeHelper.FullName(Dao.CompleteTime)),
                GameField.ScreenView => 
                    ScreenView(),
                GameField.Dlcs => 
                    ListToHtmlColumn(Dao.Dlcs.StringList),
                GameField.Links => 
                    Links(),
                GameField.RelatedGames => 
                    ListToHtmlColumn(Dao.RelatedGames.StringList),
                GameField.GameModes => 
                    ListToHtmlColumn(Dao.GameModes.StringList),
                GameField.TrophysetAccess => 
                    TrophysetAccessibility(),
                GameField.FullGenre => 
                    CalcedGenre(),
                GameField.Progress => 
                    $"{base.Value(field)}%",
                GameField.FullPlatinum or 
                GameField.FullGold or 
                GameField.FullSilver or 
                GameField.FullBronze or 
                GameField.FullFromDLC or 
                GameField.FullNet or 
                GameField.Pegi => 
                    DecoratorHelper.NoWrap(
                        OtherDecorator(DecoratorType.Card)[field]?.ToString()
                    ),
                GameField.Status => 
                    CalcedStatus(),
                GameField.StrategeLink or 
                GameField.PSNProfilesLink or 
                GameField.Verified =>
                    OtherDecorator(DecoratorType.Table)[field],
                _ => 
                    base.Value(field),
            };

        public override string Attributes(GameField field)
        {
            List<string?> attributesList = new();
            switch (field)
            {
                case GameField.Image:
                    attributesList.Add(string.Format(Attr_Width, 140));
                    break;
                case GameField.ReleasePlatforms:
                    attributesList.Add(string.Format(Attr_Width, 120));
                    break;
                case GameField.FullGenre:
                    attributesList.Add(string.Format(Attr_Width, 160));
                    break;
            }

            attributesList.Add(string.Format(Attr_Align, Align(field)));

            for (int i = 0; i < attributesList.Count; i++)
                attributesList[i] = $" {attributesList[i]}";

            return DecoratorHelper.ListToString(attributesList, " ", false);
        }

        private object CalcedStatus() =>
            DecoratorHelper.NoWrap(
                OtherDecorator(DecoratorType.Table)[GameField.Status]?.ToString()
            );

        private object Links()
        {
            List<string?> list = new();

            foreach (Link link in Dao.Links)
            {
                string fontColor = ColorTranslator.ToHtml(new OxColorHelper(link.LinkColor).HDarker(1).Bluer(2));
                list.Add(
                    $"<a"+
                    $" style=\"color:{fontColor}; text-decoration:None;\"" +
                    $" target = \"_blank\"" +
                    $" href=\"{link.Url}\">{link.Name}</a>"
                );
            }

            return ListToHtmlColumn(list, false);
        }

        private static string ListToString(List<string?> list, bool noWrapItems = true) =>
            DecoratorHelper.ListToString(list, ", ", noWrapItems);

        private static string ListToHtmlColumn(List<string?> list, bool noWrapItems = true) =>
            DecoratorHelper.ListToString(list, "<br>", noWrapItems);

        private object TrophysetAccessibility() =>
            DecoratorHelper.NoWrap(TypeHelper.Name(Dao.TrophysetAccess));

        private object ScreenView() =>
            DecoratorHelper.NoWrap(TypeHelper.Name(Dao.ScreenView));

        private object CalcedGenre() =>
            DecoratorHelper.NoWrap($"{ScreenView()} {Dao.Genre}");

        private object Image()
        {
            string calcedImageBase64 = OxBase64.BitmapToBase64(
                (Bitmap?)OtherDecorator(DecoratorType.Card)[GameField.Image]
            );
            return $"<img src =\"data: image/png; base64, {calcedImageBase64}\" />";
        }

        private static string Align(GameField field) =>
            field switch
            {
                GameField.Image or 
                GameField.CriticScore or 
                GameField.Platform or 
                GameField.Format or 
                GameField.EarnedPlatinum or 
                GameField.EarnedGold or 
                GameField.EarnedSilver or 
                GameField.EarnedBronze or 
                GameField.EarnedFromDLC or 
                GameField.EarnedNet or 
                GameField.AvailablePlatinum or 
                GameField.AvailableGold or 
                GameField.AvailableSilver or 
                GameField.AvailableBronze or 
                GameField.AvailableFromDLC or 
                GameField.AvailableNet or 
                GameField.Source or 
                GameField.Year or 
                GameField.Pegi or 
                GameField.Difficult or 
                GameField.CompleteTime or 
                GameField.ScreenView or 
                GameField.Links or
                GameField.Tags or
                GameField.TrophysetAccess or 
                GameField.Progress or 
                GameField.FullPlatinum or 
                GameField.FullGold or 
                GameField.FullSilver or 
                GameField.FullBronze or 
                GameField.FullFromDLC or 
                GameField.FullNet or 
                GameField.Status or 
                GameField.EarnedPoints or 
                GameField.EarnedPointsOld or 
                GameField.StrategeLink or 
                GameField.PSNProfilesLink => 
                    "center",
                _ => 
                    "left",
            };

        private const string Attr_Align = "align = {0}";
        private const string Attr_Width = "width = {0}";
    }
}
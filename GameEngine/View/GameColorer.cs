﻿using OxLibrary;
using OxXMLEngine.Data.Types;
using OxXMLEngine.View;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.View
{
    public class GameColorer : ItemColorer<GameField, Game>
    {
        public override Color BaseColor(Game? item) =>
            new OxColorHelper(TypeHelper.BackColor(item?.SourceType)).Darker(7);

        public override Color ForeColor(Game? item) =>
            TypeHelper.FontColor(item?[GameField.Status]);
    }
}
using System.Collections.Generic;

namespace MyBot.Bomberjam
{
    public static class Constants
    {
        public const string Up = "up";
        public const string Down = "down";
        public const string Left = "left";
        public const string Right = "right";
        public const string Stay = "stay";
        public const string Bomb = "bomb";
        public const string Fire = "fire";

        public const char Empty = '.';
        public const char Wall = '#';
        public const char Block = '+';
        public const char Explosion = '*';

        public const string TopLeft = "tl";
        public const string TopRight = "tr";
        public const string BottomLeft = "bl";
        public const string BottomRight = "br";

        public static readonly IReadOnlyDictionary<ActionKind, string> ActionKindToActionStringMappings = new Dictionary<ActionKind, string>
        {
            { ActionKind.Up, Up },
            { ActionKind.Down, Down },
            { ActionKind.Left, Left },
            { ActionKind.Right, Right },
            { ActionKind.Stay, Stay },
            { ActionKind.Bomb, Bomb }
        };

        public static readonly IReadOnlyDictionary<char, TileKind> TileCharToTileKindMappings = new Dictionary<char, TileKind>
        {
            { Empty, TileKind.Empty },
            { Wall, TileKind.Wall },
            { Block, TileKind.Block },
            { Explosion, TileKind.Explosion }
        };
    }
}
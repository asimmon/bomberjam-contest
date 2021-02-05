using System.Collections.Generic;
using System.Linq;
using Bomberjam.Common;

namespace Bomberjam
{
    internal static class Translator
    {
        public static readonly IReadOnlyDictionary<char, TileKind> CharacterToTileKindMappings = new Dictionary<char, TileKind>
        {
            [Constants.Empty] = TileKind.Empty,
            [Constants.Wall] = TileKind.Wall,
            [Constants.Block] = TileKind.Block,
            [Constants.Explosion] = TileKind.Explosion,
        };

        public static readonly IReadOnlyDictionary<TileKind, char> TileKindToCharacterMappings = CharacterToTileKindMappings.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        public static readonly IReadOnlyDictionary<BonusKind, string> ToBonusKindToStringMappings = new Dictionary<BonusKind, string>
        {
            [BonusKind.Bomb] = Constants.Bomb,
            [BonusKind.Fire] = Constants.Fire,
        };

        public static readonly IReadOnlyDictionary<string, ActionCode> StringToActionCodeMappings = new Dictionary<string, ActionCode>
        {
            [Constants.Up] = ActionCode.Up,
            [Constants.Down] = ActionCode.Down,
            [Constants.Left] = ActionCode.Left,
            [Constants.Right] = ActionCode.Right,
            [Constants.Stay] = ActionCode.Stay,
            [Constants.Bomb] = ActionCode.Bomb,
        };

        public static readonly IReadOnlyDictionary<Corner, string> CornerToStringMappings = new Dictionary<Corner, string>
        {
            [Corner.TopLeft] = Constants.TopLeft,
            [Corner.TopRight] = Constants.TopRight,
            [Corner.BottomLeft] = Constants.BottomLeft,
            [Corner.BottomRight] = Constants.BottomRight,
        };
    }
}
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Bomberjam
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

        // Only accept process messages that looks like 5:up, 0:mypseudo, etc.
        internal static readonly Regex ProcessMessageRegex = new Regex(
            "^(?<tick>[0-9]{1,5}):(?<message>[a-zA-Z0-9\\-_]{1,32})$",
            RegexOptions.Compiled);

        internal static readonly JsonWriterOptions DefaultJsonWriterOptions = new JsonWriterOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }
}
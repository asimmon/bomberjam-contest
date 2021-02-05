using System.Text.Json;

namespace Bomberjam.Common
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

        public static readonly JsonWriterOptions DefaultJsonWriterOptions = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new()
        {
            Encoder = DefaultJsonWriterOptions.Encoder
        };
    }
}
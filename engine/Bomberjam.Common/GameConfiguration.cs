using System;
using System.Text.Json.Serialization;

namespace Bomberjam.Common
{
    public class GameConfiguration
    {
        private static readonly Func<int, bool> GreaterThanZero = n => n > 0;
        private static readonly Func<int, bool> AnyInteger = _ => true;
        private static readonly Func<bool, bool> AnyBoolean = _ => true;

        [JsonPropertyName("defaultBombRange")]
        public int? DefaultBombRange { get; set; } = 2;

        [JsonPropertyName("defaultBombCountDown")]
        public int? DefaultBombCountDown { get; set; } = 8;

        [JsonPropertyName("defaultBombBonusCount")]
        public int? DefaultBombBonusCount { get; set; } = 8;

        [JsonPropertyName("defaultFireBonusCount")]
        public int? DefaultFireBonusCount { get; set; } = 8;

        [JsonPropertyName("suddenDeathCountdown")]
        public int? SuddenDeathCountdown { get; set; } = 240;

        [JsonPropertyName("respawnTime")]
        public int? RespawnTime { get; set; } = 10;

        [JsonPropertyName("shufflePlayerPositions")]
        public bool? ShufflePlayerPositions { get; set; } = true;

        [JsonPropertyName("pointsPerAliveTick")]
        public int? PointsPerAliveTick { get; set; } = 0;

        [JsonPropertyName("pointsBlockDestroyed")]
        public int? PointsBlockDestroyed { get; set; } = 5;

        [JsonPropertyName("pointsKilledPlayer")]
        public int? PointsKilledPlayer { get; set; } = 20;

        [JsonPropertyName("pointsDeath")]
        public int? PointsDeath { get; set; } = -10;

        [JsonPropertyName("pointsLastSurvivor")]
        public int? PointsLastSurvivor { get; set; } = 50;

        public GameConfiguration()
        {
        }

        public GameConfiguration(GameConfiguration? other)
        {
            if (other != null)
            {
                this.Merge(other);
            }
        }

        private void Merge(GameConfiguration other)
        {
            OverrideProperty(x => this.DefaultBombRange = x, other.DefaultBombRange, GreaterThanZero);
            OverrideProperty(x => this.DefaultBombCountDown = x, other.DefaultBombCountDown, GreaterThanZero);
            OverrideProperty(x => this.DefaultBombBonusCount = x, other.DefaultBombBonusCount, GreaterThanZero);
            OverrideProperty(x => this.DefaultFireBonusCount = x, other.DefaultFireBonusCount, GreaterThanZero);
            OverrideProperty(x => this.SuddenDeathCountdown = x, other.SuddenDeathCountdown, GreaterThanZero);
            OverrideProperty(x => this.RespawnTime = x, other.RespawnTime, GreaterThanZero);
            OverrideProperty(x => this.ShufflePlayerPositions = x, other.ShufflePlayerPositions, AnyBoolean);
            OverrideProperty(x => this.PointsPerAliveTick = x, other.PointsPerAliveTick, AnyInteger);
            OverrideProperty(x => this.PointsBlockDestroyed = x, other.PointsBlockDestroyed, AnyInteger);
            OverrideProperty(x => this.PointsKilledPlayer = x, other.PointsKilledPlayer, AnyInteger);
            OverrideProperty(x => this.PointsDeath = x, other.PointsDeath, AnyInteger);
            OverrideProperty(x => this.PointsLastSurvivor = x, other.PointsLastSurvivor, AnyInteger);
        }

        private static void OverrideProperty<T>(Action<T> setter, T? value, Func<T, bool> asserter) where T : struct
        {
            if (value.HasValue && asserter(value.Value))
            {
                setter(value.Value);
            }
        }
    }
}
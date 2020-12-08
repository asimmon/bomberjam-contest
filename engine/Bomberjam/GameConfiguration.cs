using System;
using Newtonsoft.Json;

namespace Bomberjam
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GameConfiguration
    {
        private static readonly Func<int, bool> GreaterThanZero = n => n > 0;
        private static readonly Func<int, bool> AnyInteger = _ => true;
        private static readonly Func<bool, bool> AnyBoolean = _ => true;

        [JsonProperty("defaultBombRange")]
        public int? DefaultBombRange { get; set; } = 2;

        [JsonProperty("defaultBombCountDown")]
        public int? DefaultBombCountDown { get; set; } = 8;

        [JsonProperty("defaultBombBonusCount")]
        public int? DefaultBombBonusCount { get; set; } = 8;

        [JsonProperty("defaultFireBonusCount")]
        public int? DefaultFireBonusCount { get; set; } = 8;

        [JsonProperty("suddenDeathCountdown")]
        public int? SuddenDeathCountdown { get; set; } = 240;

        [JsonProperty("respawnTime")]
        public int? RespawnTime { get; set; } = 10;

        [JsonProperty("shufflePlayerPositions")]
        public bool? ShufflePlayerPositions { get; set; } = true;

        [JsonProperty("pointsPerAliveTick")]
        public int? PointsPerAliveTick { get; set; } = 0;

        [JsonProperty("pointsBlockDestroyed")]
        public int? PointsBlockDestroyed { get; set; } = 5;

        [JsonProperty("pointsKilledPlayer")]
        public int? PointsKilledPlayer { get; set; } = 20;

        [JsonProperty("pointsDeath")]
        public int? PointsDeath { get; set; } = -10;

        [JsonProperty("pointsLastSurvivor")]
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

        internal void Merge(GameConfiguration other)
        {
            this.OverrideProperty(x => this.DefaultBombRange = x, other.DefaultBombRange, GreaterThanZero);
            this.OverrideProperty(x => this.DefaultBombCountDown = x, other.DefaultBombCountDown, GreaterThanZero);
            this.OverrideProperty(x => this.DefaultBombBonusCount = x, other.DefaultBombBonusCount, GreaterThanZero);
            this.OverrideProperty(x => this.DefaultFireBonusCount = x, other.DefaultFireBonusCount, GreaterThanZero);
            this.OverrideProperty(x => this.SuddenDeathCountdown = x, other.SuddenDeathCountdown, GreaterThanZero);
            this.OverrideProperty(x => this.RespawnTime = x, other.RespawnTime, GreaterThanZero);
            this.OverrideProperty(x => this.ShufflePlayerPositions = x, other.ShufflePlayerPositions, AnyBoolean);
            this.OverrideProperty(x => this.PointsPerAliveTick = x, other.PointsPerAliveTick, AnyInteger);
            this.OverrideProperty(x => this.PointsBlockDestroyed = x, other.PointsBlockDestroyed, AnyInteger);
            this.OverrideProperty(x => this.PointsKilledPlayer = x, other.PointsKilledPlayer, AnyInteger);
            this.OverrideProperty(x => this.PointsDeath = x, other.PointsDeath, AnyInteger);
            this.OverrideProperty(x => this.PointsLastSurvivor = x, other.PointsLastSurvivor, AnyInteger);
        }

        private void OverrideProperty<T>(Action<T> setter, T? value, Func<T, bool> asserter) where T : struct
        {
            if (value.HasValue && asserter(value.Value))
            {
                setter(value.Value);
            }
        }
    }
}
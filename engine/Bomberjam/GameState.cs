using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Bomberjam.Common;

namespace Bomberjam
{
    internal class GameState : IToDto<State>
    {
        private static readonly IDictionary<Direction, Action<IHasXY>> DirectionPositionIncrementers = new Dictionary<Direction, Action<IHasXY>>
        {
            [Direction.Up] = pos => pos.SetXY(pos.X, pos.Y - 1),
            [Direction.Down] = pos => pos.SetXY(pos.X, pos.Y + 1),
            [Direction.Left] = pos => pos.SetXY(pos.X - 1, pos.Y),
            [Direction.Right] = pos => pos.SetXY(pos.X + 1, pos.Y)
        };

        private static readonly IDictionary<ActionCode, Action<IHasXY>> ActionCodePositionIncrementers = new Dictionary<ActionCode, Action<IHasXY>>
        {
            [ActionCode.Up] = pos => DirectionPositionIncrementers[Direction.Up](pos),
            [ActionCode.Down] = pos => DirectionPositionIncrementers[Direction.Down](pos),
            [ActionCode.Left] = pos => DirectionPositionIncrementers[Direction.Left](pos),
            [ActionCode.Right] = pos => DirectionPositionIncrementers[Direction.Right](pos),
            [ActionCode.Stay] = pos => { },
            [ActionCode.Bomb] = pos => { }
        };

        private static readonly int[] DefaultPlayerColors =
        {
            0xd81b60,
            0x1e88e5,
            0xffc107,
            0x00997f
        };

        private readonly HashSet<ExplosionPosition> _explosionPositions = new HashSet<ExplosionPosition>();
        private readonly IDictionary<int, BonusKind> _plannedBonuses = new Dictionary<int, BonusKind>();
        private readonly IList<StartingPosition> _startingPositions = new List<StartingPosition>();
        private readonly SuddenDeathPosition _suddenDeathPos = new SuddenDeathPosition();
        private readonly string _id;
        private readonly List<TileKind> _tiles;
        private readonly IDictionary<string, GamePlayer> _players;
        private readonly IDictionary<string, GameBomb> _bombs;
        private readonly IDictionary<string, GameBonus> _bonuses;
        private readonly GameConfiguration _configuration;

        private bool _endGameCleanupExecuted;
        private int _objectCounter;
        private bool _isFinished;
        private int _tick;
        private int _width;
        private int _height;
        private int _suddenDeathCountDown;
        private bool _isSuddenDeathEnabled;

        public GameState(IReadOnlyList<string> map, GameConfiguration? configuration)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            this._id = Guid.NewGuid().ToString("N");
            this._tiles = new List<TileKind>();
            this._players = new Dictionary<string, GamePlayer>();
            this._bombs = new Dictionary<string, GameBomb>();
            this._bonuses = new Dictionary<string, GameBonus>();
            this._configuration = new GameConfiguration(configuration);
            this._suddenDeathCountDown = this._configuration.SuddenDeathCountdown!.Value;
            this.History = new GameHistory(this._configuration);

            this.LoadAsciiMap(map);
            this.PlanStartingPositions();
            this.PlanBonusPositions();
            this.ExecuteTick(new Dictionary<string, PlayerAction>());
        }

        public GameHistory History { get; }

        private void LoadAsciiMap(IReadOnlyList<string> asciiMap)
        {
            if (!Extensions.IsValidAsciiMap(asciiMap))
                throw new ArgumentException("ASCII map is invalid", nameof(asciiMap));

            this._tiles.Clear();

            foreach (var asciiLine in asciiMap)
            foreach (var asciiChar in asciiLine)
                this._tiles.Add(Translator.CharacterToTileKindMappings[asciiChar]);

            this._width = asciiMap[0].Length;
            this._height = asciiMap.Count;
        }

        private void PlanStartingPositions()
        {
            var shuffledColors = DefaultPlayerColors.Shuffle().Take(4).ToList();

            this._startingPositions.Clear();
            this._startingPositions.Add(new StartingPosition(0, 0, Corner.TopLeft, shuffledColors[0]));
            this._startingPositions.Add(new StartingPosition(this._width - 1, 0, Corner.TopRight, shuffledColors[1]));
            this._startingPositions.Add(new StartingPosition(0, this._height - 1, Corner.BottomLeft, shuffledColors[2]));
            this._startingPositions.Add(new StartingPosition(this._width - 1, this._height - 1, Corner.BottomRight, shuffledColors[3]));

            if (this._configuration.ShufflePlayerPositions!.Value)
            {
                this._startingPositions.ShuffleInPlace();
            }
        }

        private void PlanBonusPositions()
        {
            var potentialBonusPositions = this._tiles
                .Where((tile, idx) => tile == TileKind.Block)
                .Select((tile, idx) => idx)
                .ToList();

            var bombBonusCount = this._configuration.DefaultBombBonusCount!.Value;
            var fireBonusCount = this._configuration.DefaultFireBonusCount!.Value;
            var totalBonusCount = bombBonusCount + fireBonusCount;

            // shrinking bonuses count to the size of the map
            if (totalBonusCount > potentialBonusPositions.Count)
            {
                var bombBonusRatio = 1f * bombBonusCount / totalBonusCount;
                var fireBonusRatio = 1f * fireBonusCount / totalBonusCount;

                bombBonusCount = (int)Math.Floor(potentialBonusPositions.Count * bombBonusRatio);
                fireBonusCount = (int)Math.Floor(potentialBonusPositions.Count * fireBonusRatio);
            }

            // TODO fair bonus positioning instead of random
            this._plannedBonuses.Clear();

            for (var i = 0; i < bombBonusCount; i++)
            {
                var idx = potentialBonusPositions.PickRandom();
                this._plannedBonuses[idx] = BonusKind.Bomb;
                potentialBonusPositions.Remove(idx);
            }

            for (var i = 0; i < fireBonusCount; i++)
            {
                var idx = potentialBonusPositions.PickRandom();
                this._plannedBonuses[idx] = BonusKind.Fire;
                potentialBonusPositions.Remove(idx);
            }
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || y < 0 || x >= this._width || y >= this._height;
        }

        private TileKind GetTileAt(int x, int y)
        {
            return this.IsOutOfBounds(x, y) ? TileKind.OutOfBound : this._tiles[this.CoordToTileIndex(x, y)];
        }

        private void SetTileAt(int x, int y, TileKind newTile)
        {
            if (!this.IsOutOfBounds(x, y)) this._tiles[this.CoordToTileIndex(x, y)] = newTile;
        }

        private int CoordToTileIndex(int x, int y)
        {
            return y * this._width + x;
        }

        private bool TryFindActiveBombAt(int x, int y, [NotNullWhen(true)] out GameBomb? bomb)
        {
            bomb = this._bombs.Values.FirstOrDefault(b => b.CountDown > 0 && b.X == x && b.Y == y);
            return bomb != null;
        }

        private bool TryFindAlivePlayerAt(int x, int y, [NotNullWhen(true)] out GamePlayer? player)
        {
            player = this._players.Values.FirstOrDefault(p => p.IsAlive && p.X == x && p.Y == y);
            return player != null;
        }

        private bool TryFindDroppedBonusAt(int x, int y, [NotNullWhen(true)] out GameBonus? bonus)
        {
            bonus = this._bonuses.Values.FirstOrDefault(b => b.X == x && b.Y == y);
            return bonus != null;
        }

        internal void AddPlayer(string id, string name, int? websitePlayerId)
        {
            if (this._players.Count >= this._startingPositions.Count)
                throw new Exception("More players than starting spots");

            if (this._players.ContainsKey(id))
                throw new Exception($"Player with ID {id} already exists");

            var startingPosition = this._startingPositions[this._players.Count];
            var player = new GamePlayer(id, name, startingPosition, this._configuration.DefaultBombRange!.Value);
            this._players.Add(player.Id, player);
            this.MovePlayerToItsSpawnLocation(player);
            this.History.AddPlayer(id, name, websitePlayerId);
        }

        public void ExecuteTick(IDictionary<string, PlayerAction> actions)
        {
            if (this._isFinished)
            {
                this.ExecuteEndGameCleanup();
            }
            else
            {
                this.AppendTickToHistory(actions);
                this.RespawnPlayers();
                this.UnleashSuddenDeath();
                this.RunBombs();
                this.ApplyPlayerMoves(actions);
                this.UpdateIsFinished();
                this.AddScorePerTick();

                if (this._isFinished)
                {
                    this.AppendTickToHistory(new Dictionary<string, PlayerAction>());
                }
            }

            this._tick++;
        }

        private void ExecuteEndGameCleanup()
        {
            if (!this._endGameCleanupExecuted)
            {
                this._bombs.Clear();
                this._endGameCleanupExecuted = true;
            }
        }

        private void AppendTickToHistory(IDictionary<string, PlayerAction> actions)
        {
            if (this._players.Count > 0)
            {
                this.History.AddState(this, actions);
            }
        }

        private void RespawnPlayers()
        {
            foreach (var (_, player) in this._players)
            {
                if (player.MustRespawn) this.RespawnPlayer(player);

                if (player.Respawning > 0) player.Respawning--;
            }
        }

        private void RespawnPlayer(GamePlayer player)
        {
            player.Respawning = this._configuration.RespawnTime!.Value;
            player.MustRespawn = false;
            this.MovePlayerToItsSpawnLocation(player);
        }

        private void MovePlayerToItsSpawnLocation(GamePlayer player)
        {
            var startingPos = this._startingPositions.Single(sp => sp.Corner == player.StartingCorner);
            this.MovePlayerToAvailableLocationAround(player, startingPos.X, startingPos.Y);
        }

        private void MovePlayerToAvailableLocationAround(GamePlayer player, int x, int y)
        {
            var maxRadius = Math.Max(this._width, this._height);

            for (var radius = 0; radius < maxRadius; radius++)
            {
                var minX = x - radius;
                var maxX = x + radius;
                var minY = y - radius;
                var maxY = y + radius;

                for (var oy = minY; oy <= maxY; oy++)
                for (var ox = minX; ox <= maxX; ox++)
                {
                    var tile = this.GetTileAt(ox, oy);

                    // cannot set a player location to an non-empty tile
                    if (!(tile == TileKind.Empty || tile == TileKind.Explosion))
                        continue;

                    // cannot set a player location to another alive player location
                    if (this.TryFindAlivePlayerAt(ox, oy, out var otherPlayer) && otherPlayer.Id != player.Id)
                        continue;

                    // cannot set a player location to an active bomb
                    if (this.TryFindActiveBombAt(ox, oy, out _))
                        continue;

                    // found a safe spot!
                    player.X = ox;
                    player.Y = oy;
                    return;
                }
            }
        }

        private void UnleashSuddenDeath()
        {
            if (this._suddenDeathCountDown > 0) this._suddenDeathCountDown--;

            if (this._suddenDeathCountDown <= 0)
            {
                if (!this._isSuddenDeathEnabled) this._isSuddenDeathEnabled = true;

                var idx = this.CoordToTileIndex(this._suddenDeathPos.X, this._suddenDeathPos.Y);
                this._tiles[idx] = TileKind.Wall;

                if (this.TryFindAlivePlayerAt(this._suddenDeathPos.X, this._suddenDeathPos.Y, out var victim))
                    KillPlayer(victim);

                if (this.TryFindActiveBombAt(this._suddenDeathPos.X, this._suddenDeathPos.Y, out var bomb))
                    this._bombs.Remove(bomb.Id);

                if (this.TryFindDroppedBonusAt(this._suddenDeathPos.X, this._suddenDeathPos.Y, out var bonus))
                    this._bonuses.Remove(bonus.Id);

                // walling bottom
                if (this._suddenDeathPos.Direction == Direction.Right && this._suddenDeathPos.X + 1 >= this._width - this._suddenDeathPos.Iteration)
                {
                    this._suddenDeathPos.Direction = Direction.Down;
                }
                // walling left
                else if (this._suddenDeathPos.Direction == Direction.Down && this._suddenDeathPos.Y + 1 >= this._height - this._suddenDeathPos.Iteration)
                {
                    this._suddenDeathPos.Direction = Direction.Left;
                }
                // walling up
                else if (this._suddenDeathPos.Direction == Direction.Left && this._suddenDeathPos.X - 1 < this._suddenDeathPos.Iteration)
                {
                    this._suddenDeathPos.Direction = Direction.Up;
                    this._suddenDeathPos.Iteration++;
                }
                // walling right
                else if (this._suddenDeathPos.Direction == Direction.Up && this._suddenDeathPos.Y - 1 < this._suddenDeathPos.Iteration)
                {
                    this._suddenDeathPos.Direction = Direction.Right;
                }

                DirectionPositionIncrementers[this._suddenDeathPos.Direction](this._suddenDeathPos);
            }
        }

        public void KillPlayer(string playerId)
        {
            if (this._players.TryGetValue(playerId, out var player))
            {
                KillPlayer(player);
            }
        }

        private static void KillPlayer(GamePlayer victim)
        {
            // The winner needs to stay alive to appear at the end of the game
            if (!victim.HasWon)
                victim.IsAlive = false;
        }

        private void RunBombs()
        {
            this.ComputeBombCountdownAndPlayerBombsLeft();

            // avoid handling bombs twice
            var visitedBombs = new HashSet<GameBomb>();
            var explosionChain = new Queue<Explosion>();
            var deletedBombIds = new HashSet<string>();
            var playersHits = new Dictionary<string, List<PlayerHit>>();
            var destroyedBlocks = new HashSet<DestroyedBlock>();

            this._explosionPositions.Clear();

            // 0) replace previous explosions with empty tiles
            for (var idx = 0; idx < this._tiles.Count; idx++)
                if (this._tiles[idx] == TileKind.Explosion)
                    this._tiles[idx] = TileKind.Empty;

            // 1) detect zero-countdown exploding bombs
            foreach (var (_, bomb) in this._bombs)
                // bomb explodes
                if (bomb.CountDown == 0)
                {
                    visitedBombs.Add(bomb);
                    explosionChain.Enqueue(new Explosion(bomb));
                }

            void PropagateExplosion(Explosion explosion, Action<IHasXY> posIncrementer)
            {
                var bomb = explosion.ExplodedBomb;
                var pos = new Position(bomb);

                this._explosionPositions.Add(new ExplosionPosition(pos.X, pos.Y, bomb.Owner));

                if (this.TryFindAlivePlayerAt(bomb.X, bomb.Y, out var victim))
                {
                    var playerHits = playersHits.GetOrAdd(victim.Id, () => new List<PlayerHit>());
                    playerHits.Add(new PlayerHit(bomb.Owner, victim, GetDistanceFrom(bomb, victim), explosion.CountdownWhenExploded));
                }

                for (var i = 1; i <= bomb.Range; i++)
                {
                    posIncrementer(pos);

                    var tile = this.GetTileAt(pos.X, pos.Y);

                    // destroy block and do not spread explosion beyond that
                    if (tile == TileKind.Block)
                    {
                        this._explosionPositions.Add(new ExplosionPosition(pos.X, pos.Y, bomb.Owner));
                        destroyedBlocks.Add(new DestroyedBlock(pos.X, pos.Y, bomb.Owner));
                        return;
                    }

                    // check if hitting another bomb / player / bonus
                    if (tile == TileKind.Empty || tile == TileKind.Explosion)
                    {
                        if (this.TryFindActiveBombAt(pos.X, pos.Y, out var otherBomb) && !visitedBombs.Contains(otherBomb))
                        {
                            visitedBombs.Add(otherBomb);
                            explosionChain.Enqueue(new Explosion(otherBomb));
                        }

                        if (this.TryFindAlivePlayerAt(pos.X, pos.Y, out victim))
                        {
                            var playerHits = playersHits.GetOrAdd(victim.Id, () => new List<PlayerHit>());
                            playerHits.Add(new PlayerHit(bomb.Owner, victim, GetDistanceFrom(bomb, victim), explosion.CountdownWhenExploded));
                        }

                        if (this.TryFindDroppedBonusAt(pos.X, pos.Y, out var bonus))
                            this._bonuses.Remove(bonus.Id);

                        this._explosionPositions.Add(new ExplosionPosition(pos.X, pos.Y, bomb.Owner));
                    }
                    else
                    {
                        // nothing to do on walls or out of bounds
                        return;
                    }
                }
            }

            // 2) propagate explosion and detonate other bombs on the way
            while (explosionChain.Count > 0)
            {
                var explosion = explosionChain.Dequeue();
                explosion.ExplodedBomb.CountDown = 0;

                // find other bombs that would explode and their victims
                PropagateExplosion(explosion, DirectionPositionIncrementers[Direction.Up]);
                PropagateExplosion(explosion, DirectionPositionIncrementers[Direction.Down]);
                PropagateExplosion(explosion, DirectionPositionIncrementers[Direction.Left]);
                PropagateExplosion(explosion, DirectionPositionIncrementers[Direction.Right]);
            }

            // 3) remove destroyed walls now otherwise too many walls could have been destroyed
            foreach (var destroyedBlock in destroyedBlocks)
            {
                this.SetTileAt(destroyedBlock.X, destroyedBlock.Y, TileKind.Empty);
                destroyedBlock.Destroyer.AddScore(this._configuration.PointsBlockDestroyed!.Value);

                // drop bonus if applicable
                var idx = this.CoordToTileIndex(destroyedBlock.X, destroyedBlock.Y);
                if (this._plannedBonuses.TryGetValue(idx, out var bonusType))
                {
                    this.DropBonus(destroyedBlock.X, destroyedBlock.Y, bonusType);
                    this._plannedBonuses.Remove(idx);
                }
            }

            // 4) apply damage to players
            foreach (var (_, playerHits) in playersHits)
            {
                var bestHit = playerHits
                    .OrderBy(hit => hit.BombCountdownWhenExploded)
                    .ThenBy(hit => hit.DistanceFromBombToVictim)
                    .First();

                this.HitPlayer(bestHit.Victim, bestHit.Attacker);
            }

            // 5) replace tiles with explosions
            foreach (var exploPos in this._explosionPositions)
            {
                var explosionIndex = this.CoordToTileIndex(exploPos.X, exploPos.Y);
                this._tiles[explosionIndex] = TileKind.Explosion;
            }

            // 6) remove exploded bombs
            foreach (var (bombId, bomb) in this._bombs)
                if (bomb.CountDown <= 0)
                    deletedBombIds.Add(bombId);

            foreach (var deletedBombId in deletedBombIds) this._bombs.Remove(deletedBombId);
        }

        private void ComputeBombCountdownAndPlayerBombsLeft()
        {
            var playerBombCounts = new Dictionary<string, int>();

            foreach (var (playerId, _) in this._players) playerBombCounts[playerId] = 0;

            foreach (var (_, bomb) in this._bombs)
                if (--bomb.CountDown >= 0)
                    playerBombCounts[bomb.Owner.Id]++;

            foreach (var (playerId, player) in this._players)
            {
                player.BombsLeft = player.MaxBombs - playerBombCounts[playerId];
                if (player.BombsLeft < 0) player.BombsLeft = 0;
            }
        }

        private static double GetDistanceFrom(IHasXY from, IHasXY to)
        {
            var a = from.X - to.X;
            var b = from.Y - to.Y;
            return Math.Sqrt(a * a + b * b);
        }


        private void DropBonus(int x, int y, BonusKind kind)
        {
            var bonusId = this._objectCounter++;
            var bonus = new GameBonus(bonusId.ToString(CultureInfo.InvariantCulture), x, y, kind);
            this._bonuses[bonus.Id] = bonus;
        }

        private void HitPlayer(GamePlayer victim, GamePlayer attacker)
        {
            if (victim.Respawning == 0 && !victim.MustRespawn)
            {
                victim.AddScore(this._configuration.PointsDeath!.Value);

                if (victim.Id != attacker.Id) attacker.AddScore(this._configuration.PointsKilledPlayer!.Value);

                if (this._isSuddenDeathEnabled)
                {
                    KillPlayer(victim);
                }
                else
                {
                    victim.MustRespawn = true;
                    victim.Respawning = this._configuration.RespawnTime!.Value;
                }
            }
        }

        private void ApplyPlayerMoves(IDictionary<string, PlayerAction> moves)
        {
            foreach (var (playerId, player) in this._players)
            {
                player.IsTimedOut = false;

                if (player.IsAlive && moves.TryGetValue(playerId, out var move))
                {
                    if (move.Action == ActionCode.Bomb)
                        this.DropBomb(player);
                    else
                        this.MovePlayer(player, move.Action);
                }
                else
                {
                    player.IsTimedOut = true;
                }
            }
        }

        private void DropBomb(GamePlayer player)
        {
            var hasEnoughBombs = player.BombsLeft > 0;
            var isRespawning = player.Respawning > 0;
            if (!hasEnoughBombs || isRespawning) return;

            if (!this.TryFindActiveBombAt(player.X, player.Y, out _))
            {
                var bombId = this._objectCounter++;
                var newBomb = new GameBomb(bombId.ToString(CultureInfo.InvariantCulture), player, this._configuration.DefaultBombCountDown!.Value);
                this._bombs[newBomb.Id] = newBomb;
                player.BombsLeft--;
            }
        }

        private void MovePlayer(GamePlayer player, ActionCode movement)
        {
            if (movement == ActionCode.Stay)
                return;

            if (player.MustRespawn || player.Respawning == this._configuration.RespawnTime - 1)
                return;

            var posIncrementer = ActionCodePositionIncrementers[movement];
            var nextPos = new Position(player);

            posIncrementer(nextPos);

            var nextTile = this.GetTileAt(nextPos.X, nextPos.Y);
            if (nextTile == TileKind.OutOfBound)
                return;

            if (nextTile == TileKind.Empty || nextTile == TileKind.Explosion)
            {
                if (this.TryFindAlivePlayerAt(nextPos.X, nextPos.Y, out _))
                    return;

                if (this.TryFindActiveBombAt(nextPos.X, nextPos.Y, out _))
                    return;

                if (this.TryFindDroppedBonusAt(nextPos.X, nextPos.Y, out var bonus))
                {
                    if (bonus.Kind == BonusKind.Bomb)
                        player.MaxBombs++;
                    else if (bonus.Kind == BonusKind.Fire) player.BombRange++;

                    this._bonuses.Remove(bonus.Id);
                }

                player.X = nextPos.X;
                player.Y = nextPos.Y;

                // just entered in an explosion
                foreach (var exploPos in this._explosionPositions)
                    if (exploPos.X == player.X && exploPos.Y == player.Y)
                        this.HitPlayer(player, exploPos.Attacker);
            }
        }

        private void UpdateIsFinished()
        {
            if (this._players.Count == 0)
            {
                return;
            }

            var alivePlayers = new List<GamePlayer>(this._players.Values.Where(p => p.IsAlive)).ToList();

            // game ended: all dead or only one player alive left
            if (alivePlayers.Count <= 1)
            {
                this._isFinished = true;

                if (alivePlayers.Count == 1)
                {
                    alivePlayers[0].HasWon = true;
                    alivePlayers[0].AddScore(this._configuration.PointsLastSurvivor!.Value);
                }
            }
        }

        private void AddScorePerTick()
        {
            var alivePlayers = this._players.Values.Where(p => p.IsAlive);
            foreach (var alivePlayer in alivePlayers) alivePlayer.AddScore(this._configuration.PointsPerAliveTick!.Value);
        }

        public State Convert()
        {
            return new State
            {
                Id = this._id,
                IsFinished = this._isFinished,
                Tick = this._tick,
                Width = this._width,
                Height = this._height,
                SuddenDeathCountdown = this._suddenDeathCountDown,
                IsSuddenDeathEnabled = this._isSuddenDeathEnabled,
                Tiles = new string(this._tiles.Select(t => Translator.TileKindToCharacterMappings[t]).ToArray()),
                Players = this._players.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Convert()),
                Bombs = this._bombs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Convert()),
                Bonuses = this._bonuses.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Convert()),
            };
        }

        private sealed class DestroyedBlock : Position
        {
            public DestroyedBlock(int x, int y, GamePlayer destroyer) : base(x, y)
            {
                this.Destroyer = destroyer;
            }

            public GamePlayer Destroyer { get; }

            public override void SetXY(int x, int y)
            {
                throw new InvalidOperationException("Cannot be moved");
            }
        }

        private sealed class ExplosionPosition : Position
        {
            public ExplosionPosition(int x, int y, GamePlayer attacker) : base(x, y)
            {
                this.Attacker = attacker;
            }

            public GamePlayer Attacker { get; }

            public override void SetXY(int x, int y)
            {
                throw new InvalidOperationException("Cannot be moved");
            }
        }

        private sealed class Explosion
        {
            public Explosion(GameBomb explodedBomb)
            {
                this.ExplodedBomb = explodedBomb;
                this.CountdownWhenExploded = explodedBomb.CountDown;
            }

            public GameBomb ExplodedBomb { get; }

            public int CountdownWhenExploded { get; }
        }

        private sealed class PlayerHit
        {
            public PlayerHit(GamePlayer attacker, GamePlayer victim, double distanceFromBombToVictim, int bombCountdownWhenExploded)
            {
                this.Attacker = attacker;
                this.Victim = victim;
                this.DistanceFromBombToVictim = distanceFromBombToVictim;
                this.BombCountdownWhenExploded = bombCountdownWhenExploded;
            }

            public GamePlayer Attacker { get; }

            public GamePlayer Victim { get; }

            public double DistanceFromBombToVictim { get; }

            public int BombCountdownWhenExploded { get; }
        }
    }
}
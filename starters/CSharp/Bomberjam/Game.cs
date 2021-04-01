using System;
using System.Globalization;
using System.Text.Json;

namespace MyBot.Bomberjam
{
    public class Game
    {
        private bool _isReady;
        private string _myPlayerId;
        private State _state;

        public Game()
        {
            this._isReady = false;
            this._myPlayerId = null;
            this._state = null;
        }

        public string MyPlayerId
        {
            get
            {
                this.EnsureIsReady();
                return this._myPlayerId;
            }
        }

        public State State
        {
            get
            {
                this.EnsureIsReady();
                this.EnsureInitialState();
                return this._state;
            }
        }

        public Player MyPlayer
        {
            get
            {
                this.EnsureIsReady();
                this.EnsureInitialState();
                return this._state.Players[this._myPlayerId];
            }
        }

        public void Ready(string playerName)
        {
            if (this._isReady)
                return;

            if (string.IsNullOrWhiteSpace(playerName))
                throw new ArgumentException("Your name cannot be null or empty");

            Console.Out.WriteLine("0:{0}", playerName);
            Console.Out.Flush();

            this._myPlayerId = Console.In.ReadLine();

            int parsedId;
            if (string.IsNullOrWhiteSpace(this._myPlayerId) || !int.TryParse(this._myPlayerId, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedId))
                throw new InvalidOperationException("Could not retrieve your ID from standard input");

            this._isReady = true;
        }

        public void ReceiveCurrentState()
        {
            this.EnsureIsReady();
            this._state = JsonSerializer.Deserialize<State>(Console.In.ReadLine());
        }

        public void SendAction(ActionKind action)
        {
            this.EnsureIsReady();
            this.EnsureInitialState();

            var actionStr = Constants.ActionKindToActionStringMappings[action];

            Console.Out.WriteLine("{0}:{1}", this._state.Tick.ToString(CultureInfo.InvariantCulture), actionStr);
            Console.Out.Flush();
        }

        private void EnsureIsReady()
        {
            if (!this._isReady)
                throw new InvalidOperationException("You need to call Game.Ready(...) with your name first");
        }

        private void EnsureInitialState()
        {
            if (this._state == null)
                throw new InvalidOperationException("You need to call Game.ReceiveCurrentState() to retrieve the initial state of the game");
        }
    }
}
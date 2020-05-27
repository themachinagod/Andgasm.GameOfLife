using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Andgasm.GameOfLife.Core.Abstractions
{
    public class GameManager : IGameManager
    {
        public event EventHandler GameTickProcessed;

        private readonly IGameConfiguration _gameConfiguration;
        private readonly IBoardManager _boardManager;
        private readonly Stopwatch _gameTimer = new Stopwatch();

        public int TickCount { get; set; }
        public TimeSpan GameElapsedTime
        {
            get
            {
                return _gameTimer.Elapsed;
            }
        }
        public TimeSpan AverageTickElapsedTime
        {
            get
            {
                return _boardManager.AverageTickElapsedTime;
            }
        }

        public GameManager(IGameConfiguration gameConfiguration, 
                           IBoardManager boardManager)
        {
            _gameConfiguration = gameConfiguration;
            _boardManager = boardManager;
        }

        public async Task StartGame(bool[,] seedBoard = null)
        {
            _gameTimer.Restart();
            if (seedBoard == null) _boardManager.InitialiseRandomBoard();
            else _boardManager.InitialiseFromSeed(seedBoard);
            GameTickProcessed?.Invoke(this, new EventArgs());
            TickCount++;

            do
            {
                _boardManager.ProcessBoardTick();
                if (_gameConfiguration.RaiseRenderEvents)
                {
                    GameTickProcessed?.Invoke(this, new EventArgs());
                    await Task.Delay(_gameConfiguration.TickDelay);
                }
                TickCount++;

            } while (TickCount <= _gameConfiguration.MaxTickCount);
            _gameTimer.Stop();
        }
    }
}

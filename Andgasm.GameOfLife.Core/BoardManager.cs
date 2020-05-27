using Andgasm.GameOfLife.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Andgasm.GameOfLife.Core
{
    public class BoardManager : IBoardManager
    {
        private readonly IBoardContext _boardContext;
        private readonly ICellStateRule _cellStateManager;
        private readonly IGameConfiguration _gameConfiguration;
        private readonly Stopwatch _processTimer = new Stopwatch();

        public TimeSpan AverageTickElapsedTime { get; set; }

        public BoardManager(IGameConfiguration gameConfiguration, 
                            IBoardContext boardContext, 
                            ICellStateRule cellStateManager)
        {
            _boardContext = boardContext;
            _gameConfiguration = gameConfiguration;
            _cellStateManager = cellStateManager;
        }

        public void InitialiseRandomBoard()
        {
            _boardContext.Height = _gameConfiguration.Height;
            _boardContext.Width = _gameConfiguration.Width;
            _boardContext.LoopEdges = _gameConfiguration.LoopEdges;

            var random = new Random();
            var tmpBoard = new bool[_boardContext.Width, _boardContext.Height];
            ParallelOptions po = new ParallelOptions() { MaxDegreeOfParallelism = _gameConfiguration.MaxConcurrency };
            Parallel.For(0, _boardContext.Height, po, y =>
            {
                for(int x = 0; x < _boardContext.Width; x++)
                {
                    tmpBoard[x, y] = random.Next(_gameConfiguration.DeadCellInitialisationBias) == 0;
                }
            });
            _boardContext.GameBoard = tmpBoard;
        }

        public void InitialiseFromSeed(bool[,] seedBoard)
        {
            _boardContext.Height = seedBoard.GetUpperBound(0);
            _boardContext.Width = seedBoard.GetUpperBound(1);
            _boardContext.LoopEdges = _gameConfiguration.LoopEdges;
            _boardContext.GameBoard = seedBoard;
        }

        public void ProcessBoardTick()
        {
            _processTimer.Start();
            var tmpBoard = new bool[_boardContext.Width, _boardContext.Height];
            ParallelOptions po = new ParallelOptions() { MaxDegreeOfParallelism = _gameConfiguration.MaxConcurrency };
            Parallel.For(0, _boardContext.Height, po, y =>
            {
                Parallel.For(0, _boardContext.Width, po, x =>
                {
                    _cellStateManager.ExecuteCellStateRule(tmpBoard, x, y);
                });
            });
            _boardContext.GameBoard = tmpBoard;
            ComputeAverageTimeElapsed();
        }

        private void ComputeAverageTimeElapsed()
        {
            var avgTimeData = new List<TimeSpan>();
            avgTimeData.Add(_processTimer.Elapsed);
            avgTimeData.Add(AverageTickElapsedTime);
            AverageTickElapsedTime = new TimeSpan((long)avgTimeData.Average(x => x.Ticks));
            _processTimer.Reset();
        }
    }
}

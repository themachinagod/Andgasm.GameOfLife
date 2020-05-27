using System;

namespace Andgasm.GameOfLife.Core.Abstractions
{
    public interface IBoardManager
    {
        TimeSpan AverageTickElapsedTime { get; set; }

        void InitialiseRandomBoard();
        void InitialiseFromSeed(bool[,] seedBoard);
        void ProcessBoardTick();
    }
}

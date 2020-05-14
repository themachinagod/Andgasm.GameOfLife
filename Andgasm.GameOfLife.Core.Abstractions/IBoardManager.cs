using System;

namespace Andgasm.GameOfLife.Core.Abstractions
{
    public interface IBoardManager
    {
        TimeSpan AverageTickElapsedTime { get; set; }

        void InitialiseRandomBoard();
        void ProcessBoardTick();
    }
}

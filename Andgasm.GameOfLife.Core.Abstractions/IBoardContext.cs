using System;

namespace Andgasm.GameOfLife.Core.Abstractions
{
    public interface IBoardContext
    {
        int Height { get; set; }
        int Width { get; set; }
        bool LoopEdges { get; set; }
        bool[,] GameBoard { get; set; }
    }
}

using System;

namespace Andgasm.GameOfLife.Core.Abstractions
{
    public interface ICellStateManager
    {
        int CountLiveNeighbors(int x, int y);
    }
}

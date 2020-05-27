using System;

namespace Andgasm.GameOfLife.Core.Abstractions
{
    public interface ICellStateRule
    {
        void ExecuteCellStateRule(bool[,] tempboard, int x, int y);
    }
}

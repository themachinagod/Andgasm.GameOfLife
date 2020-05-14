using Andgasm.GameOfLife.Core.Abstractions;

namespace Andgasm.GameOfLife.Core
{
    public class CellStateManager : ICellStateManager
    {
        private readonly IBoardContext _boardContext;

        public CellStateManager(IBoardContext boardContext)
        {
            _boardContext = boardContext;
        }

        public int CountLiveNeighbors(int x, int y)
        {
            // The number of live neighbors.
            int value = 0;

            // This nested loop enumerates the 9 cells in the specified cells neighborhood.
            for (var j = -1; j <= 1; j++)
            {
                // If loopEdges is set to false and y+j is off the board, continue.
                if (!_boardContext.LoopEdges && y + j < 0 || y + j >= _boardContext.Height)
                {
                    continue;
                }

                // Loop around the edges if y+j is off the board.
                int k = (y + j + _boardContext.Height) % _boardContext.Height;

                for (var i = -1; i <= 1; i++)
                {
                    // If loopEdges is set to false and x+i is off the board, continue.
                    if (!_boardContext.LoopEdges && x + i < 0 || x + i >= _boardContext.Width)
                    {
                        continue;
                    }

                    // Loop around the edges if x+i is off the board.
                    int h = (x + i + _boardContext.Width) % _boardContext.Width;

                    // Count the neighbor cell at (h,k) if it is alive.
                    value += _boardContext.GameBoard[h, k] ? 1 : 0;
                }
            }

            // Subtract 1 if (x,y) is alive since we counted it as a neighbor.
            return value - (_boardContext.GameBoard[x, y] ? 1 : 0);
        }
    }
}

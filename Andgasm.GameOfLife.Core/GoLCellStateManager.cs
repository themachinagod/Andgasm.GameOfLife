using Andgasm.GameOfLife.Core.Abstractions;

namespace Andgasm.GameOfLife.Core
{
    public class GoLCellStateManager : ICellStateRule
    {
        private readonly IBoardContext _boardContext;

        public GoLCellStateManager(IBoardContext boardContext)
        {
            _boardContext = boardContext;
        }

        public void ExecuteCellStateRule(bool[,] tempboard, int x, int y)
        {
            // A live cell dies unless it has exactly 2 or 3 live neighbors.
            // A dead cell remains dead unless it has exactly 3 live neighbors.
            var cell = _boardContext.GameBoard[x, y];
            var cellLiveNeighborsCount = CountLiveNeighbors(x, y);
            tempboard[x, y] = cell && (cellLiveNeighborsCount == 2 || cellLiveNeighborsCount == 3) || !cell && cellLiveNeighborsCount == 3;
        }

        private int CountLiveNeighbors(int x, int y)
        {
            int value = 0;
            for (var j = -1; j <= 1; j++)
            {
                if (!_boardContext.LoopEdges && y + j < 0 || y + j >= _boardContext.Height) continue;
                int k = (y + j + _boardContext.Height) % _boardContext.Height;

                for (var i = -1; i <= 1; i++)
                {
                    if (!_boardContext.LoopEdges && x + i < 0 || x + i >= _boardContext.Width) continue;
                    
                    int h = (x + i + _boardContext.Width) % _boardContext.Width;
                    value += _boardContext.GameBoard[h, k] ? 1 : 0;
                }
            }
            return value - (_boardContext.GameBoard[x, y] ? 1 : 0);
        }
    }
}

using Andgasm.GameOfLife.Core.Abstractions;

namespace Andgasm.GameOfLife.Core
{
    public class BoardContext : IBoardContext
    {
        public int Height { get; set; }
        public  int Width { get; set; }
        public bool LoopEdges { get; set; }
        public bool[,] GameBoard { get; set; }
    }
}

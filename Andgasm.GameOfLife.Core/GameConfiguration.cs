using Andgasm.GameOfLife.Core.Abstractions;

namespace Andgasm.GameOfLife.Core
{
    public class GameConfiguration : IGameConfiguration
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int MaxTickCount { get; set; }
        public int TickDelay { get; set; }
        public bool LoopEdges { get; set; }
        public bool ManualTickProgression { get; set; }
        public int DeadCellInitialisationBias { get; set; }
        public int MaxConcurrency { get; set; }
        public bool RaiseRenderEvents { get; set; }
    }
}

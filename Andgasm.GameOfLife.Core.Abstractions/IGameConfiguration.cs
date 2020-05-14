using System;

namespace Andgasm.GameOfLife.Core.Abstractions
{
    public interface IGameConfiguration
    {
        int Height { get; set; }
        int Width { get; set; }
        int MaxTickCount { get; set; }
        int TickDelay { get; set; }
        bool LoopEdges { get; set; }
        bool ManualTickProgression { get; set; }
        int DeadCellInitialisationBias { get; set; }
        int MaxConcurrency { get; set; }
        bool RaiseRenderEvents { get; set; }
    }
}

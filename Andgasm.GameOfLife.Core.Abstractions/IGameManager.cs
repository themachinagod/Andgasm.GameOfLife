using System;
using System.Threading.Tasks;

namespace Andgasm.GameOfLife.Core.Abstractions
{
    public interface IGameManager
    {
        event EventHandler GameTickProcessed;

        TimeSpan GameElapsedTime { get; }
        TimeSpan AverageTickElapsedTime { get; }

        int TickCount { get; set; }

        Task StartGame();
    }
}

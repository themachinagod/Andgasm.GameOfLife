using Andgasm.GameOfLife.Core;
using Andgasm.GameOfLife.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ConwaysGameOfLife
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IGameManager _gameManager;

        private const char EMPTY_BLOCK_CHAR = ' ';
        private const char FULL_BLOCK_CHAR = '\u2588';

        static async Task Main(string[] args)
        {
            _serviceProvider = InitialiseCompositionRoot();
            InitialiseGameConfiguration();

            _gameManager = _serviceProvider.GetService<IGameManager>();
            _gameManager.GameTickProcessed += RenderBoardState;
            await _gameManager.StartGame();

            RenderCompletionStats(_gameManager);
            Console.WriteLine("Press any key to close the application...");
            Console.ReadKey();
        }

        static IServiceProvider InitialiseCompositionRoot()
        {
            return new ServiceCollection()
                .AddSingleton<IGameConfiguration, GameConfiguration>()
                .AddSingleton<IBoardContext, BoardContext>()
                .AddTransient<IGameManager, GameManager>()
                .AddTransient<IBoardManager, BoardManager>()
                .AddTransient<ICellStateManager, CellStateManager>()
                .BuildServiceProvider();
        }

        static void InitialiseGameConfiguration()
        {
            var gameconfig = _serviceProvider.GetService<IGameConfiguration>();
            gameconfig.Height = 40;
            gameconfig.Width = 85;
            gameconfig.LoopEdges = true;
            gameconfig.TickDelay = 50;
            gameconfig.MaxTickCount = 1000;
            gameconfig.ManualTickProgression = false;
            gameconfig.DeadCellInitialisationBias = 2;
            gameconfig.MaxConcurrency = 4;
            gameconfig.RaiseRenderEvents = true;
        }

        static void RenderBoardState(object sender, EventArgs args)
        {
            var gameconfig = _serviceProvider.GetService<IGameConfiguration>();
            var boardcontext = _serviceProvider.GetService<IBoardContext>();
            var builder = new StringBuilder();
            for (var y = 0; y < boardcontext.Height; y++)
            {
                for (var x = 0; x < boardcontext.Width; x++)
                {
                    char c = boardcontext.GameBoard[x, y] ? FULL_BLOCK_CHAR : EMPTY_BLOCK_CHAR;
                    builder.Append(c);
                    builder.Append(c);
                }
                builder.Append('\n');
            }
            Console.SetCursorPosition(0, 0);
            Console.Write(builder.ToString());
            if (gameconfig.ManualTickProgression) Console.ReadKey();
        }

        static void RenderCompletionStats(IGameManager gamemanager)
        {
            Console.WriteLine("Number of ticks processed: " + gamemanager.TickCount);
            Console.WriteLine("Average Process Time Per Tick: " + gamemanager.AverageTickElapsedTime);
            Console.WriteLine("Total Game Process Time: " + gamemanager.GameElapsedTime);
        }
    }
}
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
            var _gameconfig = InitialiseGameConfiguration();

            _gameManager = _serviceProvider.GetService<IGameManager>();
            _gameManager.GameTickProcessed += RenderBoardState;

            var seed = InitialiseTestBoardSeed(_gameconfig);
            await _gameManager.StartGame(seed);

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
                .AddTransient<ICellStateRule, GoLCellStateManager>()
                .BuildServiceProvider();
        }

        static IGameConfiguration InitialiseGameConfiguration()
        {
            var gameconfig = _serviceProvider.GetService<IGameConfiguration>();
            gameconfig.Height = 15;
            gameconfig.Width = 15;
            gameconfig.LoopEdges = true;
            gameconfig.TickDelay = 50;
            gameconfig.MaxTickCount = 1000;
            gameconfig.ManualTickProgression = true;
            gameconfig.DeadCellInitialisationBias = 2;
            gameconfig.MaxConcurrency = 1;
            gameconfig.RaiseRenderEvents = true;
            return gameconfig;
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

        static bool[,] InitialiseTestBoardSeed(IGameConfiguration _gameConfiguration)
        {
            const int livecellseed = 2;
            var counter = 0;
            var tmpBoard = new bool[_gameConfiguration.Width, _gameConfiguration.Height];
            for (int y = 0; y < _gameConfiguration.Height; y++)
            {
                for (int x = 0; x < _gameConfiguration.Width; x++)
                {
                    tmpBoard[x, y] = counter++ == livecellseed;
                    if (counter > livecellseed) counter = 0;
                }
            }
            return tmpBoard;
        }
    }
}
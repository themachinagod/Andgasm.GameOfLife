using Andgasm.GameOfLife.Core.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Andgasm.GameOfLife.Core.Tests
{
    public class BoardManagerShould
    {
        [Fact]
        public void InitialiseRandomBoardToConfiguredDimensions_WhenInvoked()
        {
            var gameConfig = SetupGameConfig();
            var boardContext = new BoardContext();
            var cellStateManager = new Mock<ICellStateRule>();
            var boardManager = new BoardManager(gameConfig.Object, boardContext, cellStateManager.Object);
            boardManager.InitialiseRandomBoard();

            Assert.NotNull(boardContext.GameBoard);
            Assert.Equal(15, boardContext.Height);
            Assert.Equal(15, boardContext.Width);
            Assert.True(boardContext.LoopEdges);

            List<bool> lst = boardContext.GameBoard.Cast<bool>().ToList();
            Assert.True(lst.Exists(x => x == true));
            Assert.True(lst.Exists(x => x == false));
        }

        [Fact]
        public void ProgressBoardTick_WhenInvoked()
        {
            var gameConfig = SetupGameConfig();
            var boardContext = InitialiseTestBoardContext(gameConfig.Object);
            var cellStateManager = new Mock<ICellStateRule>();
            var boardManager = new BoardManager(gameConfig.Object, boardContext, cellStateManager.Object);
            boardManager.ProcessBoardTick();

            Assert.NotNull(boardContext.GameBoard);
            List<bool> lst = boardContext.GameBoard.Cast<bool>().ToList();
            Assert.True(lst.All(x => x == false));
            cellStateManager.Verify(x => x.ExecuteCellStateRule(It.IsAny<bool[,]>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(boardContext.Height * boardContext.Width));
        }

        #region Setup
        private Mock<IGameConfiguration> SetupGameConfig()
        {
            var gameConfig = new Mock<IGameConfiguration>();
            gameConfig.Setup(x => x.Height).Returns(15);
            gameConfig.Setup(x => x.Width).Returns(15);
            gameConfig.Setup(x => x.LoopEdges).Returns(true);
            gameConfig.Setup(x => x.MaxConcurrency).Returns(1);
            gameConfig.Setup(x => x.RaiseRenderEvents).Returns(false);
            gameConfig.Setup(x => x.DeadCellInitialisationBias).Returns(3);
            gameConfig.Setup(x => x.ManualTickProgression).Returns(false);
            gameConfig.Setup(x => x.TickDelay).Returns(50);
            return gameConfig;
        }

        private IBoardContext InitialiseTestBoardContext(IGameConfiguration _gameConfiguration)
        {
            var _boardContext = new BoardContext();
            _boardContext.Height = _gameConfiguration.Height;
            _boardContext.Width = _gameConfiguration.Width;
            _boardContext.LoopEdges = _gameConfiguration.LoopEdges;

            const int livecellseed = 2;
            var counter = 0;
            var tmpBoard = new bool[_boardContext.Width, _boardContext.Height];
            for(int y = 0; y < _boardContext.Height; y++)
            {
                for (int x = 0; x < _boardContext.Width; x++)
                {
                    tmpBoard[x, y] = counter++ == livecellseed;
                    if (counter > livecellseed) counter = 0;
                }
            }
            _boardContext.GameBoard = tmpBoard;
            return _boardContext;
        }
        #endregion
    }
}

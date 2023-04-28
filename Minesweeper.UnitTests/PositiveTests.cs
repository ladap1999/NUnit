using Minesweeper.Core;
using Minesweeper.Core.Enums;
using NUnit.Framework;

namespace Minesweeper.UnitTests;

[TestFixture]
public class PositiveTests : BaseTest
{
    [Test]
    public void GameProcessor_CheckInitialState_GameStateIsActive()
    {
        var setting = DifficultyManager.GetGameSettingsByDifficultylevel(DifficultyLevel.Beginner);
        var field = FieldGenerator.GetRandomField(setting.Width, setting.Height, setting.Mines);
        GameProcessor gameProcessor = new GameProcessor(field);
        
        Assert.That(gameProcessor.GameState, Is.EqualTo(GameState.Active));
    }
    
    [Test]
    public void GameProcessor_OpenFewCells_GameStateIsActive()
    {
        var gameProcessor = new GameProcessor(boolField);
        gameProcessor.Open(0, 0);
        gameProcessor.Open(1, 0);
        
        Assert.That(gameProcessor.GameState, Is.EqualTo(GameState.Active));
    }
    
    [Test]
    public void GameProcessor_OpenWithCellIsMine_GameStateIsLost()
    {
        var gameProcessor = new GameProcessor(boolField);
        gameProcessor.Open(2, 0);
        
        Assert.That(gameProcessor.GameState, Is.EqualTo(GameState.Lose));
    }

    [Test]
    public void GameProcessor_OpenAllNotMineCells_GameStateIsWin()
    {
        var gameProcessor = new GameProcessor(boolField);
        
        gameProcessor.Open(0, 0);
        gameProcessor.Open(1, 0);
        gameProcessor.Open(0, 1);
        gameProcessor.Open(2, 1);
        gameProcessor.Open(1, 2);
        gameProcessor.Open(2, 2);
        
        Assert.That(gameProcessor.GameState, Is.EqualTo(GameState.Win));
    }
    
    [Test]
    public void GameProcessor_GetCurrentFieldBeforeOpen_AllCellsClosed()
    {
        var gameProcessor = new GameProcessor(boolField);
        
        var result = gameProcessor.GetCurrentField();
        
        for (int row = 0; row < boolField.GetLength(0); row++)
        {
            for (int column = 0; column < boolField.GetLength(1); column++)
            {
                Assert.That(result[row, column], Is.EqualTo(PointState.Close));
            }
        }
    }

    [Test]
    public void GameProcessor_OpenCellWithoutMineNeighbors_AllNeighborsAreOpenedAndGameStateIsWin()
    {
        var amountOfMines = 0;
        var setting = DifficultyManager.GetGameSettingsByDifficultylevel(DifficultyLevel.Beginner);
        var field = FieldGenerator.GetRandomField(setting.Width, setting.Height, amountOfMines);
        var gameProcessor = new GameProcessor(field);

        gameProcessor.Open(1, 1);

        for (var row = 0; row < field.GetLength(0); row++)
        {
            for (var column = 0; column < field.GetLength(1); column++)
            {
                Assert.IsTrue(gameProcessor.GetCurrentField()[row, column] == PointState.Neighbors0);
            }
        }
        Assert.That(gameProcessor.GameState, Is.EqualTo(GameState.Win));
    }
    
    [Test]
    public void GameProcessor_GetCellsWithMineNeighbors_NeighborsAreCounted()
    {
        var gameProcessor = new GameProcessor(boolField);
        gameProcessor.Open(0, 0);
        gameProcessor.Open(1, 0);
        
        Assert.IsTrue(gameProcessor.GetCurrentField()[0, 0] == PointState.Neighbors1);
        Assert.IsTrue(gameProcessor.GetCurrentField()[0, 1] == PointState.Neighbors2);
    }
    
    [Test]
    public void GameProcessor_GetCellWithAllMine_AllMineNeighborsAreCounted()
    {
        var field = new bool[,]
        {
            { true, true, true },
            { true, false, true },
            { true, true, true }
        };
        var gameProcessor = new GameProcessor(field);
        
        gameProcessor.Open(1, 1);

        Assert.IsTrue(gameProcessor.GetCurrentField()[1, 1] == PointState.Neighbors8);
    }
    
    [Test]
    public void GameProcessor_GetCellMine_PointStateIsMine()
    {
        var gameProcessor = new GameProcessor(boolField);
        gameProcessor.Open(2, 0);

        Assert.IsTrue(gameProcessor.GetCurrentField()[0, 2] == PointState.Mine);
    }
}
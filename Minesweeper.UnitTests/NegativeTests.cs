using Minesweeper.Core;
using Minesweeper.Core.Enums;
using NUnit.Framework;


namespace Minesweeper.UnitTests;

[TestFixture]
public class NegativeTests : BaseTest
{
    [Test]
    public void GameProcessor_InitialStateWithNull_ThrowsNullReferenceException()
    {
        Assert.Throws<NullReferenceException>(() => new GameProcessor(null));
    }

    [Test]
    [TestCase(-3,-4)]
    [TestCase(3,4)]
    public void GameProcessor_OpenCellsOutOfRange_ThrowsException(int x, int y)
    {
        var gameProcessor = new GameProcessor(boolField);

        Assert.Throws<IndexOutOfRangeException>(() => gameProcessor.Open(x, y));
    }

    [Test]
    public void GameProcessor_OpenCellAfterWin_ThrowsInvalidOperationException()
    {
        var gameProcessor = new GameProcessor(boolField);

        gameProcessor.Open(0, 0);
        gameProcessor.Open(1, 0);
        gameProcessor.Open(0, 1);
        gameProcessor.Open(2, 1);
        gameProcessor.Open(1, 2);
        gameProcessor.Open(2, 2);

        Assert.Throws<InvalidOperationException>(() => gameProcessor.Open(0, 0));
    }

    //Check CI/CD trigger
    [Test]
    public void GameProcessor_OpenSameCellTwice_GameStateIsActive()
    {
        var gameProcessor = new GameProcessor(boolField);

        gameProcessor.Open(0, 0);
        gameProcessor.Open(0, 0);

        Assert.That(gameProcessor.GameState, Is.EqualTo(GameState.Active));
    }

    [Test]
    public void GameProcessor_CheckMinimalSizeOfField_MinimalSizeIsSet()
    {
        var minX = 0;
        var minY = 7;
        var expectedMinSizeX = 1;
        var expectedMinSizeY = 8;
        var setting = DifficultyManager.GetGameSettingsByDifficultylevel(DifficultyLevel.Beginner);
        var field = FieldGenerator.GetRandomField(minX, minY, setting.Mines);
        GameProcessor gameProcessor = new GameProcessor(field);

        Assert.Multiple(() =>
        {
            Assert.That(field.GetLength(0), Is.EqualTo(expectedMinSizeY));
            Assert.That(field.GetLength(1), Is.EqualTo(expectedMinSizeX));
        });
    }

    [Test]
    public void GameProcessor_CheckNegativeSizeOfField_ThrowsOverflowException()
    {
        var minX = -20;
        var minY = -20;
        var setting = DifficultyManager.GetGameSettingsByDifficultylevel(DifficultyLevel.Beginner);
        
        Assert.Throws<OverflowException>(() => FieldGenerator.GetRandomField(minX, minY, setting.Mines));
    }
    
    [Test]
    [TestCase(-20,1)]
    [TestCase(2,-10)]
    public void GameProcessor_CheckNegativeSizeOfOneColumnOrRow_ThrowsArgumentOutOfRangeException(int minX, int minY)
    {
        var setting = DifficultyManager.GetGameSettingsByDifficultylevel(DifficultyLevel.Beginner);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => FieldGenerator.GetRandomField(minX, minY, setting.Mines));
    }
    
    [Test]
    [TestCase(81, 80, DifficultyLevel.Beginner)]
    [TestCase(256, 255, DifficultyLevel.Intermediate)]
    [TestCase(480, 479, DifficultyLevel.Expert)]
    public void GameProcessor_CheckMaxAmountOfMines_ExpectedOmeNonMineCell(int amounOfMines, int expectedResult, DifficultyLevel level)
    {
        var count = 0;
        var setting = DifficultyManager.GetGameSettingsByDifficultylevel(level);
        var field = FieldGenerator.GetRandomField(setting.Width, setting.Height, amounOfMines);
        GameProcessor gameProcessor = new GameProcessor(field);
        
        for (int row = 0; row < field.GetLength(0); row++)
        {
            for (int column = 0; column < field.GetLength(1); column++)
            {
                if (field[row, column].Equals(true))
                {
                    count++;
                }
            }
        }
        Assert.That(count, Is.EqualTo(expectedResult));
    }
}
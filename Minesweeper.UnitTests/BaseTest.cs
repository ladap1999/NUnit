using NUnit.Framework;

namespace Minesweeper.UnitTests;


public class BaseTest
{
    protected bool[,] boolField;

    [SetUp]
    public void Setup()
    {
        boolField = new bool[,]
        {
            { false, false, true },
            { false, true, false },
            { true, false, false }
        };
    }
}
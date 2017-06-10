using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class RobotTests
{
    private int[] _molecules;
    private Robot _robot;

    [SetUp]
    public void SetupTest()
    {
        this._molecules = new[] { 0, 0, 0, 0, 0 };
        this._robot = new Robot("START_POS", 0, 0, this._molecules);
    }

    [Test]
    public void RobotSetup()
    {     
        Assert.That(_robot.Position, Is.EqualTo(Position.StartPos));
        Assert.That(_robot.Eta, Is.EqualTo(0));
        Assert.That(_robot.Score, Is.EqualTo(0));

        foreach (var kvp in _robot.Storage)
        {
            Assert.That(kvp.Value, Is.EqualTo(_molecules[(int)kvp.Key]));
        }
    }
}
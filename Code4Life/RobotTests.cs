using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class RobotTests
{
    private int[] molecules;
    private Robot robot;

    [SetUp]
    public void SetupTest()
    {
        this.molecules = = new int[] { 1, 2, 3, 4, 5 };
        this.robot = new Robot("START_POS", 0, 0, this.molecules);
    }

    [Test]
    public void RobotSetup()
    {     
        Assert.That(robot.Position, Is.EqualTo(Position.StartPos));
        Assert.That(robot.Eta, Is.EqualTo(0));
        Assert.That(robot.Score, Is.EqualTo(0));

        foreach (var kvp in robot.Storage)
        {
            Assert.That(kvp.Value, Is.EqualTo(molecules[(int)kvp.Key]));
        }
    }
}
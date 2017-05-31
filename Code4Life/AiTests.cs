using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class AiTests
{
    private AI ai;

    [SetUp]
    public void SetupAi()
    {
        var robot = new Robot("START_POS", 0, 0, new int[] { 0, 0, 0, 0, 0 });
        this.ai = new AI(robot);
    }

    [Test]
    public void SetupTest()
    {
        Assert.That(this.ai.Mode, Is.EqualTo(Mode.SamplePickup));
    }
}


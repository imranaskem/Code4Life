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

    [Test]
    public void SamplePickUpModeAtCapacityTest()
    {
        var sample = new Sample(0, 0, 10, new int[] { 1, 1, 1, 1, 0 });
        ai.Samples.AddSample(sample);

        sample = new Sample(1, 0, 10, new int[] { 1, 1, 1, 1, 0 });
        ai.Samples.AddSample(sample);

        sample = new Sample(2, 0, 10, new int[] { 1, 1, 1, 1, 0 });
        ai.Samples.AddSample(sample);

        var order = ai.SamplePickUpMode();

        Assert.That(order.Type, Is.EqualTo(OrderType.Move));
        Assert.That(order.Position, Is.EqualTo(Position.Molecules));
        Assert.That(ai.Mode, Is.EqualTo(Mode.MoleculePickup));
    }

    [Test]
    public void SamplePickUpModeInPositionHaveCapacityNullTest()
    {
        ai.Player.Position = Position.Diagnosis;

        var order = ai.SamplePickUpMode();

        Assert.That(ai.Mode, Is.EqualTo(Mode.MoleculePickup));
        Assert.That(order.Type, Is.EqualTo(OrderType.Move));
        Assert.That(order.Position, Is.EqualTo(Position.Molecules));
    }

    [Test]
    public void SamplePickupModeInPositionHaveCapacityTest()
    {
        var sample = new Sample(0, -1, 100, new int[] { 1, 1, 1, 1, 0 });
        ai.Samples.AddSample(sample);

        sample = new Sample(1, -1, 10, new int[] { 1, 1, 1, 1, 0 });
        ai.Samples.AddSample(sample);

        sample = new Sample(2, -1, 10, new int[] { 1, 1, 1, 1, 0 });
        ai.Samples.AddSample(sample);

        ai.Player.Position = Position.Diagnosis;

        var order = ai.SamplePickUpMode();

        Assert.That(order.Type, Is.EqualTo(OrderType.ConnectSample));
        Assert.That(order.SampleId, Is.EqualTo(0));
    }
}


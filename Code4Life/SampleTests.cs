using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class SampleTests
{
    private int[] _molecules;
    private Sample _sample;

    [SetUp]
    public void SetupSample()
    {
        this._molecules = new[] { 1, 0, 1, 1, 1 };
        this._sample = new Sample(1, 1, 10, _molecules);
    }

    [Test]
    public void SampleSetup()
    {      
        Assert.That(_sample.Id, Is.EqualTo(1));
        Assert.That(_sample.CarriedBy, Is.EqualTo(Carried.Other));
        Assert.That(_sample.HealthPoints, Is.EqualTo(10));
        Assert.That(_sample.MoleculeValue, Is.EqualTo(2.5));

        foreach (var kvp in _sample.Cost)
        {
            Assert.That(kvp.Value, Is.EqualTo(_molecules[(int)kvp.Key]));
        }
    }
}
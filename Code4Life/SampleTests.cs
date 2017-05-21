using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class SampleTests
{
    private int[] molecules;
    private Sample sample;

    [SetUp]
    public void SetupSample()
    {
        this.molecules = new int[] { 1, 0, 1, 1, 1 };
        this.sample = new Sample(1, 1, 10, molecules);
    }

    [Test]
    public void SampleSetup()
    {      
        Assert.That(sample.Id, Is.EqualTo(1));
        Assert.That(sample.CarriedBy, Is.EqualTo(Carried.Other));
        Assert.That(sample.HealthPoints, Is.EqualTo(10));
        Assert.That(sample.MoleculeValue, Is.EqualTo(2.5));

        foreach (var kvp in sample.Cost)
        {
            Assert.That(kvp.Value, Is.EqualTo(molecules[(int)kvp.Key]));
        }
    }
}
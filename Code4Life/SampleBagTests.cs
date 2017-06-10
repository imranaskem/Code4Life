using System;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class SampleBagTests
{
    private SampleBag _sampleBag;

    [SetUp]
    public void SetupSampleBag()
    {
        this._sampleBag = new SampleBag();

        var sample = new Sample(0, -1, 10, new[] { 0, 1, 2, 1, 0 });
        this._sampleBag.AddSample(sample);

        sample = new Sample(1, -1, 10, new[] { 0, 1, 2, 1, 0 });
        this._sampleBag.AddSample(sample);

        sample = new Sample(2, -1, 10, new[] { 0, 1, 2, 1, 0 });
        this._sampleBag.AddSample(sample);

        sample = new Sample(3, 0, 10, new[] { 0, 1, 2, 1, 0 });
        this._sampleBag.AddSample(sample);

        sample = new Sample(4, 0, 10, new[] { 0, 1, 2, 1, 0 });
        this._sampleBag.AddSample(sample);
    }

    [Test]
    public void DoesThisExistTest()
    {
        var result = this._sampleBag.DoesThisExistInBag(0);

        Assert.That(result, Is.EqualTo(true));
    }

    [Test]
    public void SingleSampleByIdTest()
    {
        var sample = this._sampleBag.SingleSampleById(1);

        Assert.That(sample.Id, Is.EqualTo(1));
        Assert.That(sample.CarriedBy, Is.EqualTo(Carried.Cloud));
        Assert.That(sample.HealthPoints, Is.EqualTo(10));
    }

    [Test]
    public void FirstSampleCarriedByPlayerTest()
    {
        var sample = this._sampleBag.FirstSampleCarriedByPlayer();

        Assert.That(sample.Id, Is.EqualTo(3));
    }

    [Test]
    public void AllSamplesCarriedByPlayerTest()
    {
        var samples = this._sampleBag.AllSamplesCarriedByPlayer();

        Assert.That(samples.Count(), Is.EqualTo(2));
        Assert.That(samples.Any(s => s.Id == 3), Is.EqualTo(true));
        Assert.That(samples.Any(s => s.Id == 4), Is.EqualTo(true));
    }

    [Test]
    public void AddSampleTest()
    {
        var sample = new Sample(5, 1, 20, new[] { 1, 1, 1, 1, 0 });

        this._sampleBag.AddSample(sample);

        Assert.That(this._sampleBag.DoesThisExistInBag(5), Is.EqualTo(true));
    }

    [Test]
    public void NumberCarriedByPlayer()
    {
        var count = this._sampleBag.NumberCarriedByPlayer();

        Assert.That(count, Is.EqualTo(2));
    }

    [Test]
    public void IsPlayerAtCapacity()
    {
        Assert.That(this._sampleBag.IsPlayerAtCapacity(), Is.EqualTo(false));

        var sample = new Sample(5, 0, 20, new[] { 1, 1, 1, 1, 0 });

        this._sampleBag.AddSample(sample);

        Assert.That(this._sampleBag.IsPlayerAtCapacity(), Is.EqualTo(true));
    }

    [Test]
    public void ListOfPossibleSamplesToTakeTest()
    {
        var samples = this._sampleBag.ListOfPossibleSamplesToTake(10);
        var count = samples.Count();

        Assert.That(count, Is.EqualTo(3));
    }
}
using System;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class SampleBagTests
{
    private SampleBag sampleBag;

    [SetUp]
    public void SetupSampleBag()
    {
        this.sampleBag = new SampleBag();

        var sample = new Sample(0, -1, 10, new int[] { 0, 1, 2, 1, 0 });
        this.sampleBag.AddSample(sample);

        sample = new Sample(1, -1, 10, new int[] { 0, 1, 2, 1, 0 });
        this.sampleBag.AddSample(sample);

        sample = new Sample(2, -1, 10, new int[] { 0, 1, 2, 1, 0 });
        this.sampleBag.AddSample(sample);

        sample = new Sample(3, 0, 10, new int[] { 0, 1, 2, 1, 0 });
        this.sampleBag.AddSample(sample);

        sample = new Sample(4, 0, 10, new int[] { 0, 1, 2, 1, 0 });
        this.sampleBag.AddSample(sample);
    }
}
using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
[TestFixture]
public class MoleculesBagTests
{
    private MoleculeBag moleculeBag;

    [SetUp]
    public void SetupMoleculeBag()
    {
        this.moleculeBag = new MoleculeBag();
    }
    [Test]
    public void MoleculeBagSetup()
    {      
        foreach (var item in moleculeBag)
        {
            Assert.That(item.Value, Is.EqualTo(0));
        }
    }
    [Test]
    public void UpdateMoleculeValuesTest()
    {
        var moleculeValues = new int[] { 2, 2, 2, 2, 2 };
        this.moleculeBag.UpdateMoleculeValues(moleculeValues);
        foreach (var item in this.moleculeBag)
        {
            Assert.That(item.Value, Is.EqualTo(moleculeValues[(int)item.Key]));
        }
    }    [Test]    public void UpdateSingleValueTest()
    {
        this.moleculeBag.UpdateSingleValue(MoleculeType.A, 2);
        this.moleculeBag.UpdateSingleValue(MoleculeType.A, 2);

        var molecule = this.moleculeBag.Single(s => s.Key == MoleculeType.A);

        Assert.That(molecule.Value, Is.EqualTo(4));
    }    [Test]    public void SetValuesToZeroTest()    {
        this.moleculeBag.UpdateSingleValue(MoleculeType.A, 2);
        this.moleculeBag.UpdateSingleValue(MoleculeType.A, 2);

        this.moleculeBag.SetValuesToZero();

        foreach (var item in this.moleculeBag)
        {
            Assert.That(item.Value, Is.EqualTo(0));
        }
    }    [Test]    public void GetValueOfKeyTest()
    {
        var value = this.moleculeBag.GetValueOfKey(MoleculeType.A);

        Assert.That(value, Is.EqualTo(0));
    }
    [Test]
    public void UpdateMoleculeValuesExceptionTest()
    {
        var moleculeValues = new int[] { 5, 2, 2, 2, 2 };
        Assert.That(() => this.moleculeBag.UpdateMoleculeValues(moleculeValues), Throws.TypeOf<ArgumentOutOfRangeException>());    
    }
    [Test]
    public void EmptyRemainingCapacityTest()
    {
        var capacity = this.moleculeBag.RemainingCapacity();
        Assert.That(capacity, Is.EqualTo(10));
    }
    [Test]
    public void FullRemainingCapacityTest()
    {
        var moleculeValues = new int[] { 2, 2, 2, 2, 2 };
        this.moleculeBag.UpdateMoleculeValues(moleculeValues);
        var capacity = this.moleculeBag.RemainingCapacity();
        Assert.That(capacity, Is.EqualTo(0));
    }
}
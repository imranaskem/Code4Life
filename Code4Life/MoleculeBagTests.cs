using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class MoleculesBagTests
{
    private MoleculeBag _moleculeBag;

    [SetUp]
    public void SetupMoleculeBag()
    {
        this._moleculeBag = new MoleculeBag();
    }

    [Test]
    public void MoleculeBagSetup()
    {      
        foreach (var item in _moleculeBag)
        {
            Assert.That(item.Value, Is.EqualTo(0));
        }
    }

    [Test]
    public void UpdateMoleculeValuesTest()
    {
        var moleculeValues = new[] { 2, 2, 2, 2, 2 };

        this._moleculeBag.UpdateMoleculeValues(moleculeValues);

        foreach (var item in this._moleculeBag)
        {
            Assert.That(item.Value, Is.EqualTo(moleculeValues[(int)item.Key]));
        }
    }

    [Test]
    public void UpdateSingleValueTest()
    {
        this._moleculeBag.UpdateSingleValue(MoleculeType.A, 2);
        this._moleculeBag.UpdateSingleValue(MoleculeType.A, 2);

        var molecule = this._moleculeBag.Single(s => s.Key == MoleculeType.A);

        Assert.That(molecule.Value, Is.EqualTo(4));
    }

    [Test]
    public void SetValuesToZeroTest()
    {
        this._moleculeBag.UpdateSingleValue(MoleculeType.A, 2);
        this._moleculeBag.UpdateSingleValue(MoleculeType.A, 2);

        this._moleculeBag.SetValuesToZero();

        foreach (var item in this._moleculeBag)
        {
            Assert.That(item.Value, Is.EqualTo(0));
        }
    }

    [Test]
    public void GetValueOfKeyTest()
    {
        var value = this._moleculeBag.GetValueOfKey(MoleculeType.A);

        Assert.That(value, Is.EqualTo(0));
    }

    [Test]
    public void UpdateMoleculeValuesExceptionTest()
    {
        var moleculeValues = new[] { 5, 2, 2, 2, 2 };

        Assert.That(() => this._moleculeBag.UpdateMoleculeValues(moleculeValues), Throws.TypeOf<ArgumentOutOfRangeException>());    
    }

    [Test]
    public void EmptyRemainingCapacityTest()
    {
        var capacity = this._moleculeBag.RemainingCapacity();

        Assert.That(capacity, Is.EqualTo(10));
    }

    [Test]
    public void FullRemainingCapacityTest()
    {
        var moleculeValues = new[] { 2, 2, 2, 2, 2 };

        this._moleculeBag.UpdateMoleculeValues(moleculeValues);

        var capacity = this._moleculeBag.RemainingCapacity();

        Assert.That(capacity, Is.EqualTo(0));
    }
}
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Bring data on patient samples from the diagnosis machine to the laboratory with enough molecules to produce medicine!
 **/
class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        int projectCount = int.Parse(Console.ReadLine());
        for (int i = 0; i < projectCount; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int a = int.Parse(inputs[0]);
            int b = int.Parse(inputs[1]);
            int c = int.Parse(inputs[2]);
            int d = int.Parse(inputs[3]);
            int e = int.Parse(inputs[4]);
        }

        Robot player = new Robot("START_POS", 0, 0, new int[] { 0, 0, 0, 0, 0 });       

        var ai = new AI(player);

        // game loop
        while (true)
        {
            for (int i = 0; i < 2; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                string target = inputs[0];
                int eta = int.Parse(inputs[1]);
                int score = int.Parse(inputs[2]);
                int storageA = int.Parse(inputs[3]);
                int storageB = int.Parse(inputs[4]);
                int storageC = int.Parse(inputs[5]);
                int storageD = int.Parse(inputs[6]);
                int storageE = int.Parse(inputs[7]);
                int expertiseA = int.Parse(inputs[8]);
                int expertiseB = int.Parse(inputs[9]);
                int expertiseC = int.Parse(inputs[10]);
                int expertiseD = int.Parse(inputs[11]);
                int expertiseE = int.Parse(inputs[12]);

                int[] storageNumbers = new int[5]
                {
                    storageA,
                    storageB,
                    storageC,
                    storageD,
                    storageE
                };

                if (i == 0)
                {
                    player.Eta = eta;
                    player.Score = score;
                    player.SetTarget(target);
                    player.Storage.UpdateMoleculeValues(storageNumbers);
                }
            }
            inputs = Console.ReadLine().Split(' ');
            int availableA = int.Parse(inputs[0]);
            int availableB = int.Parse(inputs[1]);
            int availableC = int.Parse(inputs[2]);
            int availableD = int.Parse(inputs[3]);
            int availableE = int.Parse(inputs[4]);
            int sampleCount = int.Parse(Console.ReadLine());

            for (int i = 0; i < sampleCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int sampleId = int.Parse(inputs[0]);
                int carriedBy = int.Parse(inputs[1]);
                int rank = int.Parse(inputs[2]);
                string expertiseGain = inputs[3];
                int health = int.Parse(inputs[4]);
                int costA = int.Parse(inputs[5]);
                int costB = int.Parse(inputs[6]);
                int costC = int.Parse(inputs[7]);
                int costD = int.Parse(inputs[8]);
                int costE = int.Parse(inputs[9]);

                int[] costNumbers = new int[5]
                {
                    costA,
                    costB,
                    costC,
                    costD,
                    costE
                };

                if (ai.Samples.DoesThisExistInBag(sampleId))
                {
                    var sample = ai.Samples.SingleSampleById(sampleId);
                    sample.CarriedBy = (Carried)carriedBy;
                }
                else
                {
                    var sample = new Sample(sampleId, carriedBy, health, costNumbers);
                    ai.Samples.AddSample(sample);
                }
            }

            ai.Run();

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
        }
    }
}

public class AI
{
    private const int _sampleLimit = 3;
    private const int _moleculeLimit = 10;

    public Robot Player { get; set; }
    public SampleBag Samples { get; set; }
    public Mode Mode { get; set; }
    public MoleculeBag CostOfCarriedSamples { get; set; }

    public AI(Robot robot)
    {
        this.Player = robot;
        this.Samples = new SampleBag();
        this.Mode = Mode.SamplePickup;
        this.CostOfCarriedSamples = new MoleculeBag();
    }

    public void Run()
    {
        if (this.Mode == Mode.SamplePickup)
        {
            this.SamplePickUpMode();
        }

        if (this.Mode == Mode.MoleculePickup)
        {
            this.MoleculePickUpMode();
        }

        if (this.Mode == Mode.ProduceMedicines)
        {
            this.ProduceMedicinesMode();
        }
    }

    public Order ProduceMedicinesMode()
    {
        if (this.Samples.NumberCarriedByPlayer() == 0)
        {
            var order = new Order()
            {
                Type = OrderType.Move,
                Position = Position.Diagnosis
            };

            this.Mode = Mode.SamplePickup;
            return order;
        }
        else
        {
            var sample = this.Samples.FirstSampleCarriedByPlayer();

            var order = new Order()
            {
                Type = OrderType.ConnectSample,
                SampleId = sample.Id
            };

            return order;
        }
    }

    public Order MoleculePickUpMode()
    {      
        if (this.Player.Storage.AtCapacity())
        {
            this.Mode = Mode.ProduceMedicines;
            return null;
        }
        else if (this.Player.Position == Position.Molecules)
        {
            var typeList = Enum.GetValues(typeof(MoleculeType)).Cast<MoleculeType>();

            foreach (var item in typeList)
            {
                var playerMoleculeCount = this.Player.Storage.GetValueOfKey(item);

                var sampleMoleculeNeeded = this.CostOfCarriedSamples.GetValueOfKey(item);

                if (sampleMoleculeNeeded > playerMoleculeCount)
                {
                    var order = new Order()
                    {
                        Type = OrderType.ConnectMolecule,
                        Molecule = item
                    };

                    return order;                    
                }
            }
        }
        else
        {
            var order = new Order()
            {
                Type = OrderType.Move,
                Position = Position.Molecules
            };

            return order;
        }

        return null;
    }

    private void SetTotalCostOfCollectedSamples()
    {
        var playerSamples = this.Samples.AllSamplesCarriedByPlayer();

        this.CostOfCarriedSamples.SetValuesToZero();

        foreach (var sample in playerSamples)
        {
            foreach (var cost in sample.Cost)
            {
                this.CostOfCarriedSamples.UpdateSingleValue(cost.Key, cost.Value);                
            }
        }

    }

    public Order SamplePickUpMode()
    {
        if (this.Samples.IsPlayerAtCapacity())
        {
            this.Mode = Mode.MoleculePickup;
            this.SetTotalCostOfCollectedSamples();
            var order = new Order()
            {
                Type = OrderType.Move,
                Position = Position.Molecules
            };

            return order;
        }
        else if (this.Player.Position == Position.Diagnosis)
        {
            var sample = this.DetermineSample();

            if (sample != null)
            {
                var order = new Order()
                {
                    Type = OrderType.ConnectSample,
                    SampleId = sample.Id
                };

                return order;
            }
            else
            {
                this.Mode = Mode.MoleculePickup;
                var order = new Order()
                {
                    Type = OrderType.Move,
                    Position = Position.Molecules
                };

                return order;
            }
        }
        else
        {
            var order = new Order()
            {
                Type = OrderType.Move,
                Position = Position.Diagnosis
            };

            return order;
        }
    }

    private Sample DetermineSample()
    {
        var sampleList = this.Samples.ListOfPossibleSamplesToTake(this.Player.Storage.RemainingCapacity());

        var sampleToTake = sampleList.FirstOrDefault();       

        return sampleToTake;
    }

    private void OrderWriter(Order order)
    {
        var orderString = string.Empty;

        switch (order.Type)
        {
            case OrderType.Move:
                orderString = "GOTO " +
                    Enum.GetName(typeof(Position), order.Position).ToString().ToUpper();
                break;
            case OrderType.ConnectSample:
                orderString = $"CONNECT {order.SampleId}";
                break;
            case OrderType.ConnectMolecule:
                orderString = "CONNECT " +
                    Enum.GetName(typeof(MoleculeType), order.Molecule).ToString().ToUpper();
                break;
            default:
                throw new ArgumentOutOfRangeException("typeOfOrder", "Argument is not between 0-2");
        }

        Console.WriteLine(orderString);
    }
}

public class Order
{
    public OrderType Type { get; set; }
    public Position? Position { get; set; }
    public int? SampleId { get; set; }
    public MoleculeType? Molecule { get; set; }
}

public class Robot
{
    public Position Position { get; set; }
    public int Eta { get; set; }
    public int Score { get; set; }
    public MoleculeBag Storage { get; set; }

    public Robot(string target, int eta, int score, int[] storageNumbers)
    {
        this.SetTarget(target);
        this.Eta = eta;
        this.Score = score;
        this.Storage = new MoleculeBag();
        this.Storage.UpdateMoleculeValues(storageNumbers);
    }

    public void SetTarget(string target)
    {
        switch (target)
        {
            case "DIAGNOSIS":
                this.Position = Position.Diagnosis;
                break;
            case "MOLECULES":
                this.Position = Position.Molecules;
                break;
            case "LABORATORY":
                this.Position = Position.Laboratory;
                break;
            case "START_POS":
                this.Position = Position.StartPos;
                break;
            default:
                throw new ArgumentOutOfRangeException("target is out of range");
        }
    }    
}

public class Sample
{
    public int Id { get; set; }
    public Carried CarriedBy { get; set; }
    public int HealthPoints { get; set; }
    public MoleculeBag Cost { get; set; }
    public float MoleculeValue
    {
        get
        {
            float totalCost = this.Cost.TotalNumber();
            return this.HealthPoints / totalCost;
        }
    }

    public Sample(int id, int carriedBy, int health, int[] costNumbers)
    {
        this.Id = id;
        this.CarriedBy = (Carried)carriedBy;
        this.HealthPoints = health;
        this.Cost = new MoleculeBag();
        this.Cost.UpdateMoleculeValues(costNumbers);
    }    
}

public class MoleculeBag : IEnumerable<KeyValuePair<MoleculeType, int>>
{
    private Dictionary<MoleculeType, int> _molecules;
    private const int moleculeLimit = 10;  

    public MoleculeBag()
    {
        this._molecules = new Dictionary<MoleculeType, int>()
        {
            { MoleculeType.A, 0 },
            { MoleculeType.B, 0 },
            { MoleculeType.C, 0 },
            { MoleculeType.D, 0 },
            { MoleculeType.E, 0 }
        };
    }

    public void UpdateMoleculeValues(int[] moleculeValues)
    {
        var total = moleculeValues.Sum();

        if (total > moleculeLimit)
        {
            throw new ArgumentOutOfRangeException("moleculeValues", "Too many molecules!");
        }

        foreach (var key in this._molecules.Keys.ToList())
        {
            this._molecules[key] = moleculeValues[(int)key];
        }
    }

    public void SetValuesToZero()
    {
        foreach (var key in this._molecules.Keys.ToList())
        {
            this._molecules[key] = 0;
        }
    }

    public void UpdateSingleValue(MoleculeType moleculeType, int value)
    {
        this._molecules[moleculeType] += value;
    }

    public int GetValueOfKey(MoleculeType moleculeType)
    {
        return this._molecules.Single(s => s.Key == moleculeType).Value;
    }

    public int RemainingCapacity()
    {
        return moleculeLimit - this.TotalNumber();
    }

    public int TotalNumber()
    {      
        return this._molecules.Sum(s => s.Value);        
    }

    public bool AtCapacity()
    {
        return this.TotalNumber() == moleculeLimit;
    }

    public IEnumerator<KeyValuePair<MoleculeType, int>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<MoleculeType, int>>)this._molecules).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<MoleculeType, int>>)this._molecules).GetEnumerator();
    }
}

public class SampleBag
{
    private List<Sample> _samples;
    private const int sampleLimit = 3;    

    public SampleBag()
    {
        this._samples = new List<Sample>();
    }

    public bool DoesThisExistInBag(int id)
    {
        return this._samples.Any(s => s.Id == id);
    }

    public Sample SingleSampleById(int id)
    {
        return this._samples.SingleOrDefault(s => s.Id == id);
    }

    public Sample FirstSampleCarriedByPlayer()
    {
        return this._samples.FirstOrDefault(s => s.CarriedBy == Carried.Player);
    }

    public IEnumerable<Sample> AllSamplesCarriedByPlayer()
    {
        return this._samples.Where(s => s.CarriedBy == Carried.Player);
    }

    public void AddSample(Sample sample)
    {
        this._samples.Add(sample);
    }

    public int NumberCarriedByPlayer()
    {
        return this._samples.Count(s => s.CarriedBy == Carried.Player);
    }

    public bool IsPlayerAtCapacity()
    {
        return this.NumberCarriedByPlayer() == sampleLimit;
    }

    public IEnumerable<Sample> ListOfPossibleSamplesToTake(int remainingCapacity)
    {
        return this._samples
            .Where(s => s.CarriedBy == Carried.Cloud)
            .Where(s => s.Cost.TotalNumber() < remainingCapacity)
            .OrderByDescending(s => s.MoleculeValue);
    }    
}

public enum Position
{
    Diagnosis,
    Molecules,
    Laboratory,
    StartPos
}

public enum Mode
{
    SamplePickup,
    MoleculePickup,
    ProduceMedicines
}

public enum Carried
{
    Cloud = -1,
    Player = 0,
    Other = 1
}

public enum OrderType
{
    Move = 0,
    ConnectSample = 1,
    ConnectMolecule = 2
}

public enum MoleculeType
{
    A,
    B,
    C,
    D,
    E
}
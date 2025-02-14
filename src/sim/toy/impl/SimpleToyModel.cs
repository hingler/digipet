using digipet.framework;
using digipet.world;

namespace digipet.sim.toy.impl;
public class SimpleToyModel : IToyModel {
  private readonly Dictionary<int, ToyInterest> toys = [];

  private readonly IEngine engine;

  public SimpleToyModel(IEngine engine, ToyModelData data) {
    this.engine = engine;
    foreach (KeyValuePair<int, ToyInterest> pair in data.toys) {
      toys.Add(pair.Key, pair.Value);
    }
  }

  public ToyModelData AsData() {
    return new ToyModelData(toys);
  }



  public double GetRawInterest(IToy toy) {
    UpdateToyList();
    return FetchInterestData(toy).CurrentEnjoyment;
  }

  public void Tick(int tick_seconds) {
    UpdateToyList();
    HashSet<int> spawned_rids = [];
    // get list of all spawned RIDs
    foreach (IToy toy in GetSpawnedToys()) {
      spawned_rids.Add(toy.RID);
    }

    // if the RID is not spawned, then increment
    foreach (KeyValuePair<int, ToyInterest> ti in toys) {
      ToyInterest interest = ti.Value;
      // if not spawned, replenish interest
      if (!spawned_rids.Contains(ti.Key)) {
        interest.CurrentEnjoyment = Math.Min(
          interest.CurrentEnjoyment + (tick_seconds / interest.RegenRate),
          interest.BaseEnjoyment
        );
      }
    }
  }

  private HashSet<IToyActivator> GetSpawnedToys_Internal() {
    HashSet<IToyActivator> toys = [];
    foreach (IPhysObject o in engine.GetPhysWorld().GetPhysObjects()) {
      if (o.Pickup is IToyActivator toy) {
        toys.Add(toy);
        if (!this.toys.ContainsKey(toy.RID)) {
          FetchInterestData(toy);
        }
      }
    }

    return toys;
  }

  public bool IsActive(IToy toy) {
    HashSet<IToyActivator> activators = GetSpawnedToys_Internal();
    foreach (IToyActivator a in activators) {
      if (a.RID == toy.RID && a.Active) {
        return true;
      }
    }

    return false;
  }

  public IEnumerable<IToyActivator> GetSpawnedToys() {
    return GetSpawnedToys_Internal();
  }

  public IEnumerable<IToyActivator> GetActiveToys() {
    HashSet<IToyActivator> toys = GetSpawnedToys_Internal();
    toys.RemoveWhere(t => !t.Active);
    return toys;
  }

  public double ConsumeInterest(int ticks) {
    double net_interest = 0.0;
    foreach (IToyActivator toy in GetActiveToys()) {
      ToyInterest interest = FetchInterestData(toy);
      double tick_consume = ticks * (1.0 / interest.ConsumeRate);
      double toy_consume = Math.Min(
        tick_consume, 
        interest.CurrentEnjoyment
      );

      if (toy_consume <= 0.0) {
        // sick of this; add a decrement
        net_interest -= tick_consume / 6;
      } else {
        // still interested - tack on interest, and deduct from enjoy
        net_interest += toy_consume;
        interest.CurrentEnjoyment -= toy_consume;
      }
    }

    return net_interest;
  }


  public double ConsumeInterest(IToy toy, double amt) {
    ToyInterest interest = FetchInterestData(toy);
    // linear falloff?
    double requested_enjoyment = Math.Min(
      interest.CurrentEnjoyment,
      amt
    );

    interest.CurrentEnjoyment -= requested_enjoyment;

    return requested_enjoyment;
  }

  public void UpdateToyList() {
    // should fetch interest data as we go
    GetSpawnedToys();
  }

  private ToyInterest FetchInterestData(IToy toy) {
    if (!toys.TryGetValue(toy.RID, out ToyInterest res)) {
      res = new ToyInterest() {
        BaseEnjoyment = toy.MaxInterest,
        RegenRate = 3600,
        ConsumeRate = toy.ConsumeTime,
        CurrentEnjoyment = toy.MaxInterest
      };

      toys[toy.RID] = res;
    }
    
    return res;
  }
}
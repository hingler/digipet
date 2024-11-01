using System.Collections.Generic;
using System.Linq;

namespace digipet.sim.db;

#nullable enable

public class SimpleSimRepo<T> : ISimRepo<T> where T : IWorldItem {
  private readonly Dictionary<int, T> contents = [];

  public SimpleSimRepo(
    params T[] args
  ) : this(args.ToList()) {}

  public SimpleSimRepo(
    IList<T> args
  ) {
    for (int i = 0; i < args.Count; i++) {
      T n = args[i];
      contents[i] = n;
    }
  }

  public IReadOnlyCollection<T> GetEntries() {
    return contents.Values;
  }

  public IReadOnlyCollection<int> GetDescriptors() {
    return contents.Keys;
  }

  public T Fetch(int descriptor) {
    return contents[descriptor];
  }

  // idea: method to sample random, given seed? 
  // expect implementers to handle as they prefer


  public bool TryFetch(int descriptor, out T? output) {
    return contents.TryGetValue(descriptor, out output);
  }
}
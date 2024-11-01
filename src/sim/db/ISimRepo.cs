using System.Collections.Generic;

namespace digipet.sim.db;

#nullable enable

public interface ISimRepo<T> where T : IWorldItem {
  // returns a list of all entries stored in this DB
  IReadOnlyCollection<T> GetEntries();

  // returns a list of int descriptors for each item
  // (thinking: descriptors makes remap trivial)
  IReadOnlyCollection<int> GetDescriptors();

  // fetches item associated with descriptor, if it exists
  T Fetch(int descriptor);
  bool TryFetch(int descriptor, out T? output);
}
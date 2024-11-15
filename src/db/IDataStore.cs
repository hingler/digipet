using System;
using System.Runtime.Serialization;

namespace digipet.db;

#nullable enable

public interface IDataStore {
  // stores arbitrary data
  void Store(string index, object data);
  T? Fetch<T>(string index) where T : class;
  bool TryFetch<T>(string index, out T? output) where T : class;
  // tryfetch
  bool Contains<T>(string index);
  // fetches a sub-store which is scoped to a given prefix
  IDataStore GetSubspace(string prefix);
}
using System;
using System.Runtime.Serialization;

namespace digipet.db;

#nullable enable

public interface IDataStore {
  // stores arbitrary data
  void Store(string index, object data);
  T? Fetch<T>(string index) where T : class;
  // fetches a sub-store which is scoped to a given prefix
  IDataStore GetSubspace(string prefix);
}
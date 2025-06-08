using System.Runtime.Serialization;

namespace digipet.db;

#nullable enable

public class SubDataStore : IDataStore {
  private readonly string prefix;
  private readonly IDataStore superstore;

  public SubDataStore(string prefix, IDataStore superstore) {
    this.prefix = prefix;
    this.superstore = superstore;
  }

  public void Store(string index, object data) {
    superstore.Store(prefix + index, data);
  }

  public void StoreLong(string index, long data) {
    superstore.Store(prefix + index, data);
  }

  public bool TryFetchLong(string index, out long data) {
    return superstore.TryFetchLong(prefix + index, out data);
  }

  public T? Fetch<T>(string index) where T : class {
    return superstore.Fetch<T>(prefix + index);
  }

  public bool TryFetch<T>(string index, out T? output) where T : class {
    return superstore.TryFetch<T>(prefix + index, out output);
  }

  public bool Contains<T>(string index) {
    return superstore.Contains<T>(prefix + index);
  }

  public IDataStore GetSubspace(string prefix) {
    // string concat vs recursion
    return new SubDataStore(this.prefix + prefix + ".", superstore);
  }
}
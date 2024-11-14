using System.Runtime.Serialization;

namespace digipet.db;

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

  public T Fetch<T>(string index) where T : class {
    return superstore.Fetch<T>(prefix + index);
  }

  public IDataStore GetSubspace(string prefix) {
    // string concat vs recursion
    return new SubDataStore(this.prefix + "." + prefix + ".", superstore);
  }
}
namespace digipet.sim.db;

#nullable enable

public class DataRepo<T> : ISimRepo<T> where T : IWorldItem {
  private readonly Dictionary<int, T> entries;

  public DataRepo() {
    entries = [];
  }

  public void AddItem(T item) {
    entries.TryAdd(item.RID, item);
  }

  public IReadOnlyCollection<T> GetEntries() {
    return entries.Values;
  }

  public IReadOnlyCollection<int> GetDescriptors() {
    return entries.Keys;
  }

  public T Fetch(int descriptor) {
    return entries[descriptor];
  }

  public bool TryFetch(int descriptor, out T? output) {
    return entries.TryGetValue(descriptor, out output);
  }
}
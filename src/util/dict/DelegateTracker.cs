namespace digipet.util.dict;

public static class DelegateTracker {
  public static void RemoveAll<T, U>(this Dictionary<T, U> source, IEnumerable<T> removes, Action<U> cleanup_func) where T : notnull {
    foreach (T key in removes) {
      cleanup_func(source[key]);
      source.Remove(key);
    }
  } 

  public delegate T Factory<T, U>(U template);
  public static void AddAll<T, U>(this Dictionary<T, U> dest, IEnumerable<T> new_keys, Factory<U, T> factory_func) where T : notnull {
    foreach (T key in new_keys) {
      U map = factory_func(key);
      dest.Add(key, map);
    }
  }
}
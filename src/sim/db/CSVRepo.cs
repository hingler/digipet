using System;
using System.Collections.Generic;
using digipet.file;
using digipet.file.csv;

namespace digipet.sim.db;

#nullable enable

// repo generated from a csv file
public class CSVRepo<T> : ISimRepo<T> where T : IWorldItem {
  private readonly SortedDictionary<int, T> values;
  public CSVRepo(
    ICSVConverter<T> converter,
    string path,
    IFileLoader loader
  ) {
    values = [];
    ReadContents(
      converter,
      loader.Load(path, FileFlags.Read)
    );
  }

  public IReadOnlyCollection<T> GetEntries() {
    return values.Values;
  }

  public IReadOnlyCollection<int> GetDescriptors() {
    return values.Keys;
  }

  public T Fetch(int descriptor) {
    return values[descriptor];
  }

  public bool TryFetch(int descriptor, out T? output) {
    return values.TryGetValue(descriptor, out output);
  }

  // impl

  private void ReadContents(
    ICSVConverter<T> converter,
    IFileHandle csv_file
  ) {
    IList<T> list = CSVHandler.FetchFromFile<T>(converter, csv_file);
    foreach (T item in list) {
      values.Add(item.RID, item);
    }
  }
}
using System.Collections.Generic;
using System.Linq;
using static digipet.util.Closure;

namespace digipet.file.csv;

#nullable enable

public static class CSVHandler {
  public static IList<T> FetchFromFile<T>(
    ICSVConverter<T> converter,
    IFileHandle file
  ) {

    IList<T> results = [];
    IList<string> columns = 
      file.GetLine().Split(',').Select(s => s.Trim()).ToList();
    while (!file.Eof()) {
      string line  = file.GetLine().Trim();
      IList<string> elements = line.Split(',').Select(s => s.Trim()).ToList();
      T? element = converter.Parse(elements);
      element?.Let(results.Add);
    }

    return results;
  }
}
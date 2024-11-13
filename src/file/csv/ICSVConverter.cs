using System.Collections.Generic;

namespace digipet.file.csv;

#nullable enable

public interface ICSVConverter<T> {
  // parse a single CSV line
  T? Parse(IList<string> args);
}
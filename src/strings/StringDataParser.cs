using System.Text.RegularExpressions;

namespace digipet.strings;

#nullable enable

public class StringDataParser {
  private readonly string content;
  private readonly Regex regex;
  private readonly Regex regex_spaces;
  private int cursor;

  private static readonly string PATTERN_TAG = @"^\[(\s*(\w+)\s*=(\w+)\s*)+]$";
  // 2+ space chars, or newline
  private static readonly string SPACE_TAG = @"\s{2,}|(\n|\r\n)";
  public StringDataParser(string content) {
    this.content = content;
    regex = new Regex(PATTERN_TAG, RegexOptions.Multiline);
    regex_spaces = new Regex(SPACE_TAG);
    cursor = 0;
  }

  private Match GetNextTag(int start_index) {
    return regex.Match(content, start_index);
  }

  public StringDataTag? GetNextResult() {
    if (cursor < 0) {
      return null;
    }

    Match tag = GetNextTag(cursor);
    if (!tag.Success) {
      return null;
    }

    cursor = tag.Index + tag.Length;

    Match next_tag = GetNextTag(cursor);

    string data;
    if (!next_tag.Success) {
      data = content[cursor..].Trim();
    } else {
      int item_end = next_tag.Index - 1;
      data = content[cursor..item_end].Trim();
    }


    data = regex_spaces.Replace(data, " ");
    

    cursor = next_tag.Index - 1;

    Dictionary<string, string> attribs = [];

    var keys = tag.Groups[2].Captures;
    var values = tag.Groups[3].Captures;
    for (int i = 0; i < keys.Count && i < values.Count; i++) {
      attribs.Add(keys[i].Value, values[i].Value);
    }

    return new(attribs, data);
  }
}


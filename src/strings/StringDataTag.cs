namespace digipet.strings;

public class StringDataTag {
  public readonly IReadOnlyDictionary<string, string> attributes;
  public readonly string text_content;

  public StringDataTag(
    IReadOnlyDictionary<string, string> attributes,
    string text_content
  ) {
    this.attributes = attributes;
    this.text_content = text_content;
  }
}
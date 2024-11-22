using digipet.strings;

namespace digipet.tests.strings;

public class StringDataParserTest {
  private static string test_data = @"[RID=1]
testone
[RID=2]
testtwo";

  [Test]
  public void TestSimpleParse() {
    StringDataParser parser = new(test_data);

    var result_a = parser.GetNextResult();
    var result_b = parser.GetNextResult();
    var result_null = parser.GetNextResult();

    Assert.That(result_a, Is.Not.Null);
    Assert.That(result_b, Is.Not.Null);
    Assert.That(result_null, Is.Null);

    if (result_a != null && result_b != null) {
      Assert.That(result_a.text_content, Is.EqualTo("testone"));
      Assert.That(result_b.text_content, Is.EqualTo("testtwo"));

      Assert.That(result_a.attributes.ContainsKey("RID"), Is.True);
      Assert.That(result_b.attributes.ContainsKey("RID"), Is.True);
    }

  }
}
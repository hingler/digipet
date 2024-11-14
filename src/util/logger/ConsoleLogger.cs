namespace digipet.util;

public class ConsoleLogger : ILogger {
  private readonly TextWriter writer;
  private readonly TextWriter writer_err;
  public ConsoleLogger() : this(Console.Out, Console.Error) {}
  public ConsoleLogger(TextWriter writer, TextWriter writer_err) {
    this.writer = writer;
    this.writer_err = writer_err;
  }

  public void Log(params object[] args) {
    foreach (object o in args) {
      writer.Write(o);
    }

    writer.WriteLine("");
  }

  public void Error(params object[] args) {
    foreach (object o in args) {
      writer_err.Write(o);
    }

    writer_err.WriteLine("");
  }
}
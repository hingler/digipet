using System.Globalization;

namespace digipet.util;

public class ClassAwareLogger : ILogger {
  private readonly ILogger logger;
  private readonly string classIdentifier;

  public ClassAwareLogger(ILogger logger, Type classType) {
    this.logger = logger;
    classIdentifier = classType.FullName;
  }

  private static string GetTimecode() {
    return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
  }

  private object[] GetArgList(object[] args) {
    return [
      "[ ", GetTimecode(), " | ", classIdentifier, " ] - ",
      ..args
    ];
  }

  public void Log(params object[] args) {
    logger.Log(args: GetArgList(args));
  }

  public void Error(params object[] args) {
    logger.Error(args: GetArgList(args));
  }
}
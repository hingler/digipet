namespace digipet.util;

public static class LoggerSingleton {
  private static readonly ConsoleLogger defaultLogger = new();
  private static ILogger logger = null;

  public static void SetLogger(ILogger logger) {
    LoggerSingleton.logger = logger;
  }

  public static ILogger GetLogger() {
    if (logger != null) {
      return logger;
    }

    return defaultLogger;
  }

  public static ILogger GetLogger(this object o) {
    return GetLogger(o.GetType());
  }

  public static ILogger GetStaticLogger<T>() {
    return new ClassAwareLogger(GetLogger(), typeof(T));
  }

  public static ILogger GetLogger(Type t) {
    return new ClassAwareLogger(GetLogger(), t);
  }
}
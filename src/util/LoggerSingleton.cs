namespace digipet.util;

public static class LoggerSingleton {
  private static ILogger logger = new ConsoleLogger();

  public static void SetLogger(ILogger logger) {
    LoggerSingleton.logger = logger;
  }

  public static ILogger GetLogger() {
    return logger;
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
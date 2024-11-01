namespace digipet.util;

public static class LoggerSingleton {
  public delegate ILogger LoggerFactoryFunc();

  private static LoggerFactoryFunc f;

  public static void SetFactoryMethod(LoggerFactoryFunc d) {
    f = d;
  }

  public static ILogger GetLogger() {
    return f();
  }
}
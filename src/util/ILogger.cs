using System;

namespace digipet.util;

public interface ILogger {
  void Log(params object[] args);
  void Error(params object[] args);
}
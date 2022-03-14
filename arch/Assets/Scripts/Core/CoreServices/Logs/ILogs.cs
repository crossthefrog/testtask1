using System;

namespace Arch.CoreServices
{
  internal interface ILogs : IDisposable
  {
    void Log(string message);
    void LogError(string message);
  }
}
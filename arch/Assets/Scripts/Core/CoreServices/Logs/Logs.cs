using DefaultNamespace;
using UnityEngine;

namespace Arch.CoreServices
{
  internal class Logs : BaseDisposable, ILogs
  {
    public void Log(string message)
    {
      Debug.Log(message);
    }

    public void LogError(string message)
    {
      Debug.LogError(message);
    }
  }
}
using System;
using Object = UnityEngine.Object;

namespace Arch.Loaders
{
  internal interface IContentLoader : IDisposable
  {
    void LoadPrefab<T>(string path, Action<T> onComplete) where T : Object;
  }
}
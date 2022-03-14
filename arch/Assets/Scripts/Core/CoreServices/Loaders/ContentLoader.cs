using System;
using DefaultNamespace;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Arch.Loaders
{
  internal class ContentLoader : BaseDisposable, IContentLoader
  {
    public void LoadPrefab<T>(string path, Action<T> onComplete) where T : Object
    {
      T obj = Resources.Load<T>(path);
      onComplete?.Invoke(obj);
    }
  }
}
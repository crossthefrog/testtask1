using System;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
  internal static class DisposeExtensions
  {
    public static void AddTo(this IDisposable disposable, BaseDisposable baseDisposable)
    {
      baseDisposable.AddDisposable(disposable);
    }
    
    public static void AddTo<T>(this T obj, BaseDisposable baseDisposable) where T : Object
    {
      baseDisposable.AddDisposable(obj);
    }
  }
}
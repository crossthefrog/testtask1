using System;
using Object = UnityEngine.Object;

namespace Arch.CoreServices
{
  internal interface IInstantiator : IDisposable
  {
    T CreateOnScene<T>(T gameObject) where T : Object;
    void Destroy<T>(T gameObject) where T : Object;
  }
}
using DefaultNamespace;
using UnityEngine;

namespace Arch.CoreServices
{
  internal class Instantiator : BaseDisposable, IInstantiator
  {
    public struct Ctx
    {
      public Transform parent;
    }

    private readonly Ctx _ctx;

    public Instantiator(Ctx ctx)
    {
      _ctx = ctx;
    }
    
    public T CreateOnScene<T>(T gameObject) where T : Object
    {
      T obj = Object.Instantiate(gameObject, _ctx.parent);
      return obj;
    }

    public void Destroy<T>(T gameObject) where T : Object
    {
      Object.Destroy(gameObject);
    }
  }
}
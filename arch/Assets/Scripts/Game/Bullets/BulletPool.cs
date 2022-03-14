using Arch.CoreServices;
using UniRx.Toolkit;

namespace Game
{
  internal class BulletPool : ObjectPool<BulletView>
  {
    public struct Ctx
    {
      public BulletView bulletViewPrefab;
      public IInstantiator instantiator;
    }

    private readonly Ctx _ctx;

    public BulletPool(Ctx ctx)
    {
      _ctx = ctx;
    }
    
    protected override BulletView CreateInstance()
    {
      return _ctx.instantiator.CreateOnScene(_ctx.bulletViewPrefab);
    }
  }
}
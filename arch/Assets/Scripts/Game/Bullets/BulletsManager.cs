using System.Collections.Generic;
using Arch.CoreServices;
using DefaultNamespace;
using UniRx;
using UniRx.Triggers;

namespace Game
{
  internal class BulletsManager : BaseDisposable
  {
    public struct Ctx
    {
      public IInstantiator instantiator;
      public IReadOnlyReactiveProperty<BulletInfo> bulletTrigger;
    }

    private readonly Ctx _ctx;
    private readonly Dictionary<BulletView, BulletPool> _pools;

    public BulletsManager(Ctx ctx)
    {
      _ctx = ctx;
      _pools = new Dictionary<BulletView, BulletPool>();
      _ctx.bulletTrigger.SkipLatestValueOnSubscribe().Subscribe(OnBulletRequest).AddTo(this);
    }

    private void OnBulletRequest(BulletInfo bulletInfo)
    {
      if (!_pools.TryGetValue(bulletInfo.bulletPrefab, out BulletPool pool))
      {
        BulletPool.Ctx poolCtx = new BulletPool.Ctx
        {
          bulletViewPrefab = bulletInfo.bulletPrefab,
          instantiator = _ctx.instantiator,
        };
        pool = new BulletPool(poolCtx);
        _pools.Add(bulletInfo.bulletPrefab, pool);
      }

      BulletView bulletView = pool.Rent();
      bulletView.transform.position = bulletInfo.origin;
      BulletView.Ctx bulletCtx = new BulletView.Ctx
      {
        onHit = () => pool.Return(bulletView),
        damage = bulletInfo.damage,
        velocity =  bulletInfo.direction * 12,
      };
      bulletView.SetCtx(bulletCtx);
    }
  }
}
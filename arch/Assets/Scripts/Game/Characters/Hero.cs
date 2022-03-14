using System.Collections.Generic;
using Arch;
using DefaultNamespace;
using Game.Healths;
using UniRx;
using UnityEngine;

namespace Game
{
  internal class Hero : BaseDisposable
  {
    public struct Ctx
    {
      public HeroView heroView;
      public float maxHp;
      public float damage;
      public float fireRate;
      public float moveSpeed;
      public IReadOnlyList<Transform> potentialTargets;
      public ReactiveProperty<BulletInfo> bulletTrigger;
    }

    private readonly Ctx _ctx;

    public Hero(Ctx ctx)
    {
      _ctx = ctx;
      ReactiveProperty<float> currentHp = new ReactiveProperty<float>(_ctx.maxHp);
      Health.Ctx healthCtx = new Health.Ctx
      {
        currentHp = currentHp,
        healthView = _ctx.heroView.HealthView,
        onDeath = Dispose,
      };
      Health health = new Health(healthCtx);
      health.AddTo(this);
      
      ReactiveProperty<bool> allowAttack = new ReactiveProperty<bool>();
      DamageDealer.Ctx dmgCtx = new DamageDealer.Ctx
      {
        allowAttack = allowAttack,
        damage = _ctx.damage,
        fireRate = _ctx.fireRate,
        myTransform = _ctx.heroView.transform,
        potentialTargets = _ctx.potentialTargets,
        damageView = _ctx.heroView.DamageView,
        bulletTrigger = _ctx.bulletTrigger,
      };
      DamageDealer dealer = new DamageDealer(dmgCtx);
      dealer.AddTo(this);

      BaseMove.BaseCtx baseCtx = new BaseMove.BaseCtx
      {
        moveView = _ctx.heroView.MoveView,
        allowAttack = allowAttack,
        moveSpeed = _ctx.moveSpeed,
        myTransform = _ctx.heroView.transform,
      };
      HeroMoves.Ctx moveCtx = new HeroMoves.Ctx
      {
        moveView = _ctx.heroView.MoveView,
      };
      HeroMoves heroMoves = new HeroMoves(moveCtx, baseCtx);
      heroMoves.AddTo(this);
      
    }
  }
}
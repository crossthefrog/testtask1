using System;
using System.Collections.Generic;
using Arch;
using DefaultNamespace;
using Game.Healths;
using UniRx;
using UnityEngine;

namespace Game.Characters
{
  internal class Enemy : BaseDisposable
  {
    public struct Ctx
    {
      public EnemyView enemyView;
      public float maxHp;
      public float damage;
      public float fireRate;
      public float moveSpeed;
      public float freezeTime;
      public float moveDistance;
      public IReadOnlyList<Transform> potentialTargets;
      public ReactiveProperty<BulletInfo> bulletTrigger;
      public Action onDeath;
    }

    private readonly Ctx _ctx;

    public Enemy(Ctx ctx)
    {
      _ctx = ctx;
      ReactiveProperty<float> currentHp = new ReactiveProperty<float>(_ctx.maxHp);
      Health.Ctx healthCtx = new Health.Ctx
      {
        currentHp = currentHp,
        healthView = _ctx.enemyView.HealthView,
        onDeath = () =>
        {
          _ctx.onDeath?.Invoke();
          Dispose();
        },
      };
      Health health = new Health(healthCtx);
      health.AddTo(this);

      ReactiveProperty<bool> allowAttack = new ReactiveProperty<bool>();
      DamageDealer.Ctx dmgCtx = new DamageDealer.Ctx
      {
        allowAttack = allowAttack,
        damage = _ctx.damage,
        fireRate = _ctx.fireRate,
        myTransform = _ctx.enemyView.transform,
        potentialTargets = _ctx.potentialTargets,
        damageView = _ctx.enemyView.DamageView,
        bulletTrigger = _ctx.bulletTrigger,
      };
      DamageDealer dealer = new DamageDealer(dmgCtx);
      dealer.AddTo(this);

      BaseMove.BaseCtx baseCtx = new BaseMove.BaseCtx
      {
        moveView = _ctx.enemyView.EnemyMoveView,
        allowAttack = allowAttack,
        moveSpeed = _ctx.moveSpeed,
        myTransform = _ctx.enemyView.transform,
      };
      EnemyMoves.Ctx moveCtx = new EnemyMoves.Ctx
      {
        freezeTime = _ctx.freezeTime,
        moveDistance = _ctx.moveDistance,
      };
      EnemyMoves enemyMoves = new EnemyMoves(moveCtx, baseCtx);
      enemyMoves.AddTo(this);
    }
  }
}
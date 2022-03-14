using System;
using System.Collections.Generic;
using DefaultNamespace;
using UniRx;
using UnityEngine;

namespace Game
{
  internal class DamageDealer : BaseDisposable
  {
    public struct Ctx
    {
      public IReadOnlyReactiveProperty<bool> allowAttack;
      public float damage;
      public float fireRate;
      public IReadOnlyList<Transform> potentialTargets;
      public Transform myTransform;
      public ReactiveProperty<BulletInfo> bulletTrigger;
      public DamageView damageView;
    }

    private readonly Ctx _ctx;
    private IDisposable _attackCycle;
    private IDisposable _targetingCycle;
    private float _timePass;
    private Transform _nearTarget;

    public DamageDealer(Ctx ctx)
    {
      _ctx = ctx;
      _ctx.allowAttack.Subscribe(OnAllowAttack).AddTo(this);
    }

    private void OnAllowAttack(bool allow)
    {
      _attackCycle?.Dispose();
      _targetingCycle?.Dispose();
      if (allow)
      {
        _attackCycle = Observable.EveryUpdate().Subscribe(AttackCycle);
        _attackCycle.AddTo(this);
        _targetingCycle = Observable.EveryUpdate().Subscribe(_ => TargetingCycle());
        _targetingCycle.AddTo(this);
      }
    }

    private void TargetingCycle()
    {
      _nearTarget = null;
      float minDistance = float.MaxValue;
      foreach (Transform target in _ctx.potentialTargets)
      {
        if (!target)
          continue;
        float dist = Vector3.Distance(_ctx.myTransform.position, target.position);
        if (dist < minDistance)
        {
          minDistance = dist;
          _nearTarget = target;
        }
      }
      if (_nearTarget != null)
      {
        _ctx.myTransform.forward = _nearTarget.position - _ctx.myTransform.position;
      }
    }

    private void AttackCycle(long tick)
    {
      if (_timePass < 1 / _ctx.fireRate)
      {
        _timePass += Time.deltaTime;
        return;
      }
      _timePass = 0;

      if (_nearTarget != null)
      {
        // do attack
        _ctx.bulletTrigger.SetValueAndForceNotify(new BulletInfo
        {
          direction = _ctx.myTransform.forward,
          damage = _ctx.damage,
          bulletPrefab = _ctx.damageView.BulletPrefab,
          origin = _ctx.damageView.shotTransform.position,
        });
      }
    }
  }
}
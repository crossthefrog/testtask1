using System;
using DefaultNamespace;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game.Healths
{
  internal class Health : BaseDisposable
  {
    public struct Ctx
    {
      public HealthView healthView;
      public ReactiveProperty<float> currentHp;
      public Action onDeath;
    }

    private readonly Ctx _ctx;

    public Health(Ctx ctx)
    {
      _ctx = ctx;
      _ctx.currentHp.Subscribe(HpChanged).AddTo(this);
      _ctx.healthView.Detector.OnTriggerEnterAsObservable().Subscribe(OnHit).AddTo(this);
    }

    private void HpChanged(float hp)
    {
      if (hp < 0)
        _ctx.onDeath?.Invoke();
    }

    private void OnHit(Collider collider)
    {
      BulletView bulletView = collider.GetComponentInParent<BulletView>();
      if (bulletView)
      {
        _ctx.currentHp.Value -= bulletView.Damage;
      }
      HealthView healthView = collider.GetComponent<HealthView>();
      if (healthView)
      {
        _ctx.currentHp.Value -= healthView.DamageOnTouch;
      }
    }
  }
}
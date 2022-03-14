using System;
using UnityEngine;

namespace Game
{
  internal class BulletView : MonoBehaviour
  {
    public struct Ctx
    {
      public Action onHit;
      public float damage;
      public Vector3 velocity;
    }
    
    [SerializeField] private Collider detector;
    [SerializeField] private Rigidbody rigidbody;

    public float Damage
      => _ctx.damage;

    private Ctx _ctx;

    public void SetCtx(Ctx ctx)
    {
      _ctx = ctx;
      rigidbody.velocity = _ctx.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
      _ctx.onHit?.Invoke();
    }
  }
}
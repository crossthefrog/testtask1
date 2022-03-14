using System;
using DefaultNamespace;
using UniRx;
using UnityEngine;

namespace Arch
{
  internal abstract class BaseMove : BaseDisposable
  {
    public struct BaseCtx
    {
      public BaseMoveView moveView;
      public ReactiveProperty<bool> allowAttack;
      public float moveSpeed;
      public Transform myTransform;
    }

    protected readonly BaseCtx baseCtx;
    protected readonly ReactiveProperty<Vector3> moveDir;

    protected BaseMove(BaseCtx baseCtx)
    {
      this.baseCtx = baseCtx;
      moveDir = new ReactiveProperty<Vector3>();
      moveDir.Subscribe(MoveChanged).AddTo(this);
      Observable.EveryFixedUpdate().Subscribe(_ => ProcessMove()).AddTo(this);
    }

    private void MoveChanged(Vector3 dir)
    {
      baseCtx.allowAttack.Value = Math.Abs(dir.magnitude) < 0.01f;
    }

    private void ProcessMove()
    {
      // как то двигаемся
      Vector3 direction = moveDir.Value;
      Vector3 projection = direction.Projection().normalized;
      Vector3 correction = baseCtx.moveView.moveCoeff * baseCtx.moveSpeed * projection;
      baseCtx.moveView.rigidbody.velocity = correction;
      if (projection != Vector3.zero)
        baseCtx.myTransform.forward = projection;
    }
  }
}
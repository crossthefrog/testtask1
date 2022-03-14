using DefaultNamespace;
using UniRx;
using UnityEngine;

namespace Arch
{
  // тут надо написать логику хождения, желательно по нав мешу, но так как это все таки тестовое,
  // то я сделал рандом, но его легко заполнить логикой и простой, и сложной, короче todo
  internal class EnemyMoves : BaseMove
  {
    public struct Ctx
    {
      public float freezeTime;
      public float moveDistance;
    }

    private readonly Ctx _ctx;
    private readonly float _moveTime;
    private float _timePass;

    public EnemyMoves(Ctx ctx, BaseCtx baseCtx) : base(baseCtx)
    {
      _ctx = ctx;
      _moveTime = _ctx.moveDistance / baseCtx.moveSpeed;
      Observable.EveryUpdate().Subscribe(_ => RandomMoves()).AddTo(this);
    }

    private void RandomMoves()
    {
      if (moveDir.Value == Vector3.zero)
      {
        // ждем пока стоим на месте
        if (_timePass < _ctx.freezeTime)
        {
          _timePass += Time.deltaTime;
          return;
        }
        _timePass = 0;
        moveDir.Value = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
      }
      else
      {
        // ждем пока ходим
        if (_timePass < _moveTime)
        {
          _timePass += Time.deltaTime;
          return;
        }
        _timePass = 0;
        moveDir.Value = Vector3.zero;
      }
    }
  }
}
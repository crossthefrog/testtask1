using DefaultNamespace;
using UniRx;
using UnityEngine;

namespace Arch
{
  internal class HeroMoves : BaseMove
  {
    public struct Ctx
    {
      public MoveView moveView;
    }

    private readonly Ctx _ctx;

    public HeroMoves(Ctx ctx, BaseCtx baseCtx) : base(baseCtx)
    {
      _ctx = ctx;
      ReactiveProperty<bool> upPressed = new ReactiveProperty<bool>(false);
      ReactiveProperty<bool> downPressed = new ReactiveProperty<bool>(false);
      ReactiveProperty<bool> rightPressed = new ReactiveProperty<bool>(false);
      ReactiveProperty<bool> leftPressed = new ReactiveProperty<bool>(false);
      MoveView.Ctx viewCtx = new MoveView.Ctx
      {
        upPressed = upPressed,
        downPressed = downPressed,
        rightPressed = rightPressed,
        leftPressed = leftPressed,
      };
      _ctx.moveView.SetCtx(viewCtx);

      SubToDirection(upPressed, Vector3.up);
      SubToDirection(downPressed, Vector3.down);
      SubToDirection(rightPressed, Vector3.right);
      SubToDirection(leftPressed, Vector3.left);
    }

    private void SubToDirection(IReadOnlyReactiveProperty<bool> direction, Vector3 input)
    {
      direction.Subscribe(press =>
      {
        Vector3 realInput = press ? input : -input;
        moveDir.Value += realInput;
      }).AddTo(this);
    }
  }
}
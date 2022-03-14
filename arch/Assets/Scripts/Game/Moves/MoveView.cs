using UniRx;
using UnityEngine;

namespace Arch
{
  // тут же подключать геймпад или че угодно на экране
  internal class MoveView : BaseMoveView
  {
    public struct Ctx
    {
      public ReactiveProperty<bool> upPressed;
      public ReactiveProperty<bool> downPressed;
      public ReactiveProperty<bool> rightPressed;
      public ReactiveProperty<bool> leftPressed;
    }

    private Ctx _ctx;

    public void SetCtx(Ctx ctx)
    {
      _ctx = ctx;
    }
    
    private KeyCode UpKey
      => KeyCode.W;
    private KeyCode LeftKey
      => KeyCode.A;
    private KeyCode DownKey
      => KeyCode.S;
    private KeyCode RightKey
      => KeyCode.D;

    private void Update()
    {
      _ctx.upPressed.Value = Input.GetKey(UpKey);
      _ctx.downPressed.Value = Input.GetKey(DownKey);
      _ctx.rightPressed.Value = Input.GetKey(RightKey);
      _ctx.leftPressed.Value = Input.GetKey(LeftKey);
    }
  }
}
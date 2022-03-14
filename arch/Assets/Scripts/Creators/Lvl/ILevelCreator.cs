using System;

namespace Arch.Lvl
{
  internal interface ILevelCreator : IDisposable
  {
    void LoadLevel(int number, Action<LevelView> onComplete);
  }
}
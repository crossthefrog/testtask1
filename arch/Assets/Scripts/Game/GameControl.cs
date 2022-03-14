using Arch.CoreServices;
using Arch.Loaders;
using Arch.Lvl;
using DefaultNamespace;
using Game.UI;
using UniRx;

namespace Game
{
  internal class GameControl : BaseDisposable
  {
    public struct Ctx
    {
      public ILogs logs;
      public ILevelCreator levelCreator;
      public IContentLoader contentLoader;
      public IInstantiator instantiator;
      public ReactiveProperty<int> coins;
    }

    private readonly Ctx _ctx;
    private GameProcess _gameProcess;

    public GameControl(Ctx ctx)
    {
      _ctx = ctx;
      ReactiveProperty<int> choosenLevel = new ReactiveProperty<int>(0);
      UiCore.Ctx uiCtx = new UiCore.Ctx
      {
        logs = _ctx.logs,
        choosenLevel = choosenLevel,
        contentLoader = _ctx.contentLoader,
        instantiator = _ctx.instantiator,
        coins = _ctx.coins,
      };
      UiCore uiCore = new UiCore(uiCtx);
      uiCore.AddTo(this);
      
      choosenLevel.Subscribe(LevelChoosen).AddTo(this);
    }

    private void LevelChoosen(int num)
    {
      if (num == 0)
        return;
      _ctx.levelCreator.LoadLevel(num, levelView =>
      {
        _gameProcess?.Dispose();
        if (!levelView)
        {
          _ctx.logs.Log("game finished");
          return;
        }
        GameProcess.Ctx processCtx = new GameProcess.Ctx
        {
          contentLoader = _ctx.contentLoader,
          instantiator = _ctx.instantiator,
          heroSpawnPoint = levelView.HeroSpawn,
          enemiesSpawnPoint = levelView.EnemiesSpawns,
          coins = _ctx.coins,
        };
        _gameProcess = new GameProcess(processCtx);
        _gameProcess.AddTo(this);
      });
    }
  }
}
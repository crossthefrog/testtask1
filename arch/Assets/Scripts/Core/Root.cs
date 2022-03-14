using Arch.CoreServices;
using Arch.Env;
using Arch.Loaders;
using Arch.Lvl;
using DefaultNamespace;
using Game;
using UniRx;
using UnityEngine;

namespace Arch
{
  // на этом уровне создаются объекты для всей игры на всю продолжительность сессии
  internal class Root : BaseDisposable
  {
    public struct Ctx
    {

    }

    private readonly Ctx _ctx;

    public Root(Ctx ctx)
    {
      _ctx = ctx;
      GameObject root = new GameObject("Root");
      root.AddTo(this);
      // create core functions
      ILogs logs = new Logs();
      logs.AddTo(this);
      IContentLoader contentLoader = new ContentLoader();
      contentLoader.AddTo(this);
      Instantiator.Ctx instCtx = new Instantiator.Ctx
      {
        parent = root.transform,
      };
      IInstantiator instantiator = new Instantiator(instCtx);
      instantiator.AddTo(this);

      ReactiveProperty<int> loadings = new ReactiveProperty<int>();
      // create environment
      loadings.Value++;
      EnvCreator.Ctx envCtx = new EnvCreator.Ctx
      {
        onComplete = () => loadings.Value--,
        contentLoader = contentLoader,
        instantiator = instantiator,
      };
      EnvCreator envCreator = new EnvCreator(envCtx);
      envCreator.AddTo(this);

      // create level
      loadings.Value++;
      LevelCreator.Ctx lvlCtx = new LevelCreator.Ctx
      {
        onComplete = () => loadings.Value--,
        contentLoader = contentLoader,
        instantiator = instantiator,
      };
      ILevelCreator levelCreator = new LevelCreator(lvlCtx);
      levelCreator.AddTo(this);

      ReactiveProperty<int> coins = new ReactiveProperty<int>(0);
      loadings.First(count => count == 0).Subscribe(_ =>
      {
        GameControl.Ctx gameCtx = new GameControl.Ctx
        {
          logs = logs,
          levelCreator = levelCreator,
          instantiator = instantiator,
          contentLoader = contentLoader,
          coins = coins,
        };
        GameControl gameControl = new GameControl(gameCtx);
        gameControl.AddTo(this);
      }).AddTo(this);
    }
  }
}
using System;
using Arch.CoreServices;
using Arch.Loaders;
using DefaultNamespace;
using UnityEngine;

namespace Arch.Lvl
{
  // просто загрузка уровней
  internal class LevelCreator : BaseDisposable, ILevelCreator
  {
    public struct Ctx
    {
      public Action onComplete;
      public IContentLoader contentLoader;
      public IInstantiator instantiator;
    }

    private readonly Ctx _ctx;
    private GameObject _currentLvl;

    public LevelCreator(Ctx ctx)
    {
      _ctx = ctx;
      _ctx.onComplete?.Invoke();
    }

    private const string LEVEL_PATH = "level";

    // тут может быть и загрузка сцен и прочее
    public void LoadLevel(int number, Action<LevelView> onComplete)
    {
      if (_currentLvl)
        _ctx.instantiator.Destroy(_currentLvl);
      string lvlPath = $"{LEVEL_PATH}{number.ToString()}";
      _ctx.contentLoader.LoadPrefab<GameObject>(lvlPath, prefab =>
      {
        if (!prefab)
        {
          onComplete?.Invoke(null);
          return;
        }
        _currentLvl = _ctx.instantiator.CreateOnScene(prefab);
        _currentLvl.AddTo(this);
        onComplete?.Invoke(_currentLvl.GetComponent<LevelView>());
      });
    }
  }
}
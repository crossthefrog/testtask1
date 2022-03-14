using System;
using Arch.CoreServices;
using Arch.Loaders;
using DefaultNamespace;
using UnityEngine;

namespace Arch.Env
{
  // окружение - камеры свет итд итп
  internal class EnvCreator : BaseDisposable
  {
    public struct Ctx
    {
      public Action onComplete;
      public IContentLoader contentLoader;
      public IInstantiator instantiator;
    }

    private readonly Ctx _ctx;

    public EnvCreator(Ctx ctx)
    {
      _ctx = ctx;
      _ctx.contentLoader.LoadPrefab<GameObject>(ENV_PATH, EnvLoaded);
    }
    
    private const string ENV_PATH = "env";

    private void EnvLoaded(GameObject obj)
    {
      _ctx.instantiator.CreateOnScene(obj).AddTo(this);
      _ctx.onComplete?.Invoke();
    }
  }
}
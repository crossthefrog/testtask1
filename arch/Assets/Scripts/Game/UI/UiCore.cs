using Arch.CoreServices;
using Arch.Loaders;
using DefaultNamespace;
using UniRx;
using UnityEngine;

namespace Game.UI
{
  internal class UiCore : BaseDisposable
  {
    public struct Ctx
    {
      public ILogs logs;
      public IContentLoader contentLoader;
      public IInstantiator instantiator;
      public ReactiveProperty<int> choosenLevel;
      public IReadOnlyReactiveProperty<int> coins;
    }

    public enum UiState
    {
      MainMenu,
      Pause,
      Game
    }

    private readonly Ctx _ctx;
    private readonly ReactiveProperty<UiState> _state;
    private UiView _uiView;

    public UiCore(Ctx ctx)
    {
      _ctx = ctx;
      _state = new ReactiveProperty<UiState>(UiState.MainMenu);
      _ctx.choosenLevel.Subscribe((lvl =>
      {
        if (lvl == 0)
          _state.Value = UiState.MainMenu;
      })).AddTo(this);
      _state.Subscribe(StateChanged).AddTo(this);
      _ctx.contentLoader.LoadPrefab<GameObject>(CORE_UI_PATH, uiPrefab =>
      {
        GameObject uiOnScene = _ctx.instantiator.CreateOnScene(uiPrefab);
        uiOnScene.AddTo(this);
        _uiView = uiOnScene.GetComponent<UiView>();
        UiView.Ctx viewCtx = new UiView.Ctx
        {
          onNextLvlClicked = () =>
          {
            _ctx.choosenLevel.Value++;
            _state.Value = UiState.Game;
          },
          onPauseClicked = () => _state.Value = UiState.Pause,
          onResumeClicked = () => _state.Value = UiState.Game,
          state = _state,
          coins = _ctx.coins,
        };
        _uiView.SetCtx(viewCtx);
      });
    }

    private void StateChanged(UiState uiState)
    {
      _ctx.logs.Log($"ui state is {_state.Value.ToString()}");
      // самое простейшее
      Time.timeScale = uiState == UiState.Pause ? 0 : 1f;
    }

    private const string CORE_UI_PATH = "ui";
  }
}
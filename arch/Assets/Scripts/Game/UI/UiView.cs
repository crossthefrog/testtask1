using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
  internal class UiView : MonoBehaviour
  {
    public struct Ctx
    {
      public Action onNextLvlClicked;
      public Action onPauseClicked;
      public Action onResumeClicked;
      public IReadOnlyReactiveProperty<UiCore.UiState> state;
      public IReadOnlyReactiveProperty<int> coins;
    }

    [SerializeField] private Button nextLvl;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject game;
    [SerializeField] private Text coins;

    private Ctx _ctx;

    public void SetCtx(Ctx ctx)
    {
      _ctx = ctx;
      nextLvl.OnClickAsObservable().Subscribe(NexLvlClicked).AddTo(this);
      pauseBtn.OnClickAsObservable().Subscribe(PauseClicked).AddTo(this);
      resumeBtn.OnClickAsObservable().Subscribe(ResumeClicked).AddTo(this);
      _ctx.state.Subscribe(StateChanged).AddTo(this);
      _ctx.coins.Subscribe(CoinsChanged).AddTo(this);
    }

    private void StateChanged(UiCore.UiState state)
    {
      mainMenu.SetActive(state == UiCore.UiState.MainMenu);
      pause.SetActive(state == UiCore.UiState.Pause);
      game.SetActive(state == UiCore.UiState.Game);
    }

    private void NexLvlClicked(Unit _)
    {
      _ctx.onNextLvlClicked?.Invoke();
    }

    private void PauseClicked(Unit _)
    {
      _ctx.onPauseClicked?.Invoke();
    }

    private void ResumeClicked(Unit _)
    {
      _ctx.onResumeClicked?.Invoke();
    }

    private void CoinsChanged(int amount)
    {
      coins.text = $"${amount.ToString()}";
    }
  }
}
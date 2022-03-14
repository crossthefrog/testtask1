using Arch;
using Game.Healths;
using UnityEngine;

namespace Game
{
  internal class HeroView : MonoBehaviour
  {
    [SerializeField] private MoveView moveView;
    [SerializeField] private HealthView healthView;
    [SerializeField] private DamageView damageView;

    public MoveView MoveView
      => moveView;
    public HealthView HealthView
      => healthView;
    public DamageView DamageView
      => damageView;
  }
}
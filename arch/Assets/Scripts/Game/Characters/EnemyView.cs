using Arch;
using Game.Healths;
using UnityEngine;

namespace Game
{
  internal class EnemyView : MonoBehaviour
  {
    [SerializeField] private EnemyMoveView enemyMoveView;
    [SerializeField] private HealthView healthView;
    [SerializeField] private DamageView damageView;
    
    public EnemyMoveView EnemyMoveView
      => enemyMoveView;
    public HealthView HealthView
      => healthView;
    public DamageView DamageView
      => damageView;
  }
}
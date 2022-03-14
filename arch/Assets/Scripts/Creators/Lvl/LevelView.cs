using UnityEngine;

namespace Arch.Lvl
{
  internal class LevelView : MonoBehaviour
  {
    [SerializeField] private Transform heroSpawn;
    [SerializeField] private Transform[] enemiesSpawn;

    public Transform HeroSpawn
      => heroSpawn;
    public Transform[] EnemiesSpawns
      => enemiesSpawn;
  }
}
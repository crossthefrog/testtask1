using System.Collections.Generic;
using Arch.CoreServices;
using Arch.Loaders;
using DefaultNamespace;
using Game.Characters;
using UniRx;
using UnityEngine;

namespace Game
{
  internal class GameProcess : BaseDisposable
  {
    public struct Ctx
    {
      public IContentLoader contentLoader;
      public IInstantiator instantiator;
      public Transform heroSpawnPoint;
      public Transform[] enemiesSpawnPoint;
      public ReactiveProperty<int> coins;
    }

    private readonly Ctx _ctx;

    public GameProcess(Ctx ctx)
    {
      _ctx = ctx;
      // create bullets system
      ReactiveProperty<BulletInfo> bulletTrigger = new ReactiveProperty<BulletInfo>();
      BulletsManager.Ctx bulletsCtx = new BulletsManager.Ctx
      {
        instantiator = _ctx.instantiator,
        bulletTrigger = bulletTrigger,
      };
      BulletsManager bulletsManager = new BulletsManager(bulletsCtx);
      bulletsManager.AddTo(this);
      
      List<Transform> targetsForHero = new List<Transform>();
      List<Transform> targetsForEnemy = new List<Transform>();
      // create hero
      _ctx.contentLoader.LoadPrefab<GameObject>(HERO_PREFAB_PATH, heroPrefab =>
      {
        GameObject heroOnScene = _ctx.instantiator.CreateOnScene(heroPrefab);
        heroOnScene.transform.SetParent(_ctx.heroSpawnPoint);
        heroOnScene.transform.localPosition = Vector3.zero;
        Hero.Ctx heroCtx = new Hero.Ctx
        {
          heroView = heroOnScene.GetComponent<HeroView>(),
          moveSpeed = 1f,
          damage = 10f,
          fireRate = 1.5f,
          maxHp = 100f,
          potentialTargets = targetsForHero,
          bulletTrigger = bulletTrigger,
        };
        Hero hero = new Hero(heroCtx);
        heroOnScene.AddTo(hero);
        targetsForEnemy.Add(heroOnScene.transform);
        hero.AddTo(this);
      });
      
      // create enemies
      foreach (Transform transform in _ctx.enemiesSpawnPoint)
      {
        string name = Random.Range(0, 1f) < 0.5f ? ENEMY_PREFAB_PATH : ENEMY_FLY_PREFAB_PATH;
        _ctx.contentLoader.LoadPrefab<GameObject>(name, enemyPrefab =>
        {
          GameObject enemyOnScene = _ctx.instantiator.CreateOnScene(enemyPrefab);
          enemyOnScene.transform.SetParent(transform);
          enemyOnScene.transform.localPosition = Vector3.zero;
          Enemy.Ctx enemyCtx = new Enemy.Ctx
          {
            enemyView = enemyOnScene.GetComponent<EnemyView>(),
            moveSpeed = 0.5f,
            damage = 5,
            fireRate = 1.5f,
            maxHp = 50f,
            freezeTime = 1f,
            moveDistance = 1f,
            potentialTargets = targetsForEnemy,
            bulletTrigger = bulletTrigger,
            onDeath = () => _ctx.coins.Value += 5,
          };
          Enemy enemy = new Enemy(enemyCtx);
          enemyOnScene.AddTo(enemy);
          targetsForHero.Add(enemyOnScene.transform);
          enemy.AddTo(this);
        });
      }
    }

    private const string HERO_PREFAB_PATH = "hero";
    private const string ENEMY_PREFAB_PATH = "enemy";
    private const string ENEMY_FLY_PREFAB_PATH = "enemy_fly";
  }
}
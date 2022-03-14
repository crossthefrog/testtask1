using UnityEngine;

namespace Game
{
  internal class DamageView : MonoBehaviour
  {
    [SerializeField] private BulletView bulletPrefab;

    public Transform shotTransform;
    
    public BulletView BulletPrefab
      => bulletPrefab;
  }
}
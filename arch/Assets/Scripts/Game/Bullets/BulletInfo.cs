using UnityEngine;

namespace Game
{
  internal struct BulletInfo
  {
    public Vector3 origin;
    public Vector3 direction;
    public float damage;
    public BulletView bulletPrefab;
  }
}
using UnityEngine;

namespace Game.Healths
{
  internal class HealthView : MonoBehaviour
  {
    [SerializeField] private Collider detector;
    [SerializeField] private float damageOnTouch;

    public Collider Detector
      => detector;

    public float DamageOnTouch
      => damageOnTouch;
  }
}
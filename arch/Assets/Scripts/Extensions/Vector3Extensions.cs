using UnityEngine;

namespace DefaultNamespace
{
  internal static class Vector3Extensions
  {
    public static Vector3 Projection(this Vector3 direction)
    {
      Vector3 proj = Vector3.zero;
      proj.z = direction.y;
      proj.x = direction.x;
      return proj;
    }
  }
}
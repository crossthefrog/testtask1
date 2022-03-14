using UnityEngine;

namespace Arch.Env
{
  internal class EnvView : MonoBehaviour
  {
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float sceneWidth = 10f;

    private void Awake()
    {
      float unitsPerPixel = sceneWidth / Screen.width;
      float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
      mainCamera.orthographicSize = desiredHalfHeight;
    }
  }
}
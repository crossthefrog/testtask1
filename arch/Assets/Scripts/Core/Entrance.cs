using UnityEngine;

namespace Arch
{
  public class Entrance : MonoBehaviour
  {
    private static Root _root;

    private void Awake()
    {
      if (_root == null)
      {
        _root = CreateRoot();
      }
    }

    private Root CreateRoot()
    {
      Root.Ctx rootCtx = new Root.Ctx
      {
        // todo if need
      };
      return new Root(rootCtx);
    }
  }
}
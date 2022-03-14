using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
  internal class BaseDisposable : IDisposable
  {
    private List<IDisposable> _disposables;
    private List<Object> _objects;

    public void AddDisposable(IDisposable disposable)
    {
      if (_disposables == null)
        _disposables = new List<IDisposable>();
      _disposables.Add(disposable);
    }
    
    public void AddDisposable<T>(T obj) where T : Object
    {
      if (_objects == null)
        _objects = new List<Object>();
      _objects.Add(obj);
    }

    public void Dispose()
    {
      if (_disposables != null)
      {
        for (int i = _disposables.Count - 1; i >= 0; i--)
        {
          IDisposable disposable = _disposables[i];
          disposable?.Dispose();
        }
      }
      if (_objects != null)
      {
        for (int i = _objects.Count - 1; i >= 0; i--)
        {
          Object obj = _objects[i];
          if (obj)
            Object.Destroy(obj);
        }
      }
    }
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public IObjectPool<ObjectPoolManager> _ManagedPool;

    public void SetManagedPool(IObjectPool<ObjectPoolManager> Pool)
    {
        _ManagedPool = Pool;
    }

    public void DestroyObject()
    {
        _ManagedPool.Release(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using PatternManager_2_2;

public class ObjectPoolManager : MonoBehaviour
{
    public IObjectPool<ObjectPoolManager> _ManagedPool;

    public void SetManagedPool(IObjectPool<ObjectPoolManager> Pool)
    {
        _ManagedPool = Pool;
    }

    public void ReleaseObject()
    {
        _ManagedPool.Release(this);
    }
}

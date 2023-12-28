using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using PatternManager_2_2;

public class ObjectPoolManager : MonoBehaviour
{
    public IObjectPool<GameObject> _ManagedPool;

    //관리되는 풀 설정
    public void SetManagedPool(IObjectPool<GameObject> Pool)
    {
        _ManagedPool = Pool;
    }

    //풀에 오브젝트 반납
    public void ReleaseObject()
    {
        _ManagedPool.Release(gameObject);
    }
}

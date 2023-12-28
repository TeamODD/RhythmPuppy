using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using PatternManager_2_2;

public class ObjectPoolManager : MonoBehaviour
{
    public IObjectPool<GameObject> _ManagedPool;

    //�����Ǵ� Ǯ ����
    public void SetManagedPool(IObjectPool<GameObject> Pool)
    {
        _ManagedPool = Pool;
    }

    //Ǯ�� ������Ʈ �ݳ�
    public void ReleaseObject()
    {
        _ManagedPool.Release(gameObject);
    }
}

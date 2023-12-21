using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Obstacles;

public class PatternManager_2_2 : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        //오브젝트 이름
        public string objectName;
        //오브젝트 풀에서 관리할 오브젝트
        public GameObject prefab;
        //생성할 갯수
        public int count;
    }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip audioClip;

    private ObjectPoolManager PoolingManager;

    private IObjectPool<ObjectPoolManager> _Pool;

    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<string, IObjectPool<ObjectPoolManager>> objectPoolDic = new Dictionary<string, IObjectPool<ObjectPoolManager>>();

    private int createIndex;
    public bool IsReady { get; set; }
    [SerializeField]
    private float[] Pattern1_aTimeLine_1;
    //[SerializeField]
    private float[] Pattern1_aTimeLine_2;

    void Awake()
    {
        PoolingManager = FindObjectOfType<ObjectPoolManager>();
        audioSource.clip = audioClip;
        //_Pool = new ObjectPool<ObjectPoolManager>(CreatePrefab, OnGetPrefab, OnReleasePrefab, OnDestroyPrefab, maxSize: 10);
        init();
        PatternMake();
    }

    private void init()
    {
        IsReady = false;

        for (int i = 0; i < objectInfos.Length; i++)
        {
            createIndex = i;
            _Pool = new ObjectPool<ObjectPoolManager>(CreatePrefab, OnGetPrefab, OnReleasePrefab, OnDestroyPrefab,
                maxSize: objectInfos[createIndex].count);
            objectPoolDic.Add(objectInfos[createIndex].objectName, _Pool);
        }
    }

    private void PatternMake()
    {
        float startTime = audioSource.time;

        for (int i = 0; i < Pattern1_aTimeLine_1.Length; i++)
        {
            StartCoroutine(Pattern1_a(Pattern1_aTimeLine_1[i], startTime));
        }

    }

    IEnumerator Pattern1_a(float t, float startTime)
    {
        string objectName = objectInfos[0].objectName;

        if (0 <= t - startTime)
        {
            yield return new WaitForSeconds(t - startTime);
            //var cat = objectPoolDic[objectName].Get();
        }
       // yield return PoolingManager;
       yield return objectPoolDic[objectName].Get();
    }

    private ObjectPoolManager CreatePrefab()
    {
        //if (Prefab.GetComponent<ObjectPoolManager>() == null) return;
        ObjectPoolManager PoolingObject = Instantiate(objectInfos[createIndex].prefab).GetComponent<ObjectPoolManager>();
        PoolingObject.SetManagedPool(_Pool);
        return PoolingObject;
    }
    //오브젝트 빌려올 때 사용
    private void OnGetPrefab(ObjectPoolManager Manager)
    {
        Manager.gameObject.SetActive(true);
    }

    private void OnReleasePrefab(ObjectPoolManager Manager)
    {
        Manager.gameObject.SetActive(false);
    }

    private void OnDestroyPrefab(ObjectPoolManager Manager)
    {
        Destroy(Manager.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Obstacles;
using EventManagement;

public class PatternManager_2_2 : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        //������Ʈ �̸�
        public string objectName;
        //������Ʈ Ǯ���� ������ ������Ʈ
        public GameObject prefab;
        //������ ����
        public int count;
    }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip audioClip;
    [SerializeField]
    private GameObject MusicManager;

    private ObjectPoolManager PoolingManager;

    private IObjectPool<ObjectPoolManager> _Pool;

    // ������ƮǮ���� ������ ��ųʸ�
    private Dictionary<string, IObjectPool<ObjectPoolManager>> objectPoolDic = new Dictionary<string, IObjectPool<ObjectPoolManager>>();

    private int PrefabIndex;
    public bool IsReady { get; set; }
    private float WaitingTimeBeforeGameStart;
    private float startTime = 3f;
    //���� Ÿ�Ӷ��ο� ���ؼ��� �̸��� �ٲ۴ٰų�, �� �ܿ� ���� �ǵ��� ���ÿ�.
    [SerializeField]
    private float[] Pattern1_aTimeLine_1;
    [SerializeField]
    private float[] Pattern1_aTimeLine_2;
    private float[] DelayedPattern1_a_1;
    private float[] DelayedPattern1_a_2;


    EventManager eventManager;


    void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        PoolingManager = FindObjectOfType<ObjectPoolManager>();
        //_Pool = new ObjectPool<ObjectPoolManager>(CreatePrefab, OnGetPrefab, OnReleasePrefab, OnDestroyPrefab, maxSize: 10);
        init();
    }

    void Update()
    {
        WaitingTimeBeforeGameStart += Time.fixedDeltaTime;
    }

    private void init()
    {
        IsReady = false;

        WaitingTimeBeforeGameStart = 0f;
        audioSource.clip = audioClip;

        DelayedPattern1_a_1 = new float[Pattern1_aTimeLine_1.Length];
        DelayedPattern1_a_2 = new float[Pattern1_aTimeLine_2.Length];

        for (int i = 0; i < objectInfos.Length; i++)
        {
            PrefabIndex = i;
            _Pool = new ObjectPool<ObjectPoolManager>(CreatePrefab, OnGetPrefab, OnReleasePrefab, OnDestroyPrefab,
                maxSize: objectInfos[PrefabIndex].count);
            objectPoolDic.Add(objectInfos[PrefabIndex].objectName, _Pool);
        }
        /*for (int j = 0; j < objectInfos[PrefabIndex].count; j++)
        {
            ObjectPoolManager PreMakingObject = CreatePrefab();
            PreMakingObject.ReleaseObject();
        }*/
        DelayingPattern();
        PatternMake();

        IsReady = true;
        Debug.Log("Ready");
        while (WaitingTimeBeforeGameStart < startTime)
        {
            return;
        }
        //���� ���
        audioSource.Play();
    }

    private void DelayingPattern()
    {
        float delaytime = startTime;
        for (int i = 0; i < Pattern1_aTimeLine_1.Length; i++)
            DelayedPattern1_a_1[i] = Pattern1_aTimeLine_1[i] + delaytime;
        for (int i = 0; i < Pattern1_aTimeLine_2.Length; i++)
            DelayedPattern1_a_2[i] = Pattern1_aTimeLine_2[i] + delaytime;
    }

    private void PatternMake()
    {
        float startTime = audioSource.time;

        for (int i = 0; i < DelayedPattern1_a_1.Length; i++)
        {
            StartCoroutine(Pattern1_a(DelayedPattern1_a_1[i], startTime));
        }
        for (int i = 0; i < DelayedPattern1_a_2.Length; i++)
            StartCoroutine(Pattern1_a(DelayedPattern1_a_2[i], startTime));

    }

    IEnumerator Pattern1_a(float t, float startTime)
    {
        string objectName = objectInfos[0].objectName;
        ObjectPoolManager cat;
        float randomX;
        void Warning1_a(float x)
        {
            Vector2 v = Camera.main.WorldToScreenPoint(new Vector2(x, 0));
            eventManager.stageEvent.warnWithBox(v, new Vector3(300, 1080, 0));
        }

        if (0 <= t - startTime)
        {
            //"t-startTime > 0 && t - (startTime + 1) < 0" �� ��� ���� ��� �� ��.
            yield return new WaitForSeconds(t - (startTime + 1f));
            //���� ĳ��
            randomX = Random.Range(-8f, 8f);
            //��� �ڽ� ����
            Warning1_a(randomX);

            yield return new WaitForSeconds(1f);
            //����� ����
            cat = objectPoolDic[objectName].Get();
            cat.gameObject.transform.position = new Vector3(randomX, -2, 0); //��ġ �̵�
            cat.gameObject.GetComponent<Cat_1>().Jump(); //������ �ϵ���
        }

        yield return PoolingManager;
    }



    private ObjectPoolManager CreatePrefab()
    {
        //if (Prefab.GetComponent<ObjectPoolManager>() == null) return;
        ObjectPoolManager PoolingObject = Instantiate(objectInfos[PrefabIndex].prefab).GetComponent<ObjectPoolManager>();
        PoolingObject.SetManagedPool(_Pool);
        Debug.Log("CreatePrefab");
        return PoolingObject;
    }
    //������Ʈ ������ �� ���
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

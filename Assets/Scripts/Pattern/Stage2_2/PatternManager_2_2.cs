using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Obstacles;
using EventManagement;

namespace PatternManager_2_2
{
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

        public AudioSource audioSource;
        [SerializeField]
        AudioClip audioClip;
        [SerializeField]
        private GameObject MusicManager;
        EventManager eventManager;
        //private ObjectPoolManager PoolingManager;
        private IObjectPool<GameObject> _Pool;

        // ������ƮǮ�� ������ ��ųʸ�
        private Dictionary<string, IObjectPool<GameObject>> ObjectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();
        // �������� ������ ��ųʸ�
        private Dictionary<string, GameObject> GameObjectDic = new Dictionary<string, GameObject>();

        private int PrefabIndex;
        private string objectName;
        public bool IsReady { get; set; }
        private float startTime = 3f;

        //���� Ÿ�Ӷ��ο� ���ؼ��� �̸��� �ٲ۴ٰų��ϴ� ��, ���� �ǵ��� ���ÿ�.
        [SerializeField]
        private float[] Pattern1_aTimeLine_1;
        [SerializeField]
        private float[] Pattern1_aTimeLine_2;
        private float[] DelayedPattern1_a_1;
        private float[] DelayedPattern1_a_2;


        void Awake()
        {
            eventManager = FindObjectOfType<EventManager>();
            //PoolingManager = FindObjectOfType<ObjectPoolManager>();
            init();
        }

        private void init()
        {
            IsReady = false;

            audioSource.clip = audioClip;

            DelayedPattern1_a_1 = new float[Pattern1_aTimeLine_1.Length];
            DelayedPattern1_a_2 = new float[Pattern1_aTimeLine_2.Length];

            for (int i = 0; i < objectInfos.Length; i++)
            {
                PrefabIndex = i;
                objectName = objectInfos[PrefabIndex].objectName;
                _Pool = new ObjectPool<GameObject>(CreatePrefab, OnGetPrefab, OnReleasePrefab, OnDestroyPrefab,
                    maxSize: objectInfos[PrefabIndex].count);
                ObjectPoolDic.Add(objectName, _Pool);
                GameObjectDic.Add(objectName, objectInfos[PrefabIndex].prefab);

                /* �̸� �����յ��� Ǯ���س��� �ڵ�, �ٸ� ���� �߻����� ���� �ּ� ó�� �س�����.
                 * GameObject PoolingTest;
                for (int j = 0; j < objectInfos[PrefabIndex].count; j++)
                {
                    PoolingTest = CreatePrefab();
                    PoolingTest.GetComponent<ObjectPoolManager>().ReleaseObject();
                }*/
            }

            DelayingPattern();
            PatternMake();

            IsReady = true;
            Debug.Log("Stage is Ready");
            //���� ���
            StartCoroutine(PlayMusic());
            Debug.Log("Stage Start");

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

            float randomX;

            void MakingWarningBox(float x)
            {
                Vector2 v = Camera.main.WorldToScreenPoint(new Vector2(x, 0));
                eventManager.stageEvent.warnWithBox(v, new Vector3(300, 1080, 0));
            }

            void MakingCat(string ObjectName, float X)
            {
                GameObject cat;

                cat = ObjectPoolDic[ObjectName].Get();  

                cat.gameObject.transform.position = new Vector3(X, -2, 0); //��ġ �̵�
                cat.gameObject.GetComponent<Cat_1>().Jump(); //������ �ϵ���
                cat.gameObject.GetComponent<Cat_1>().IsPooled = true;

                //return PoolingManager;
            }

            if (0 <= t - startTime)
            {
                //"t-startTime > 0 && t - (startTime + 1) < 0" �� ��� ���� ��� �� ��.
                yield return new WaitForSeconds(t - (startTime + 1f));
                //���� ĳ��
                randomX = Random.Range(-8f, 8f);
                //��� �ڽ� ����
                MakingWarningBox(randomX);
                yield return new WaitForSeconds(1f);

                //����� ����
                SetPrefabIndex(0);
                MakingCat(objectName, randomX);
            }
            yield break;
        }

        IEnumerator PlayMusic()
        {
            yield return new WaitForSeconds(startTime);
            audioSource.Play();
        }

        private void SetPrefabIndex(int prefabIndex)
        {
            PrefabIndex = prefabIndex;
        }


        //Ǯ�� �Լ����Դϴ�.
        private GameObject CreatePrefab()
        {
            /*if (_Pool.CountInactive != 0)
            {
                OnGetPrefab(PoolingManager);
                return PoolingManager;
            } 
            ObjectPoolManager PoolingObject = Instantiate(objectInfos[PrefabIndex].prefab).GetComponent<ObjectPoolManager>();
            PoolingObject.SetManagedPool(_Pool);
            return PoolingObject; */
            GameObject PoolingObject = Instantiate(objectInfos[PrefabIndex].prefab);
            PoolingObject.GetComponent<ObjectPoolManager>().SetManagedPool(ObjectPoolDic[objectName]);
            return PoolingObject;
        }
        //������Ʈ ������ �� ���
        private void OnGetPrefab(GameObject Prefab)
        {
            Prefab.SetActive(true);
        }

        private void OnReleasePrefab(GameObject Prefab)
        {
            Prefab.SetActive(false);
        }

        private void OnDestroyPrefab(GameObject Prefab)
        {
            Destroy(Prefab);
        }
    }
}

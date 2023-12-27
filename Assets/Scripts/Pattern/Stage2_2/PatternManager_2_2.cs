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

        private float randomY7_a;
        //���� Ÿ�Ӷ��ο� ���ؼ��� �̸��� �ٲ۴ٰų��ϴ� ��, ���� �ǵ��� ���ÿ�.
        [SerializeField]
        private float[] Pattern1_aTimeLine_1;
        [SerializeField]
        private float[] Pattern1_aTimeLine_2;
        private float[] DelayedPattern1_a_1;
        private float[] DelayedPattern1_a_2;
        [SerializeField]
        private float[] Pattern6TimeLine;
        private float[] DelayedPattern6;
        [SerializeField]
        private float[] Pattern7_aTimeLine;
        private float[] DelayedPattern7_a;


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
            DelayedPattern6 = new float[Pattern6TimeLine.Length];
            DelayedPattern7_a = new float[Pattern7_aTimeLine.Length];

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
            for (int i = 0; i < Pattern6TimeLine.Length; i++)
                DelayedPattern6[i] = Pattern6TimeLine[i] + delaytime;
            for (int i = 0; i < Pattern7_aTimeLine.Length; i++)
                DelayedPattern7_a[i] = Pattern7_aTimeLine[i] + delaytime;
        }

        private void PatternMake()
        {
            float startTime = audioSource.time;

            for (int i = 0; i < DelayedPattern1_a_1.Length; i++)
                StartCoroutine(pattern1_a(DelayedPattern1_a_1[i], startTime));
            for (int i = 0; i < DelayedPattern1_a_2.Length; i++)
                StartCoroutine(pattern1_a(DelayedPattern1_a_2[i], startTime));
            for (int i = 0; i < DelayedPattern6.Length; i++)
                StartCoroutine(Pattern6(DelayedPattern6[i], startTime));
            for (int i = 0; i < DelayedPattern7_a.Length; i++)
            {
                randomY7_a = Random.Range(-2f, 4.5f);
                StartCoroutine(Pattern7_a(DelayedPattern7_a[i], startTime, randomY7_a));
                StartCoroutine(Pattern7_a_WarningBox(DelayedPattern7_a[i], startTime, randomY7_a));
            }

        }

        IEnumerator pattern1_a(float t, float startTime)
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
                SetPrefabInfos(0);
                MakingCat(objectName, randomX);
            }
            yield break;
        }

        IEnumerator Pattern6(float t, float startTime)
        {
            string objectName = objectInfos[1].objectName;

            float randomX;

            void MakingCat(string ObjectName, float X)
            {
                GameObject cat2;

                cat2 = ObjectPoolDic[ObjectName].Get();

                cat2.gameObject.transform.position = new Vector3(X, 5, 0); //��ġ �̵�
                cat2.gameObject.GetComponent<Cat_2>().Setting();
                cat2.gameObject.GetComponent<Cat_2>().IsPooled = true;

                //return PoolingManager;
            }

            if (0 <= t - startTime)
            {
                //"t-startTime > 0 && t - (startTime + 1) < 0" �� ��� ���� ��� �� ��.
                yield return new WaitForSeconds(t - (startTime + 1f));
                //���� ĳ��
                randomX = Random.Range(-8f, 8f);
                //��� ǥ�� ����
                eventManager.playerEvent.markActivationEvent();
                yield return new WaitForSeconds(1f);
                eventManager.playerEvent.markInactivationEvent();

                //����� ����
                SetPrefabInfos(1);
                MakingCat(objectName, randomX);
            }
            yield break;
        }

        IEnumerator Pattern7_a(float t, float startTime, float RandomY)
        {
            string objectName = objectInfos[2].objectName;

            float randomY = RandomY;

            void MakingCat(string ObjectName, float Y)
            {
                GameObject cat3;
                cat3 = ObjectPoolDic[ObjectName].Get();
                cat3.gameObject.GetComponent<Pattern1_a>().time = 0;
                cat3.gameObject.transform.position = new Vector3(10.5f, Y, 0); //��ġ �̵�
                cat3.gameObject.GetComponent<Pattern1_a>().IsPooled = true;

            }

            if (0 <= t - startTime)
            {
                //"t-startTime > 0 && t - (startTime + 1) < 0" �� ��� ���� ��� �� ��.
                yield return new WaitForSeconds(t - (startTime + 1));

                //����� ����
                SetPrefabInfos(2);
                MakingCat(objectName, randomY);
            }
            yield break;
        }

        IEnumerator Pattern7_a_WarningBox(float t, float startTime, float RandomY)
        {
            string objectName = objectInfos[3].objectName;

            float randomY = RandomY;

            void MakingWarningBox(string WarningBox, float Y)
            {
                GameObject cat3_WarningBox;
                cat3_WarningBox = ObjectPoolDic[objectName].Get();
                cat3_WarningBox.gameObject.GetComponent<Warning1_a>().time = 0f;
                cat3_WarningBox.gameObject.transform.position = new Vector3(9f, Y, 0);
                cat3_WarningBox.gameObject.GetComponent<Warning1_a>().IsPooled = true;

                //return PoolingManager;
            }

            if (0 <= t - startTime)
            {
                //"t-startTime > 0 && t - (startTime + 1) < 0" �� ��� ���� ��� �� ��.
                yield return new WaitForSeconds(t - (startTime + 1));

                SetPrefabInfos(3);
                MakingWarningBox(objectName, randomY);
            }
            yield break;
        }

        IEnumerator PlayMusic()
        {
            yield return new WaitForSeconds(startTime);
            audioSource.Play();
        }

        private void SetPrefabInfos(int prefabIndex)
        {
            PrefabIndex = prefabIndex;
            objectName = objectInfos[prefabIndex].objectName;
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

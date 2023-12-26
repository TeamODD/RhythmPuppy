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
            //오브젝트 이름
            public string objectName;
            //오브젝트 풀에서 관리할 오브젝트
            public GameObject prefab;
            //생성할 갯수
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

        // 오브젝트풀을 관리할 딕셔너리
        private Dictionary<string, IObjectPool<GameObject>> ObjectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();
        // 프리펩을 관리할 딕셔너리
        private Dictionary<string, GameObject> GameObjectDic = new Dictionary<string, GameObject>();

        private int PrefabIndex;
        private string objectName;
        public bool IsReady { get; set; }
        private float startTime = 3f;

        //패턴 타임라인에 대해서는 이름을 바꾼다거나하는 등, 절대 건들지 마시오.
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

                /* 미리 프리팹들을 풀링해놓는 코드, 다만 오류 발생으로 인해 주석 처리 해놓았음.
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
            //음악 재생
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

                cat.gameObject.transform.position = new Vector3(X, -2, 0); //위치 이동
                cat.gameObject.GetComponent<Cat_1>().Jump(); //점프를 하도록
                cat.gameObject.GetComponent<Cat_1>().IsPooled = true;

                //return PoolingManager;
            }

            if (0 <= t - startTime)
            {
                //"t-startTime > 0 && t - (startTime + 1) < 0" 인 경우 아직 고려 안 함.
                yield return new WaitForSeconds(t - (startTime + 1f));
                //변수 캐싱
                randomX = Random.Range(-8f, 8f);
                //경고 박스 생성
                MakingWarningBox(randomX);
                yield return new WaitForSeconds(1f);

                //고양이 생성
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


        //풀링 함수들입니다.
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
        //오브젝트 빌려올 때 사용
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

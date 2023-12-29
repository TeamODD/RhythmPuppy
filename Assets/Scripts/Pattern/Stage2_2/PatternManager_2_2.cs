using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using Obstacles;
using EventManagement;


namespace Stage_2
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
        [SerializeField]
        private Image BlackScreen; //패턴3_b 
        [SerializeField]
        private GameObject ArtifactManager;
        [SerializeField]
        private GameObject Pattern8_Canvas;
        [SerializeField]
        private GameObject OverlayCanvas;

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
        [SerializeField]
        private float[] Pattern6TimeLine;
        private float[] DelayedPattern6;
        [SerializeField]
        private float[] Pattern7_aTimeLine;
        private float[] DelayedPattern7_a;
        [SerializeField]
        private float[] Pattern8TimeLine;
        private float[] DelayedPattern8;
        [SerializeField]
        private float[] Pattern9TimeLine;
        private float[] DelayedPattern9;

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
            DelayedPattern8 = new float[Pattern8TimeLine.Length];
            DelayedPattern9 = new float[Pattern9TimeLine.Length];

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
            for (int i = 0; i < Pattern6TimeLine.Length; i++)
                DelayedPattern6[i] = Pattern6TimeLine[i] + delaytime;
            for (int i = 0; i < Pattern7_aTimeLine.Length; i++)
                DelayedPattern7_a[i] = Pattern7_aTimeLine[i] + delaytime;
            for (int i = 0; i < Pattern8TimeLine.Length; i++)
                DelayedPattern8[i] = Pattern8TimeLine[i] + delaytime;
            for (int i = 0; i < Pattern9TimeLine.Length; i++)
                DelayedPattern9[i] = Pattern9TimeLine[i] + delaytime;
        }

        private void PatternMake()
        {
            float startTime = audioSource.time;
            float randomY = 0;
            float randomX = 0;

            for (int i = 0; i < DelayedPattern1_a_1.Length; i++)
                StartCoroutine(pattern1_a(DelayedPattern1_a_1[i], startTime));
            for (int i = 0; i < DelayedPattern1_a_2.Length; i++)
                StartCoroutine(pattern1_a(DelayedPattern1_a_2[i], startTime));
            for (int i = 0; i < DelayedPattern6.Length; i++)
                StartCoroutine(Pattern6(DelayedPattern6[i], startTime));
            for (int i = 0; i < DelayedPattern7_a.Length; i++)
            {
                randomY = Random.Range(-3.5f, 0f);
                StartCoroutine(Pattern7_a(DelayedPattern7_a[i], startTime, randomY));
                StartCoroutine(Pattern7_a_WarningBox(DelayedPattern7_a[i], startTime, randomY));
            }
            StartCoroutine(Pattern3_b(39.4f + 3f, startTime)); //3f = (Global) startTime
            for (int i = 0; i < DelayedPattern8.Length; i++)
                StartCoroutine(Pattern_8(DelayedPattern8[i], startTime));
            for (int i = 0; i < DelayedPattern9.Length; i++)
            {
                randomX = Random.Range(-8.7f, 8.7f);
                StartCoroutine(Pattern_9(DelayedPattern9[i], startTime, randomX));
                StartCoroutine(Pattern_9_WarningBox(DelayedPattern9[i], startTime, randomX));
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

                cat2.gameObject.transform.position = new Vector3(X, 5, 0); //위치 이동
                cat2.gameObject.GetComponent<Cat_2>().Setting();
                cat2.gameObject.GetComponent<Cat_2>().IsPooled = true;

                //return PoolingManager;
            }

            if (0 <= t - startTime)
            {
                //"t-startTime > 0 && t - (startTime + 1) < 0" 인 경우 아직 고려 안 함.
                yield return new WaitForSeconds(t - (startTime + 1f));
                //변수 캐싱
                randomX = Random.Range(-8f, 8f);
                //경고 표식 생성
                eventManager.playerEvent.markActivationEvent();
                yield return new WaitForSeconds(1f);
                eventManager.playerEvent.markInactivationEvent();

                //고양이 생성
                SetPrefabInfos(1);
                MakingCat(objectName, randomX);
            }
            yield break;
        }

        IEnumerator Pattern7_a(float t, float startTime, float RandomY)
        {
            string objectName = objectInfos[2].objectName;

            float randomY = RandomY;

            void MakingCat(float Y)
            {
                GameObject cat3;
                cat3 = ObjectPoolDic[objectName].Get();
                cat3.gameObject.GetComponent<Pattern1_a>().time = 0;
                cat3.gameObject.transform.position = new Vector3(10.5f, Y, 0); //위치 이동
                cat3.gameObject.GetComponent<Pattern1_a>().IsPooled = true;

            }

            if (0 <= t - startTime)
            {
                //"t-startTime > 0 && t - (startTime + 1) < 0" 인 경우 아직 고려 안 함.
                yield return new WaitForSeconds(t - (startTime + 1));

                //고양이 생성
                SetPrefabInfos(2);
                MakingCat(randomY);
            }
            yield break;
        }

        IEnumerator Pattern7_a_WarningBox(float t, float startTime, float RandomY)
        {
            string objectName = objectInfos[3].objectName;

            float randomY = RandomY;

            void MakingWarningBox(float Y)
            {
                GameObject cat3_WarningBox;
                cat3_WarningBox = ObjectPoolDic[objectName].Get();
                cat3_WarningBox.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                cat3_WarningBox.gameObject.transform.SetParent(OverlayCanvas.transform, true);
                Vector3 v = Camera.main.WorldToScreenPoint(new Vector3(9, Y, 0));
                cat3_WarningBox.gameObject.transform.position = v;
                //cat3_WarningBox.gameObject.transform.parent = OverlayCanvas.transform;
                cat3_WarningBox.gameObject.GetComponent<Warning1_a>().time = 0f;
                cat3_WarningBox.gameObject.GetComponent<Warning1_a>().IsPooled = true;

                //return PoolingManager;
            }

            if (0 <= t - startTime)
            {
                //"t-startTime > 0 && t - (startTime + 1) < 0" 인 경우 아직 고려 안 함.
                yield return new WaitForSeconds(t - (startTime + 1));

                SetPrefabInfos(3);
                MakingWarningBox(randomY);
            }
            yield break;
        }

        IEnumerator Pattern3_b(float t, float startTime)
        {
            void LampControll(bool s)
            {
                ArtifactManager.transform.GetChild(0).gameObject.GetComponent<LampAction>().LampControl(s);
                ArtifactManager.transform.GetChild(1).gameObject.GetComponent<LampAction>().LampControl(s);
            }

            if (0 <= t - startTime)
            {
                yield return new WaitForSeconds(t - startTime);

                float SpriteAlpha = 0;
                float Faze1Time = 0;
                float Faze2Time = 11.3f;

                while (Faze1Time < 11.5f)
                {
                    float CalFactor = 1f / 11.5f;
                    BlackScreen.color = new Color(0, 0, 0, SpriteAlpha);
                    Faze1Time += Time.fixedDeltaTime; //실제로 11.5초 걸리는지 확인 해봐야함
                    SpriteAlpha = Faze1Time * CalFactor;
                    yield return new WaitForFixedUpdate();
                }
                SpriteAlpha = 1f;
                BlackScreen.color = new Color(0, 0, 0, SpriteAlpha);
                LampControll(false);
                yield return new WaitForSeconds(5.5f);
                SpriteAlpha = 30f / 31f;
                BlackScreen.color = new Color(0, 0, 0, SpriteAlpha);
                yield return new WaitForSeconds(22.6f);

                LampControll(true);
                while (Faze2Time > 0)
                {
                    float CalFactor = 30f / 350.3f; // (1 / 11.3) * (30 / 31) 알파가 250이어야함.
                    BlackScreen.color = new Color(0, 0, 0, SpriteAlpha);
                    Faze2Time -= Time.fixedDeltaTime;
                    SpriteAlpha = Faze2Time * CalFactor;
                    yield return new WaitForFixedUpdate();
                }
                BlackScreen.color = new Color(0, 0, 0, 0);
            }

            if(0 >= t - startTime && t - startTime - 50.9f <= 0)
            {
                //패턴 진행중일시

            }
            yield break;
        }

        IEnumerator Pattern_8(float t, float startTime)
        {
            string objectName = objectInfos[4].objectName;

            float randomX;
            float randomY;

            void MakingObject()
            {
                GameObject firefly;
                firefly = ObjectPoolDic[objectName].Get();
                //firefly.gameObject.transform.parent = Pattern8_Canvas.transform;
                Vector3 v = Camera.main.WorldToScreenPoint(new Vector3(randomX, randomY, 0));
                firefly.gameObject.transform.SetParent(Pattern8_Canvas.transform, true);
                firefly.gameObject.transform.position = v;
                firefly.gameObject.GetComponent<Pattern8>().IsPooled = true;
                firefly.gameObject.GetComponent<Pattern8>().time = 0;

                //return PoolingManager;
            }

            if (0 <= t - startTime)
            {
                //"t-startTime > 0 && t - (startTime + 1) < 0" 인 경우 아직 고려 안 함.
                yield return new WaitForSeconds(t - startTime);

                randomX = Random.Range(-7f, 7);
                randomY = Random.Range(-2f, 2f);

                SetPrefabInfos(4);
                MakingObject();
            }
            yield break;
        }

        IEnumerator Pattern_9(float t, float startTime, float RandomX)
        {
            string objectName = objectInfos[5].objectName;

            float randomX = RandomX;

            void MakingObject()
            {
                GameObject Cat9;
                Cat9 = ObjectPoolDic[objectName].Get();
                Cat9.gameObject.transform.position = new Vector3(randomX, 6.5f, 0);
                Cat9.gameObject.GetComponent<Pattern9>().IsPooled = true;
                Cat9.gameObject.GetComponent<Pattern9>().velocity.y = 0;

                //return PoolingManager;
            }

            if (0 <= t - startTime)
            {
                //"t-startTime > 0 && t - (startTime + 1) < 0" 인 경우 아직 고려 안 함.
                yield return new WaitForSeconds(t - startTime);

                SetPrefabInfos(5);
                MakingObject();
            }
            yield break;
        }

        IEnumerator Pattern_9_WarningBox(float t, float startTime, float RandomX)
        {
            string objectName = objectInfos[3].objectName;

            float randomX = RandomX;

            void MakingObject()
            {
                GameObject WarningBox;
                WarningBox = ObjectPoolDic[objectName].Get();
                WarningBox.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                WarningBox.gameObject.transform.SetParent(OverlayCanvas.transform, true);
                Vector3 v = Camera.main.WorldToScreenPoint(new Vector3(randomX, 4.75f, 0));
                WarningBox.gameObject.transform.position = v;
                //cat3_WarningBox.gameObject.transform.parent = OverlayCanvas.transform;
                WarningBox.gameObject.GetComponent<Warning1_a>().time = 0f;
                WarningBox.gameObject.GetComponent<Warning1_a>().IsPooled = true;

                //return PoolingManager;
            }

            if (0 <= t - startTime)
            {
                //"t-startTime > 0 && t - (startTime + 1) < 0" 인 경우 아직 고려 안 함.
                yield return new WaitForSeconds(t - startTime);

                SetPrefabInfos(3);
                MakingObject();
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
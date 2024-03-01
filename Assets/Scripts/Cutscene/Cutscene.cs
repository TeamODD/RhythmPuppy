using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventManagement;
using Obstacles;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace CutsceneManagement
{
    public class Cutscene : MonoBehaviour
    {
        [Serializable]
        struct CutsceneInfo
        {
            public Image[] fadeinObjList, fadeoutObjList;
            public Vector3 movement;
            public AudioClip[] soundList;
            public float[] soundTimeList;
        }

        [SerializeField] Image[] sceneList;
        [SerializeField] Image[] corgiList;
        [SerializeField] Dictionary<AudioClip, float> soundEffectInfo;
        [SerializeField] CutsceneInfo[] cutsceneInfo;

        CutsceneManager cutsceneManager;
        EventManager eventManager;
        //List<Image> fadeinImageList, fadeoutImageList;
        List<GameObject> copyObjectList;
        List<Image> copyImageList;
        //List<AudioSource> soundList;
        int sceneIndex;
        Color c;
        Coroutine sceneCoroutine, soundCoroutine;

        void Awake()
        {
            int i;

            for (i = 0; i < sceneList.Length; i++)
                setImageAlpha(sceneList[i], 0);
            for (i = 0; i < corgiList.Length; i++)
                setImageAlpha(corgiList[i], 0);

            eventManager = FindObjectOfType<EventManager>();
            cutsceneManager = GetComponentInParent<CutsceneManager>();
            copyObjectList = new List<GameObject>();
            copyImageList = new List<Image>();
            //fadeinImageList = new List<Image>();
            //fadeoutImageList = new List<Image>();
            //soundList = new List<AudioSource>();
            sceneCoroutine = null;
            soundCoroutine = null;
            sceneIndex = -1;
        }

        void Start()
        {
            eventManager.uiEvent.fadeOutEvent();
            Invoke("startCutscene", 1.3f);
        }

        void startCutscene()
        {
            sceneIndex = 0;
        }

        void Update()
        {
            if (sceneIndex < 0) return;
            if (cutsceneInfo.Length <= sceneIndex)
            {
                gameObject.SetActive(false);
                return;
            }

            // ESC �Է� �� - �ƾ� ���� ��ŵ�ϱ�
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return;
            }

            // Ŭ�� �Ǵ� Ű�Է� �� - �ƾ� �ϳ��� ������ �ѱ��
            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                int i;
                // ���� ���̴� �ƾ� �ڷ�ƾ �� ȿ���� ��������
                if (sceneCoroutine != null) StopCoroutine(sceneCoroutine);
                sceneCoroutine = null;
                if (soundCoroutine != null) StopCoroutine(soundCoroutine);
                soundCoroutine = null;
                cutsceneManager.stopAllSoundEffects();
                // �ƾ� �ڷ�ƾ���� ���Ǵ� ī�� ������Ʈ�� ���� �� �ʱ�ȭ
                for (i = 0; i < copyObjectList.Count; i++)
                    Destroy(copyObjectList[i]);
                copyObjectList.Clear();
                copyImageList.Clear();
                // ���� ������Ʈ�� ����
                for (i = 0; i < cutsceneInfo[sceneIndex].fadeinObjList.Length; i++)
                    setImageAlpha(cutsceneInfo[sceneIndex].fadeinObjList[i], 1);
                for (i = 0; i < cutsceneInfo[sceneIndex].fadeoutObjList.Length; i++)
                    setImageAlpha(cutsceneInfo[sceneIndex].fadeoutObjList[i], 0);
                sceneIndex++;
                return;
            }

            if (sceneCoroutine == null)
            {
                sceneCoroutine = StartCoroutine(
                    runSceneAction(
                        cutsceneInfo[sceneIndex].fadeinObjList.ToList(),
                        cutsceneInfo[sceneIndex].fadeoutObjList.ToList(),
                        cutsceneInfo[sceneIndex].movement));
                soundCoroutine = StartCoroutine(runSoundAction(cutsceneInfo[sceneIndex].soundList, cutsceneInfo[sceneIndex].soundTimeList));
            }
        }

        IEnumerator runSceneAction(List<Image> fadeinList, List<Image> fadoutList, Vector3 distance)
        {
            float time = 0;
            List<Vector3> originalPosList = new List<Vector3>();
            int i;

            // ���⼭ ����� ��� ���� ������Ʈ�� ����Ʈ�� �켱 ����
            // ���� ������Ʈ�� �������� ī�� ������Ʈ���� ����
            for (i = 0; i < fadeinList.Count; i++)
            {
                copyObjectList.Add(Instantiate(fadeinList[i].gameObject));
                originalPosList.Add(fadeinList[i].transform.position);
                copyImageList.Add(copyObjectList[i].GetComponent<Image>());

                copyObjectList[i].transform.SetParent(fadeinList[i].transform.parent);
                copyObjectList[i].transform.position = originalPosList[i] - distance;
            }
            // ī�� ������Ʈ���� ���� ����
            while (time < 1f)
            {
                // ������ �ٲٰ� ���� ��ǥ�� Lerp()�� �̵�
                time += Time.deltaTime;
                for (i = 0; i < copyObjectList.Count; i++)
                {
                    setImageAlpha(copyImageList[i], time);
                    copyObjectList[i].transform.position = Vector3.Lerp(
                        originalPosList[i] + distance,
                        originalPosList[i],
                        time);
                }
                // ������� �ϴ� �̹����� ���̵�ƿ� ó��
                for (i = 0; i < fadoutList.Count; i++)
                    setImageAlpha(fadoutList[i], 1 - time);
                yield return null;
            }

            // �Լ� ���� �� ī�� ������Ʈ ���� �� �ʿ��� ������Ʈ�� ���̱�
            for (i = 0; i < cutsceneInfo[sceneIndex].fadeinObjList.Length; i++)
                setImageAlpha(cutsceneInfo[sceneIndex].fadeinObjList[i], 1);
            for (i = 0; i < fadoutList.Count; i++)
                setImageAlpha(fadoutList[i], 0);
            for (i = 0; i < copyObjectList.Count; i++)
                Destroy(copyObjectList[i]);
            copyObjectList.Clear();
            copyImageList.Clear();

            yield return new WaitForSeconds(0.5f);
            sceneIndex++;
            sceneCoroutine = null;
        }

        IEnumerator runSoundAction(AudioClip[] soundList, float[] soundTimeList)
        {
            float passedTime = 0;
            int i;

            for (i = 0; i < soundList.Length; i++)
            {
                yield return new WaitForSeconds(soundTimeList[i] - passedTime);
                passedTime = soundTimeList[i];
                cutsceneManager.playSoundEffect(soundList[i]);
            }
        }

        void setImageAlpha(Image image, float a)
        {
            c = image.color;
            c.a = a;
            image.color = c;
        }
    }
}
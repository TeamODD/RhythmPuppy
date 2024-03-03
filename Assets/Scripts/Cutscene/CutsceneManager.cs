using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using EventManagement;

namespace CutsceneManagement
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] GameObject tutorial_A, tutorial_B, stage1_A, stage1_B, stage2_A;
        [Space(20)]
        [SerializeField] Image darkEffectUI;
        public GameObject soundPlayerPrefab;
        public AudioClip shine, stop, love, paw, dash, think_a, think_b, warning, VS;

        EventManager eventManager;
        Transform musicManager;
        string nextStageName;
        Color c;

        void Awake()
        {
            eventManager = FindObjectOfType<EventManager>();
            musicManager = GameObject.FindGameObjectWithTag("MusicManager").transform;
            setImageAlpha(darkEffectUI, 1);

            nextStageName = PlayerPrefs.GetString("STAGE_NAME");
            PlayerPrefs.DeleteKey("STAGE_NAME");

            // µð¹ö±×¿ë ÄÚµå (±âº» ÄÆ¾À) - Stage2-1 ÄÆ¾ÀÀ» º¸¿©ÁÜ
            if (nextStageName.Length <= 0)
            {
                nextStageName = "SceneStage2_1";
                Debug.Log(string.Format("[{0}] Now Playing Cutscene (for debug) : {1}", Time.time, nextStageName));
            }
        }

        void Start()
        {
            switch (nextStageName)
            {
                case "Tutorials2":
                    StartCoroutine(runTutorialCutscene());
                    break;
                case "SceneStage1_1":
                    StartCoroutine(runStage_1Cutscene());
                    break;
                case "SceneStage2_1":
                    StartCoroutine(runStage_2Cutscene());
                    break;
                default:
                    SceneManager.LoadScene(nextStageName);
                    break;
            }
        }

        IEnumerator runTutorialCutscene()
        {
            eventManager.uiEvent.fadeOutEvent();
            yield return new WaitForSeconds(1f);
            tutorial_A.SetActive(true);
            while (tutorial_A.activeSelf)
            {
                yield return null;
            }
            tutorial_B.SetActive(true);
            while (tutorial_B.activeSelf)
            {
                yield return null;
            }
            SceneManager.LoadScene(nextStageName);
        }

        IEnumerator runStage_1Cutscene()
        {
            eventManager.uiEvent.fadeOutEvent();
            yield return new WaitForSeconds(1f);
            stage1_A.SetActive(true);
            while (stage1_A.activeSelf)
            {
                yield return null;
            }
            stage1_B.SetActive(true);
            while (stage1_B.activeSelf)
            {
                yield return null;
            }
            SceneManager.LoadScene(nextStageName);
        }

        IEnumerator runStage_2Cutscene()
        {
            eventManager.uiEvent.fadeOutEvent();
            yield return new WaitForSeconds(1f);
            stage2_A.SetActive(true);
            while (stage2_A.activeSelf)
            {
                yield return null;
            }
            /* stage1_B.SetActive(true);
            while (stage1_B.activeSelf)
            {
                yield return null;
            } */
            SceneManager.LoadScene(nextStageName);
        }

        public void playSoundEffect(AudioClip sound)
        {
            GameObject o = Instantiate(soundPlayerPrefab);
            AudioSource audioSource = o.GetComponent<AudioSource>();
            o.transform.SetParent(musicManager);
            audioSource.clip = sound;
            audioSource.Play();
        }

        public void stopAllSoundEffects()
        {
            foreach (Transform child in musicManager)
                Destroy(child.gameObject);
        }

        void setImageAlpha(Image image, float a)
        {
            c = image.color;
            c.a = a;
            image.color = c;
        }
    }
}
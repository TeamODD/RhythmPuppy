using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CutsceneManagement
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] Image darkEffectUI;
        public GameObject soundPlayerPrefab;
        public AudioClip shine, stop, love, paw, dash, think_a, think_b, warning, VS;

        Transform musicManager;
        Color c;

        void Awake()
        {
            musicManager = GameObject.FindGameObjectWithTag("MusicManager").transform;
            c = darkEffectUI.color;
            c.a = 1;
            darkEffectUI.color = c;
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
    }
}
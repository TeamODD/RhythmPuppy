using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using static Menu_PlayerTransform;

public class PlaySelectSound : MonoBehaviour
{
    private AudioSource theAudio;
    [SerializeField]
    private AudioClip[] Music_Stage;
    private AudioClip EmptyAudio;
    private float time;

    public AudioSource audioSourceSelect;
    public AudioClip audioClipSelect;


    public static PlaySelectSound instance;

    void Awake()
    {
        EmptyAudio = null;

        if(PlaySelectSound.instance == null)
            PlaySelectSound.instance = this;

        theAudio = GetComponent<AudioSource>();
    }

    public void SelectSound()
    {
        audioSourceSelect.PlayOneShot(audioClipSelect);
    }
    public void ChangeMusic()
    {
        if (Music_Stage[currentIndex] == null)
        {
            theAudio.clip = EmptyAudio;
            theAudio.Play();
        }
        theAudio.clip = Music_Stage[currentIndex];
        theAudio.Play();
    }

    //�ε� �Լ� �̱��濡 �־�����ϴ�.
    public void StartLoading(string NextScene, GameObject LoadingScreen)
    {
        StartCoroutine(asd(NextScene, LoadingScreen));
    }

    public IEnumerator asd(string NextScene, GameObject LoadingScreen)
    {
        LoadingScreen.transform.SetParent(null);
        DontDestroyOnLoad(LoadingScreen);
        yield return new WaitForSeconds(2f); //2���� �ε�
        var mAsymcOperation = SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Additive);
        yield return mAsymcOperation;
        LoadingScreen.transform.position = new Vector3(0, 0, 0);
        LoadingScreen.GetComponent<LoadingFadeOut>().FadeOut();
        mAsymcOperation = SceneManager.UnloadSceneAsync("SceneMenu_01");
        yield return mAsymcOperation;

    }
}

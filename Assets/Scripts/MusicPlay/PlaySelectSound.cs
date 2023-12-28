using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using static Menu_PlayerTransform;

public class PlaySelectSound : MonoBehaviour
{

    //아무래도 잘 못 만들어진 싱글톤 함수인듯 합니다.
    private AudioSource theAudio;
    [SerializeField]
    private AudioClip[] Music_Stage;
    private AudioClip EmptyAudio;
    private float time;

    public AudioSource audioSourceSelect;
    public AudioClip[] audioClipSelect;
    public AudioClip[] Walking;

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
        theAudio.PlayOneShot(audioClipSelect[0]);
    }
    public void MenuSelectSound()
    {
        theAudio.PlayOneShot(audioClipSelect[1]);
    }
    /*
    public void World1_Walking()
    {
        theAudio.clip = Walking[0];
        theAudio.Play();
    }
    public void World2_Walking()
    {
        theAudio.clip = Walking[1];
        theAudio.Play();
    }
    */
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

    //메뉴에서의 로딩 함수 싱글톤에 넣어놨습니다.
    public void StartLoading(string NextScene, GameObject LoadingScreen)
    {
        StartCoroutine(asd(NextScene, LoadingScreen));
    }

    public IEnumerator asd(string NextScene, GameObject LoadingScreen)
    {
        LoadingScreen.transform.SetParent(null, false); //worldpositionstays bool 인자 false로
        DontDestroyOnLoad(LoadingScreen);
        yield return new WaitForSeconds(2f); //2초후 로딩
        var mAsymcOperation = SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Single);
        LoadingScreen.GetComponent<LoadingFadeOut>().FadeOut();
        yield return mAsymcOperation;
        LoadingScreen.transform.position = new Vector3(0, 0, 0);
        /*mAsymcOperation = SceneManager.UnloadSceneAsync("SceneMenu_01");
        yield return mAsymcOperation;*/

    }
}

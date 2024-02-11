using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using static Menu_PlayerTransform;

using UIManagement;

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

        if (PlaySelectSound.instance == null)
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
        //LoadingScreen.transform.SetParent(null, false); //worldpositionstays bool ??횓??횣 false쨌횓
        //DontDestroyOnLoad(LoadingScreen);
        yield return new WaitForSeconds(2f); //2횄횎횊횆 쨌횓쨉첫
        var mAsymcOperation = SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Single);
        //LoadingScreen.GetComponent<LoadingFadeOut>().FadeOut();
        if (!(GameObject.FindWithTag("CurtainObject") == null))
            GameObject.FindWithTag("CurtainObject").GetComponent<Curtain>().CurtainEffect("Open", 0);

        Debug.Log("Destroy Loading Screen");
        Destroy(LoadingScreen);
        //로딩 시 메뉴 화면이 잠깐 보이는 버그가 있음. 
        //로딩 스크린 디스트로이를 약간 늦추면 자연스럽게 해결이 되지만, UI 이미지 사용이 어려움(loading corgi 스프라이트 ??문에)
        yield return mAsymcOperation;
        LoadingScreen.transform.position = new Vector3(0, 0, 0);
        /*mAsymcOperation = SceneManager.UnloadSceneAsync("SceneMenu_01");
        yield return mAsymcOperation;*/

    }
}

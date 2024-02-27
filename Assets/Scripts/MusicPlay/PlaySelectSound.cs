using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using static Menu_PlayerTransform;

using UIManagement;

public class PlaySelectSound : MonoBehaviour
{

    //�ƹ����� �� �� ������� �̱��� �Լ��ε� �մϴ�.
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

    public void StartLoading(string NextScene, GameObject LoadingScreen)
    {
        StartCoroutine(asd(NextScene, LoadingScreen));
    }

    public IEnumerator asd(string NextScene, GameObject LoadingScreen)
    {
        //LoadingScreen.transform.SetParent(null, false);
        //DontDestroyOnLoad(LoadingScreen);
        yield return new WaitForSeconds(2f);
        var mAsymcOperation = SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Single);
        //LoadingScreen.GetComponent<LoadingFadeOut>().FadeOut();
        if (!(GameObject.FindWithTag("CurtainObject") == null))
            GameObject.FindWithTag("CurtainObject").GetComponent<Curtain>().CurtainEffect("Open", 0);

        Debug.Log("Destroy Loading Screen");
        Destroy(LoadingScreen);
        yield return mAsymcOperation;
        LoadingScreen.transform.position = new Vector3(0, 0, 0);
    }
}

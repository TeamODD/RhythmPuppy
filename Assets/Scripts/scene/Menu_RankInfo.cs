using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_RankInfo : MonoBehaviour
{
    [SerializeField]
    private Sprite[] RankImgs;
    private string SceneName;

    void Start()
    {
        GetStageName();
        if (!PlayerPrefs.HasKey(SceneName)) return;
        Debug.Log("Rank of " + SceneName + " : "+ PlayerPrefs.GetInt(SceneName));
        gameObject.GetComponent<SpriteRenderer>().sprite = RankImgs[PlayerPrefs.GetInt(SceneName)];
    }
    
    void GetStageName()
    {
        switch(this.gameObject.name)
        {
            case "1-1": 
                SceneName = "SceneStage1_1";
                break;
            case "1-2":
                SceneName = "SceneStage1_2";
                break;
            case "1-3":
                SceneName = "SceneStage1_3";
                break;
            case "2-1":
                SceneName = "SceneStage2_1";
                break;
            case "2-2":
                SceneName = "SceneStage2_2";
                break;
            case "2-3":
                SceneName = "SceneStage2_3";
                break;

        }
    }
}

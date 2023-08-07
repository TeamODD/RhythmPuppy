using System.Collections;
using System.Collections.Generic;
using TimelineManager;
using UnityEngine;
using UnityEngine.SceneManagement;

using World_2;


public class PatternManager : MonoBehaviour
{
    [SerializeField] GameObject patternPrefab;

    public ArtifactManager artfMgr;

    private Pattern1_a Pattern1_a;
    private float yPosition;

    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        //Pattern1_a.BeeMove();

        if (scene.name.Equals("SceneStage2"))
        {

            artfMgr = FindObjectOfType<ArtifactManager>();
            GameObject o = Instantiate(patternPrefab);
            o.transform.SetParent(transform);
            o.SetActive(true);
        }
    }

    /*void Update()
    {
        yPosition = Random.Range(-5.0f, 5.0f);
    }*/
}

using System.Collections;
using System.Collections.Generic;
using TimelineManager;
using UnityEngine;
using UnityEngine.SceneManagement;

<<<<<<<< HEAD:Assets/Scripts/Options/ButtonAction.cs
public class ButtonAction : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Option;

    
    void Start()
    {
        
    }
========
using World_2;

public class PatternManager : MonoBehaviour
{
    [SerializeField] GameObject patternPrefab;

    public GameObject target;
    public GameObject Warning;
    private int count;
    public ArtifactManager artfMgr;
    public UICanvas uiCanvas;

    private Pattern1_a Pattern1_a;
    private float yPosition;

    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        //Pattern1_a.BeeMove();
>>>>>>>> dev:Assets/Scripts/Pattern/PatternManager.cs

        if (scene.name.Equals("World2"))
        {

            artfMgr = FindObjectOfType<ArtifactManager>();
            GameObject o = Instantiate(patternPrefab);
            o.transform.SetParent(transform);
            o.SetActive(true);
        }
    }
    
    void Awake()
    {
        Bee();
        count = 1;
    }
    void Bee()
    {
        Instantiate(target);
        Instantiate(Warning);
        
        count++;
        Invoke("Bee", 0.5f);
    }
    void Update()
    {
<<<<<<<< HEAD:Assets/Scripts/Options/ButtonAction.cs
        
    }

    public void onContinue()
    {
        Option.SetActive(false);
========
        if (count == 32)
            CancelInvoke("Bee");
>>>>>>>> dev:Assets/Scripts/Pattern/PatternManager.cs
    }
    //처음 두 마리 붙어서 나옴(가로)
}

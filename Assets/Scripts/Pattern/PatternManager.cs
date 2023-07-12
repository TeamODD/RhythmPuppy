using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatternManager : MonoBehaviour
{
    [SerializeField] GameObject pattern;

    private GameObject patternManager;
    private Pattern1_a Pattern1_a;
    private float yPosition;
    // Start is called before the first frame update
    void Start()
    {
        //Pattern1_a.BeeMove();

        patternManager = GameObject.FindGameObjectWithTag("PatternManager");
        GameObject o = Instantiate(pattern);
        o.transform.SetParent(patternManager.transform);
        o.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        yPosition = Random.Range(-5.0f, 5.0f);
    }
}

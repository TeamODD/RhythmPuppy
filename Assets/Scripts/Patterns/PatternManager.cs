using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obstacles;

public class PatternManager : MonoBehaviour
{
    public GameObject ObstacleManager;
    public GameObject oakA;

    private Oak_A oakAScript;

    // Start is called before the first frame update
    void Start()
    {
        oakAScript = oakA.GetComponent<Oak_A>();

        StartCoroutine(example());
    }

    // Update is called once per frame
    IEnumerator example()
    {
        GameObject o = Instantiate(oakA) as GameObject;
        o.transform.position = new Vector3(10, 1, 0);
        o.transform.SetParent(ObstacleManager.transform);
        o.GetComponent<Oak_A>().setDir(-1);
        o.SetActive(true);
        yield break;
    }
}

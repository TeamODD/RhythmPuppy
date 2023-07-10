using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatternManager : MonoBehaviour
{
    private Pattern1_a Pattern1_a;
    private float yPosition;
    // Start is called before the first frame update
    void Start()
    {
        Pattern1_a.BeeMove();
    }

    // Update is called once per frame
    void Update()
    {
        yPosition = Random.Range(-5.0f, 5.0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float ShakeAmount;
    float ShakeTime;
    Vector3 InitialPosition;

    public void VibrateForTime(float time)
    {
        ShakeTime = time;
    }

    void Start()
    {
        InitialPosition = this.transform.position;
    }

    void Update()
    {
        if (ShakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * ShakeAmount + InitialPosition;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0;
            transform.position = InitialPosition;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Head : MonoBehaviour
{
    /** head 각도 보정치*/
    float headCorrFactor { get; } = 52f;

    Transform neck;

    void Awake()
    {
        neck = GetComponent<SpriteSkin>().rootBone;
    }

    void FixedUpdate()
    {
        headToMousePos();
    }

    private void headToMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir;
        float rot, headRot;

        dir = mousePos - transform.position;
        rot = 0 < dir.y ? Vector2.Angle(dir, Vector2.right) : 360f - Vector3.Angle(dir, Vector2.right);

        headRot = rot + headCorrFactor;
        if (transform.localScale.x < 0) headRot = rot + (180 - headCorrFactor);
        neck.transform.rotation = Quaternion.Euler(0, 0, headRot);
    }
}

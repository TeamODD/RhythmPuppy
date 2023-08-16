using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Head : MonoBehaviour
{
    float correctFactor;

    Transform player, neck, head;

    void Awake()
    {
        init();
    }

    void Update()
    {
        headToMousePos();
    }

    public void init()
    {
        player = transform.parent;
        neck = GetComponent<SpriteSkin>().rootBone;
        head = neck.Find("head");
        correctFactor = neck.rotation.eulerAngles.z + head.rotation.eulerAngles.z;
    }

    private void headToMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir;
        float rot, headRot;

        dir = mousePos - neck.position;
        rot = 0 < dir.y ? Vector2.Angle(dir, Vector2.right) : 360f - Vector3.Angle(dir, Vector2.right);

        headRot = rot + correctFactor;
        if (player.localScale.x < 0) headRot = rot + (180 - correctFactor);
        neck.transform.rotation = Quaternion.Euler(0, 0, headRot);
    }
}

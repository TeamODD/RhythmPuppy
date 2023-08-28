using EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Head : MonoBehaviour
{
    [System.Serializable]
    struct Face
    {
        public Sprite normal;
        public Sprite happy;
        public Sprite sad;
        public Sprite dead;
    }

    [SerializeField] Face face;
    [SerializeField] Sprite sweat;

    EventManager eventManager;
    SpriteRenderer sp;
    Player player;
    Transform neck, head;
    float correctFactor;
    bool isAlive;
    WaitForSeconds invincibleDelay;

    void Awake()
    {
        init();
    }

    public void init()
    {
        eventManager = FindObjectOfType<EventManager>();
        player = FindObjectOfType<Player>();
        sp = GetComponent<SpriteRenderer>();
        neck = GetComponent<SpriteSkin>().rootBone;
        head = neck.Find("head");
        correctFactor = neck.rotation.eulerAngles.z + head.rotation.eulerAngles.z;
        isAlive = true;
        invincibleDelay = new WaitForSeconds(player.invincibleDuration);

        eventManager.playerEvent.playerHitEvent += playerHitEvent;
        eventManager.playerEvent.deathEvent += deathEvent;
        eventManager.playerEvent.reviveEvent += reviveEvent;
    }

    void Update()
    {
        if (isAlive) 
            headToMousePos();
    }

    private void headToMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir;
        float rot, headRot;

        dir = mousePos - neck.position;
        rot = 0 < dir.y ? Vector2.Angle(dir, Vector2.right) : 360f - Vector3.Angle(dir, Vector2.right);

        headRot = rot + correctFactor;
        if (player.transform.localScale.x < 0) headRot = rot + (180 - correctFactor);
        neck.transform.rotation = Quaternion.Euler(0, 0, headRot);
    }

    public void setNormalFace()
    {
        sp.sprite = face.normal;
    }

    public void setHappyFace()
    {
        sp.sprite = face.happy;
    }

    public void setSadFace()
    {
        sp.sprite = face.sad;
    }

    public void setDeadFace()
    {
        sp.sprite = face.dead;
        neck.transform.rotation = Quaternion.Euler(0, 0, 49f);
    }

    public void revive()
    {
        isAlive = true;
        setNormalFace();
    }

    private void playerHitEvent()
    {
        StartCoroutine(hitAction());
    }

    private IEnumerator hitAction()
    {
        setSadFace();
        yield return invincibleDelay;
        setNormalFace();
    }

    private void deathEvent()
    {
        isAlive = false;
        StopAllCoroutines();
        setDeadFace();
    }

    private void reviveEvent()
    {
        setNormalFace();
        isAlive = true;
    }
}

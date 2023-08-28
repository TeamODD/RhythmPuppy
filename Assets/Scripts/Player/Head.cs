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
    Transform neck, head, puppy;
    float correctFactor;
    bool movable;
    WaitForSeconds invincibleDelay;
    Camera mainCamera;

    void Awake()
    {
        init();
    }

    public void init()
    {
        eventManager = FindObjectOfType<EventManager>();
        player = FindObjectOfType<Player>();
        mainCamera = Camera.main;
        sp = GetComponent<SpriteRenderer>();
        neck = GetComponent<SpriteSkin>().rootBone;
        head = neck.Find("head");
        puppy = null;
        correctFactor = neck.rotation.eulerAngles.z + head.rotation.eulerAngles.z;
        movable = true;
        invincibleDelay = new WaitForSeconds(player.invincibleDuration);

        eventManager.playerEvent.playerHitEvent += playerHitEvent;
        eventManager.playerEvent.deathEvent += deathEvent;
        eventManager.playerEvent.deathEvent += freeze;
        eventManager.playerEvent.reviveEvent += reviveEvent;
        eventManager.playerEvent.reviveEvent += defreeze;
        eventManager.stageEvent.clearEvent += clearEvent;
        eventManager.stageEvent.clearEvent += freeze;
        eventManager.stageEvent.pauseEvent += freeze;
        eventManager.stageEvent.resumeEvent += defreeze;
    }

    void Update()
    {
        if (!movable) return;

        if (puppy != null &&  eventManager.stageEvent.onClear)
        {
            lookAt(puppy.transform.position);
        }
        else
        {
            lookAt(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        }

    }

    private void clearEvent()
    {
        puppy = GameObject.FindGameObjectWithTag("Puppy").transform;
    }

    private void lookAt(Vector3 pos)
    {
        Vector2 dir;
        float rot, headRot;

        dir = pos - neck.position;
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
        movable = true;
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
        StopAllCoroutines();
        setDeadFace();
    }

    private void reviveEvent()
    {
        setNormalFace();
    }

    private void freeze()
    {
        movable = false;
    }

    private void defreeze()
    {
        movable = true;
    }
}

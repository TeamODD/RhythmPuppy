using EventManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UIElements;

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

    [HideInInspector] public const float FRONT_ANGLE = 50f;

    EventManager eventManager;
    SpriteRenderer sp;
    Player player;
    Transform neck, head, puppy;
    float correctFactor;
    bool isEnabled;
    WaitForSeconds invincibleDelay;
    Camera mainCamera;
    Quaternion lowAngle, highAngle;

    void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        player = FindObjectOfType<Player>();
        mainCamera = Camera.main;
        sp = GetComponent<SpriteRenderer>();
        neck = GetComponent<SpriteSkin>().rootBone;
        head = neck.Find("head");
        lowAngle = Quaternion.Euler(0, 0, FRONT_ANGLE - 45);
        highAngle = Quaternion.Euler(0, 0, FRONT_ANGLE + 45);
        puppy = null;
        correctFactor = neck.rotation.eulerAngles.z + head.rotation.eulerAngles.z;
        isEnabled = true;
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
        if (!isEnabled) return;

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

    private void lookAt(Vector3 mousePos)
    {
        Vector2 dir;
        float rot, headAngle;

        /* Calculate Head Angle with MousePosition - ���콺 ��ġ�� ���� �÷��̾� �Ӹ� ���� ��� */
        dir = mousePos - neck.position;
        rot = 0 < dir.y ? Vector2.Angle(dir, Vector2.right) : 360f - Vector3.Angle(dir, Vector2.right);
        headAngle = rot + correctFactor;
        if (player.transform.localScale.x < 0) headAngle = rot + (180 - correctFactor);
        headAngle = 0 <= headAngle ? headAngle % 360 : headAngle % 360 + 360;

        /* 
        * If Calculated Angle is Valid, Apply to Current Player (To Prevent the Player's Head from Breaking)
        * ����� �Ϸ�� ������ �������� ��� �����ϱ� (���� �̻��ϰ�/���ϰ� ���̴� ���� ����)
        */
        if (FRONT_ANGLE - 45 < headAngle && headAngle < FRONT_ANGLE + 45)
        {
            neck.transform.rotation = Quaternion.Euler(0, 0, headAngle);
        }
        else
        {
            if ((headAngle < FRONT_ANGLE - 40) || (180 + FRONT_ANGLE < headAngle))
                neck.transform.rotation = lowAngle;
            else
                neck.transform.rotation = highAngle;
        }

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
        isEnabled = true;
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
        isEnabled = false;
    }

    private void defreeze()
    {
        isEnabled = true;
    }
}

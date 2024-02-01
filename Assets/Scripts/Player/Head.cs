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

    /* Angles Player heads to front normally, and when flipped.
     * �÷��̾ �ڿ������� ���� ���� �Ǵ� ������, �ø�(�¿����)�Ǿ��� �� ���� ���� ���� */
    public float frontAngle { get => (0 < player.localScale.x ? 50f : 130f); }
    /* Limit of head rotation angle (half of each side)
     * Head�� ȸ�� ������ ���� */
    public float rotationLimit { get => 55f; }

    EventManager eventManager;
    SpriteRenderer sp;
    Player playerScript;
    Transform player, neck, puppy;
    bool isEnabled;
    WaitForSeconds invincibleDelay;
    Camera mainCamera;

    void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        playerScript = FindObjectOfType<Player>();
        player = playerScript.transform;
        mainCamera = Camera.main;
        sp = GetComponent<SpriteRenderer>();
        neck = GetComponent<SpriteSkin>().rootBone;
        puppy = null;
        isEnabled = true;
        invincibleDelay = new WaitForSeconds(playerScript.invincibleDuration);

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

        if (puppy != null && eventManager.stageEvent.onClear)
        {
            lookAt(puppy.position);
        }
        else
        {
            lookAt((Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void clearEvent()
    {
        puppy = GameObject.FindGameObjectWithTag("Puppy").transform;
    }

    private void lookAt(Vector3 target)
    {
        float targetAngle, frontSide, backSide;

        targetAngle = getHeadingAngle(target);
        frontSide = player.localScale.x < 0 ? frontAngle + 180f : frontAngle;
        backSide = frontSide + 180f;
        /* If neck is rotated at an abnormal angle - ���� ��(�Ӹ�)�� ������������ �����ٸ� */
        if (isBetweenAngles(targetAngle, frontSide - rotationLimit, backSide))
        {
            targetAngle = frontSide - rotationLimit;   /* minus ������ �ִ� ������ ���� */
        }
        /* If neck is rotated at another abnormal angle - ���� ��(�Ӹ�)�� ������������ �����ٸ� */
        else if (isBetweenAngles(targetAngle, frontSide + rotationLimit, backSide))
        {
            targetAngle = frontSide + rotationLimit;   /* plus ������ �ִ� ������ ���� */
        }
        /* Execute rotation */
        neck.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    public float getHeadingAngle(Vector3 target)
    {
        Vector3 headDir = target - neck.position;
        return getPositiveAngle(Mathf.Atan2(headDir.y, headDir.x) * Mathf.Rad2Deg + frontAngle);
    }

    public bool isBetweenAngles(float target, float angleA, float angleB)
    {
        float fixedTarget, range;
        range = getPositiveAngle(angleB - angleA);
        fixedTarget = getPositiveAngle(target - angleA);
        if (range < 180f)
            return 0 <= fixedTarget && fixedTarget <= range;
        return range <= fixedTarget && fixedTarget <= 360;
    }

    public float getPositiveAngle(float angle)
    {
        return angle < 0 ? (angle % 360) + 360 : angle % 360;
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

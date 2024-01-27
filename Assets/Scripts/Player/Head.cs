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
     * 플레이어가 자연스럽게 앞을 보게 되는 각도와, 플립(좌우반전)되었을 때 앞을 보는 각도 */
    public float frontAngle { get => 50f; }
    /* Limit of head rotation angle (half of each side)
     * Head의 회전 가능한 각도 */
    public float rotationLimit { get => 55f; }

    EventManager eventManager;
    SpriteRenderer sp;
    Player playerScript;
    Transform player, neck, head, puppy;
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
        head = neck.Find("head");
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

        if (puppy != null &&  eventManager.stageEvent.onClear)
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
        float targetAngle = getHeadingAngle(target), fixedFrontAngle;
        fixedFrontAngle = player.localScale.x < 0 ? 180 + frontAngle : frontAngle;
 
        /* If neck is broken - 만약 목이 비정상적으로 꺾였다면 */
        if (!isBetweenAngles(targetAngle, fixedFrontAngle - rotationLimit, fixedFrontAngle + rotationLimit))
        {
            /** 현재 각도에 알맞은 최대(한계치) 각도로 설정 후 적용 */
            if (targetAngle < fixedFrontAngle)
                targetAngle = fixedFrontAngle - rotationLimit;
            else
                targetAngle = fixedFrontAngle + rotationLimit;
        }
        neck.rotation = Quaternion.Euler(0, 0, getPositiveAngle(targetAngle));
    }

    public float getHeadingAngle(Vector3 target)
    {
        Vector3 headDir = target - neck.position;
        return Mathf.Atan2(headDir.y, headDir.x) * Mathf.Rad2Deg + frontAngle;
    }

    public bool isBetweenAngles(float target, float angleA, float angleB)
    {
        float fixedTarget, low = angleA, high = angleB;
        if (angleA > angleB)
        {
            low = angleB;
            high = angleA;
        }
        fixedTarget = getPositiveAngle(target - low);
        return 0 < fixedTarget && fixedTarget < getPositiveAngle(high - low);
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

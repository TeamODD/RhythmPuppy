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

    /* The angle Player heads to front normally
     * 플레이어가 자연스럽게 앞을 보게 되는 각도  */
    public float frontAngle {
        get
        {
            if (player.localScale.x < 0)
                return 130f;
            return 50f;
        }
    }
    /* Limit of head rotation angle (half of each side)
     * Head의 회전 가능한 각도 (각 양쪽에서 절반의 각도) */
    public float rotationLimit { get => 65f; }

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
        Vector3 headDir = target - neck.position;
        float targetAngle = Mathf.Atan2(headDir.y, headDir.x) * Mathf.Rad2Deg + frontAngle;

        if (!isBetweenAngles(targetAngle, frontAngle-rotationLimit, frontAngle+rotationLimit))
        {
            float xAxis, fixedAngle;
            xAxis = player.localScale.x < 0 ? -180 : 0;
            fixedAngle = 180 - Mathf.Atan2(headDir.y, headDir.x) * Mathf.Rad2Deg - xAxis;
            if (fixedAngle < 0)
                targetAngle = frontAngle - rotationLimit;
            else
                targetAngle = frontAngle + rotationLimit;
        }
        neck.rotation = Quaternion.Euler(0, 0, getPositiveAngle(targetAngle));
    }

    public bool isBetweenAngles(float targetAngle, float lowAngle, float highAngle)
    {
        float fixedTargetAngle = getPositiveAngle(targetAngle - lowAngle);
        return 0 < fixedTargetAngle && fixedTargetAngle < getPositiveAngle(highAngle - lowAngle);
    }

    private float getPositiveAngle(float angle)
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

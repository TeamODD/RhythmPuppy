using EventManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mark : MonoBehaviour
{
    [SerializeField] float fadeDelay;

    EventManager eventManager;
    Image image;
    Color from, to;
    float progress;
    Transform player;
    Camera mainCamera;
    Vector3 pos;
    Coroutine task;

    void Awake()
    {
        image = GetComponent<Image>();
        progress = 0;
        player = transform.GetComponentInParent<Player>().transform;
        mainCamera = Camera.main;
        task = null;

        to = image.color;
        to.a = 0;
        image.color = to;
        from = to;
    }

    void Start()
    {
        eventManager = GetComponentInParent<EventManager>();
        eventManager.onMarkActivated.AddListener(markActivationEvent);
        eventManager.onMarkDeactivated.AddListener(markInactivationEvent);
        eventManager.onDeath.AddListener(deathEvent);
    }

    void Update()
    {
        pos = mainCamera.WorldToScreenPoint((Vector2)player.transform.position);
        pos.y += 225;
        transform.position = pos;
    }

    public void markActivationEvent()
    {
        /* ���� ���� ���¶��, Mark�� Ȱ��ȭ */
        if (image.color.a == 0 && task == null)
            task = StartCoroutine(fadeIn());
    }

    public void markInactivationEvent()
    {
        /* ���� Ȱ��ȭ�� ���¶��, Mark�� ��Ȱ��ȭ(����ȭ) */
        if (image.color.a == 1 && task == null)
            task = StartCoroutine(fadeOut());
    }

    private IEnumerator fadeIn()
    {
        int loop = 20;
        progress = 0;

        from = image.color;
        from.a = 0f;
        to = image.color;
        to.a = 1f;
        while (progress < 1)
        {
            progress += 1f / loop;
            image.color = Color.Lerp(from, to, progress);
            yield return new WaitForSeconds(fadeDelay / loop);
        }
        image.color = to;
        progress = 0;
        task = null;
        yield break;
    }

    private IEnumerator fadeOut()
    {
        int loop = 20;
        progress = 0;

        from = image.color;
        from.a = 1f;
        to = image.color;
        to.a = 0f;
        while (progress < 1)
        {
            progress += 1f / loop;
            image.color = Color.Lerp(from, to, progress);
            yield return new WaitForSeconds(fadeDelay / loop);
        }
        image.color = to;
        progress = 0;
        task = null;
        yield break;
    }

    private void deathEvent()
    {
        if (task != null)
            StopCoroutine(task);
        to = image.color;
        to.a = 0;
        image.color = to;
        from = to;
    }
}


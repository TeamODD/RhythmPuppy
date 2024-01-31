using EventManagement;
using System.Collections;
using System.Collections.Generic;
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
        eventManager = FindObjectOfType<EventManager>();
        image = GetComponent<Image>();
        progress = 0;
        player = transform.GetComponentInParent<Player>().transform;
        mainCamera = Camera.main;
        task = null;

        to = image.color;
        to.a = 0;
        image.color = to;
        from = to;

        eventManager.playerEvent.markActivationEvent += markActivationEvent;
        eventManager.playerEvent.markInactivationEvent += markInactivationEvent;
    }

    void Update()
    {
        pos = mainCamera.WorldToScreenPoint((Vector2)player.transform.position);
        pos.y += 225;
        transform.position = pos;
    }

    public void markActivationEvent()
    {
        if(image.color.a == 0 && task == null)
            task = StartCoroutine(fadeIn());
    }

    public void markInactivationEvent()
    {
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

        while(progress < 1)
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
}

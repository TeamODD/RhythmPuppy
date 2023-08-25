using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager.PlayerEvent;

public class Mark : MonoBehaviour
{
    [SerializeField] float fadeDelay;

    EventManager eventManager;
    SpriteRenderer sp;
    Color from, to;
    float progress;

    Coroutine task;

    void Awake()
    {
        init();
    }

    private void init()
    {
        eventManager = FindObjectOfType<EventManager>();
        sp = GetComponent<SpriteRenderer>();
        progress = 0;
        task = null;

        to = sp.color;
        to.a = 0;
        sp.color = to;
        from = to;

        eventManager.playerEvent.markActivationEvent += markActivationEvent;
        eventManager.playerEvent.markInactivationEvent += markInactivationEvent;
    }

    public void markActivationEvent()
    {
        if(sp.color.a == 0 && task == null)
            task = StartCoroutine(fadeIn());
    }

    public void markInactivationEvent()
    {
        if (sp.color.a == 1 && task == null)
            task = StartCoroutine(fadeOut());
    }

    private IEnumerator fadeIn()
    {
        int loop = 20;
        progress = 0;

        from = sp.color;
        from.a = 0f;
        to = sp.color;
        to.a = 1f;

        while(progress < 1)
        {
            progress += 1f / loop;
            sp.color = Color.Lerp(from, to, progress);
            yield return new WaitForSeconds(fadeDelay / loop);
        }
        sp.color = to;
        progress = 0;
        task = null;
        yield break;
    }

    private IEnumerator fadeOut()
    {
        int loop = 20;
        progress = 0;

        from = sp.color;
        from.a = 1f;
        to = sp.color;
        to.a = 0f;


        while (progress < 1)
        {
            progress += 1f / loop;
            sp.color = Color.Lerp(from, to, progress);
            yield return new WaitForSeconds(fadeDelay / loop);
        }
        sp.color = to;
        progress = 0;
        task = null;
        yield break;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    public GameObject[] backgrounds;
    SpriteRenderer Img;
    [SerializeField]
    CanvasGroup Text;

    float time;

    void Update()
    {
        time += Time.deltaTime;
    }

    public void backgroundAlpha(int Index, string s)
    {
        if (backgrounds[Index] == null) return;
        Img = backgrounds[Index].GetComponent<SpriteRenderer>();
        StartCoroutine(Alpha(s));
    }
    
    IEnumerator Alpha(string s)
    {
        float offset = 1f;
        float waitTime = 0.2f;
        switch (s)
        {
            case "appear":
                yield return new WaitForSeconds(waitTime);
                time = 0;
                while (time < offset)
                {
                    Img.color = new Color(1, 1f, 1f, 1 * time);
                    Text.alpha = time;
                    yield return new WaitForEndOfFrame();
                }
                Img.color = new Color(1, 1, 1, 1);
                Text.alpha = 1;
                break;

            case "disappear":
                yield return new WaitForSeconds(waitTime);
                time = 0;
                while (time < offset)
                {
                    Img.color = new Color(1, 1f, 1f, (1f-time) / time);
                    Text.alpha = 1 - time;
                    yield return new WaitForEndOfFrame();
                }
                Img.color = new Color(1, 1, 1, 0);
                Text.alpha = 0;
                break;
        }
        yield break;
    }
}

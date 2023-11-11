using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Enter : MonoBehaviour
{
    SpriteRenderer EnterSprite;
    [SerializeField]
    Sprite Enter1;
    [SerializeField]
    Sprite Enter2;
    
    void Start()
    {
        EnterSprite = this.GetComponent<SpriteRenderer>();
    }
    public void Enter(string tf)
    {
        switch (tf)
        {
            case "appear":
                EnterSprite.color = new Color(1, 1, 1, 1);
                StartCoroutine(EnterAppear(0));
                break;
            case "disappear":
                EnterSprite.color = new Color(1, 1, 1, 0);
                StopCoroutine(EnterAppear(0));
                break;
        }
    }
    IEnumerator EnterAppear(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        EnterSprite.sprite = Enter1;
        yield return new WaitForSeconds(2f);
        EnterSprite.sprite = Enter2;
        StartCoroutine(EnterAppear(2f));
    }
}

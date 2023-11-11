using System;
using System.Collections;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    private GameObject[] ChildBackGround;
    private int[] Index;
    private int[] LayerOrders;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            ChildBackGround[i] = transform.GetChild(i).gameObject;
        }

        for (int i = 0;i < ChildBackGround.Length; i++)
        {
            SpriteRenderer spriteRenderer = ChildBackGround[i].GetComponent<SpriteRenderer>();
            LayerOrders[i] = spriteRenderer.sortingOrder;
        }
    }

    private IEnumerator RemoveBackGround(GameObject background)
    {
        yield return background.transform.position.x < -29f;
        Destroy(background);
    }

    private void CopyBackGround(GameObject background)
    {
        GameObject clonedBackground = Instantiate(background);
        clonedBackground.transform.position = new Vector3(background.transform.position.x + background.GetComponent<SpriteRenderer>().bounds.size.x,
                                                           background.transform.position.y,
                                                           background.transform.position.z);
    }
}


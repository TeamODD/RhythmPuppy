using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    Transform player;
    SpriteRenderer sp;
    Color c;

    void Awake()
    {
        player = transform.parent;
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, getGroundYPos(player.position.x), 0);
        c = sp.color;
        c.a = 0.6f;
        sp.color = c;
    }

    float getGroundYPos(float playerX)
    {
        // 플레이어의 x좌표 꼭대기에서 아래쪽으로 ray를 발사해서, 그림자가 위치할 y좌표를 알아냄
        Vector2 pos = new Vector2(playerX, 30);
        LayerMask layerMask = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 100, layerMask);

        if (hit)
            return hit.point.y - 0.05f;
        return transform.position.y;
    }

    // Player.cs
    private ref SpriteRenderer getBiggerSortingOrder(ref SpriteRenderer s1, ref SpriteRenderer s2)
    {
        int s1Layer = s1.gameObject.layer, s2Layer = s2.gameObject.layer, s1SortingOrder = s1.sortingOrder, s2SortingOrder = s2.sortingOrder;
        if (LayerMask.LayerToName(s1Layer).Equals("Ground"))
            return ref s1;
        else if (LayerMask.LayerToName(s2Layer).Equals("Ground"))
            return ref s2;

        if (s1SortingOrder < s2SortingOrder)
            return ref s2;
        else if (s1SortingOrder > s2SortingOrder)
            return ref s1;

        return ref s1;
    }
}

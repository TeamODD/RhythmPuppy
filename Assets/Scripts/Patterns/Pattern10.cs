using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern10 : MonoBehaviour
{
    [SerializeField]
    private GameObject chestnut;
    [SerializeField]
    private GameObject chestnutProjectile;

    private float chestnutSpeed = 4f;
    private float splinterSpeed = 4f;
    private float splinterInterval = 45f;

    private void Start()
    {
        StartCoroutine(DropChestnuts());
    }

    private IEnumerator DropChestnuts()
    {
        while (true)
        {
            float xPos = UnityEngine.Random.Range(-7f, 7f);
            Vector3 chestnutPosition = new Vector3(xPos, 5.5f, 0f);

            GameObject newChestnut = Instantiate(chestnut, chestnutPosition, Quaternion.identity);
            Rigidbody2D chestnutRigidbody = newChestnut.GetComponent<Rigidbody2D>();
            chestnutRigidbody.velocity = Vector2.down * chestnutSpeed;

            yield return new WaitForSeconds(1f);

            ExplodeChestnut(newChestnut.transform.position);
            newChestnut.SetActive(false);
            yield return null;
        }
    }

    private void ExplodeChestnut(Vector3 position)
    {
        for (int i = 0; i < 8; i++)
        {
            float angle = i * splinterInterval;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 splinterDirection = new Vector3(x, y, 0f).normalized;
            GameObject newSplinter = Instantiate(chestnutProjectile, position, Quaternion.identity);
            newSplinter.GetComponent<MovementTransform2D>().MoveTo(splinterDirection * splinterSpeed);
        }
    }
}
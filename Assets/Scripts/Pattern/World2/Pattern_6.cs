using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_6 : MonoBehaviour
{
    [SerializeField] GameObject cat;
    [SerializeField] GameObject mark;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(runPattern());
    }

    private IEnumerator runPattern()
    {
        GameObject markObject = Instantiate(mark);
        markObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        Destroy(markObject);

        float r = Random.Range(-8f, 8f);
        GameObject catObject = Instantiate(cat);
        catObject.transform.position = new Vector3(r, 5, 0);
        Vector3 dir = (player.transform.position - catObject.transform.position);
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        /*catObject.transform.rotation = Quaternion.Euler(0, 0, rot);*/
        catObject.GetComponent<Obstacles.Cat_2>().setDir(dir);
        catObject.SetActive(true);

        Destroy(gameObject);
        yield break;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_6 : MonoBehaviour
{
    [SerializeField] GameObject cat;

    GameObject player;
    PatternManager patternManager;
    Transform obstacleManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        patternManager = transform.parent.GetComponent<PatternManager>();
        obstacleManager = patternManager.obstacleManager;
        StartCoroutine(runPattern());
    }

    private IEnumerator runPattern()
    {
        player.SendMessage("activateMark");
        yield return new WaitForSeconds(1f);
        player.SendMessage("inactivateMark");

        float r = Random.Range(-8f, 8f);
        GameObject catObject = Instantiate(cat);
        catObject.transform.SetParent(obstacleManager, false);
        catObject.transform.position = new Vector3(r, 5, 0);
        /*Vector3 dir = (player.transform.position - catObject.transform.position);
        *//*float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;*/
        /*catObject.transform.rotation = Quaternion.Euler(0, 0, rot);*//*
        catObject.GetComponent<Obstacles.Cat_2>().setDir(dir);*/
        catObject.SetActive(true);

        Destroy(gameObject);
        yield break;
    }

    /*private void warn(bool dir)
    {
        Vector2 pos = new Vector2(0, -3.6f + 0.2f);
        if (dir)
            pos.x = 10f;
        else
            pos.x = -10f;
        pos = Camera.main.WorldToScreenPoint(pos);

        GameObject o = Instantiate(patternManager.warningBox);
        o.transform.SetParent(patternManager.overlayCanvas);
        o.transform.position = pos;
        o.transform.localScale = new Vector3(700, 150, 0);
        o.SetActive(true);
    }*/
}

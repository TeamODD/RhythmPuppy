using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World_2
{
    public class Pattern_5 : MonoBehaviour
    {
        [SerializeField] GameObject ratSwarm;

        private GameObject obstacleManager;

        void OnEnable()
        {
            obstacleManager = GameObject.FindGameObjectWithTag("ObstacleManager");
            StartCoroutine(runPattern());
        }

        private IEnumerator runPattern()
        {
            bool b = Random.Range(0, 2) == 0 ? true : false;
            GameObject o = Instantiate(ratSwarm);
            o.transform.SetParent(obstacleManager.transform);
            if (b)
                o.transform.position = new Vector3(9, o.transform.position.y, o.transform.position.z);
            else
                o.transform.position = new Vector3(-9, o.transform.position.y, o.transform.position.z);
            o.SetActive(true);
            yield break;
        }
    }
}
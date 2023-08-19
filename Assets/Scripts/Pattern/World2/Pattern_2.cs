using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World_2
{
    public class Pattern_2 : MonoBehaviour
    {
        [SerializeField] GameObject paw;

        GameObject ObstacleManager;

        void Start()
        {
            ObstacleManager = GameObject.FindGameObjectWithTag("ObstacleManager");
            StartCoroutine(runPattern());
        }

        private IEnumerator runPattern()
        {
            float r = Random.Range(-8f, 8f);
            GameObject paw = Instantiate(this.paw);
            paw.transform.position = new Vector3(r, paw.transform.position.y, paw.transform.position.z);
            paw.transform.SetParent(ObstacleManager.transform);
            paw.SetActive(true);

            while(paw != null) yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);

            yield break;
        }
    }
}
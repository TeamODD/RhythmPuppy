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
            GameObject o = Instantiate(paw);
            o.transform.position = new Vector3(r, o.transform.position.y, o.transform.position.z);
            o.transform.SetParent(ObstacleManager.transform);
            o.SetActive(true);
            for (int i=10; -110<=i; i -= 1)
            {
                o.transform.Rotate(new Vector3(0, 0, -1));
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(0.5f);
            Destroy(o);
            Destroy(gameObject);

            yield break;
        }
    }
}
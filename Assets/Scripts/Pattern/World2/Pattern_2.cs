using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World_2
{
    public class Pattern_2 : MonoBehaviour
    {
        [SerializeField] GameObject paw;

        private GameObject ObstacleManager;

        void OnEnable()
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
            for (int i=90; -90<=i; i -= 1)
            {
                o.transform.Rotate(new Vector3(0, 0, -1));
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(1f);
            Destroy(o);

            yield break;
        }
    }
}
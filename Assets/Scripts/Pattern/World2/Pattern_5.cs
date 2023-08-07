using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace World_2
{
    public class Pattern_5 : MonoBehaviour
    {
        [SerializeField] GameObject ratSwarm;
        [SerializeField] int angle;
        [SerializeField] int delta;

        GameObject obstacleManager;
        ArtifactManager artfMgr;

        void Start()
        {
            obstacleManager = GameObject.FindGameObjectWithTag("ObstacleManager");
            artfMgr = transform.parent.GetComponent<PatternManager>().artfMgr;

            StartCoroutine(runPattern());
        }

        private IEnumerator runPattern()
        {
            bool r = Random.Range(0, 2) == 0 ? true : false;
            GameObject o = Instantiate(ratSwarm);
            o.transform.SetParent(obstacleManager.transform);
            if (r)
            {
                // Right
                o.transform.position = new Vector3(7, o.transform.position.y, o.transform.position.z);
                StartCoroutine(shakeManhole(artfMgr.R_Manhole));
            }
            else
            {
                // Left
                o.transform.position = new Vector3(-7, o.transform.position.y, o.transform.position.z);
                o.GetComponent<SpriteRenderer>().flipX = true;
                StartCoroutine(shakeManhole(artfMgr.L_Manhole));
            }
            o.SetActive(true);
            yield break;
        }

        private IEnumerator shakeManhole(Transform manhole)
        {
            int i = 0;

            if(manhole.Equals(artfMgr.L_Manhole))
            {
                for (; i < angle; i += delta) 
                {
                    manhole.rotation = Quaternion.Euler(0, 0, i);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(1f);
                for (; 0 < i; i -= delta)
                {
                    manhole.rotation = Quaternion.Euler(0, 0, i);
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                for (; -1 * angle < i; i -= delta)
                {
                    manhole.rotation = Quaternion.Euler(0, 0, i);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(1f);
                for (; i < 0; i += delta)
                {
                    manhole.rotation = Quaternion.Euler(0, 0, i);
                    yield return new WaitForEndOfFrame();
                }
            }

            manhole.rotation = Quaternion.Euler(0, 0, 0);
            yield break;
        }
    }
}
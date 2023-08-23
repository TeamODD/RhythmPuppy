using Obstacles;
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
        [SerializeField] float cooltime;
        [SerializeField] float duration;

        PatternManager patternManager;
        Transform obstacleManager;
        ArtifactManager artfMgr;
        WaitForSeconds warnDelay;

        void Start()
        {
            patternManager = transform.parent.GetComponent<PatternManager>();
            obstacleManager = patternManager.obstacleManager;
            artfMgr = FindObjectOfType<ArtifactManager>();
            warnDelay = new WaitForSeconds(1f);

            StartCoroutine(runPattern());
        }

        public void setDuration(float duration)
        {
            this.duration = duration;
        }

        public void setDuration(float start, float end)
        {
            this.duration = end - start + cooltime;
        }

        private IEnumerator runPattern()
        {
            bool r = Random.Range(0, 2) == 0 ? true : false;

            warn(r);

            yield return warnDelay;

            GameObject o = Instantiate(ratSwarm);
            o.transform.SetParent(obstacleManager);
            o.GetComponent<RatSwarm>().setCooltime(cooltime);
            Vector2 pos = new Vector2(0, o.transform.position.y);
            // set spawn position of ratSwarm
            if (r)
            {
                // Right
                pos.x = 10f;
                /*StartCoroutine(shakeManhole(artfMgr.R_Manhole));*/
            }
            else
            {
                // Left
                pos.x = -10f;
                o.GetComponent<SpriteRenderer>().flipX = true;
                /*StartCoroutine(shakeManhole(artfMgr.L_Manhole));*/
            }
            o.transform.position = pos;
            o.SetActive(true);

            yield return new WaitForSeconds(duration);
            Destroy(o);
            Destroy(gameObject);
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

        private void warn(bool dir)
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
        }
    }
}
using Obstacles;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Stage_2
{
    public class Pattern_5 : MonoBehaviour
    {
        [SerializeField] GameObject ratSwarm;
        [SerializeField] float cooltime;
        [SerializeField] float duration;

        EventManager eventManager;
        PatternManager patternManager;
        WaitForSeconds warnDelay;
        Camera mainCamera;

        void Start()
        {
            eventManager = FindObjectOfType<EventManager>();
            patternManager = transform.parent.GetComponent<PatternManager>();
            warnDelay = new WaitForSeconds(1f);
            mainCamera = Camera.main;

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
            o.transform.SetParent(patternManager.obstacleManager);
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

        private void warn(bool dir)
        {
            Vector2 pos = new Vector2(0, -3.6f + 0.2f);
            if (dir)
                pos.x = 10f;
            else
                pos.x = -10f;
            pos = mainCamera.WorldToScreenPoint(pos);

            patternManager.eventManager.warnWithBox(pos, new Vector3(700, 150, 0));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage_2
{
    public class Pattern_2 : MonoBehaviour
    {
        [SerializeField] GameObject paw;

        PatternManager patternManager;
        WaitForSeconds warnDelay;

        void Start()
        {
            patternManager = transform.parent.GetComponent<PatternManager>();
            warnDelay = new WaitForSeconds(1f);
            StartCoroutine(runPattern());
        }

        private IEnumerator runPattern()
        {
            float r = Random.Range(-8f, 8f);

            warn(r);

            yield return warnDelay;
            GameObject paw = Instantiate(this.paw);
            paw.transform.position = new Vector3(r, paw.transform.position.y, paw.transform.position.z);
            paw.transform.SetParent(patternManager.obstacleManager);
            paw.SetActive(true);

            while(paw != null) yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);

            yield break;
        }
        private void warn(float x)
        {
            Vector2 v = Camera.main.WorldToScreenPoint(new Vector2(x, -4.3f - 0.5f));
            patternManager.eventManager.warnWithBox(v, new Vector3(200, 500, 0));
        }
    }
}
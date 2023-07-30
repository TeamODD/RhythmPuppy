using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern6 : MonoBehaviour
{
    [SerializeField]
    private GameObject thornStem;
    [SerializeField]
    private GameObject thornStemWarning;
    [SerializeField]
    private float stemSpeed = 3f;

    private bool isPatternRunning = false;
    private GameObject currentStem;

    private void OnEnable()
    {
        StartPattern();
    }

    private void OnDisable()
    {
        StopPattern();
    }

    private void StartPattern()
    {
        if (!isPatternRunning)
        {
            isPatternRunning = true;
            StartCoroutine(RunPattern());
        }
    }

    private void StopPattern()
    {
        isPatternRunning = false;
        if (currentStem != null)
        {
            Destroy(currentStem);
            currentStem = null;
        }
        StopAllCoroutines();
    }

    private IEnumerator RunPattern()
    {
        if (isPatternRunning)
        {
            if (currentStem == null) // ���� ������ ��ֹ��� ���� ���� ���� ����
            {
                //������ ��ġ������ ����
                float startX = 9.44f;
                float startY = 0;
                Vector3 startPos = new Vector3(startX, startY, 0f);

                // ��� ������Ʈ ����
                Vector3 warningPosition = new Vector3(startX - 1f, startY, 0f);
                GameObject warning = Instantiate(thornStemWarning, warningPosition, Quaternion.identity);
                            
                Destroy(warning, 0.3f);

                // ���� �ٱ� ����
                currentStem = Instantiate(thornStem, startPos, Quaternion.identity);
                Rigidbody2D stemRigidbody = currentStem.GetComponent<Rigidbody2D>();

                // ��� ������Ʈ ����
                

                // ���������� �̵�
                if (startX < 0f)
                    stemRigidbody.velocity = Vector2.right * stemSpeed;
                // �������� �̵�
                else
                    stemRigidbody.velocity = Vector2.left * stemSpeed;

                StartCoroutine(DestroyIfOutOfBounds(currentStem));
            }

            yield return null;
        }
    }

    private IEnumerator DestroyIfOutOfBounds(GameObject obj)
    {
        while (isPatternRunning)
        {
            // �� ������ ���� ��� ������Ʈ�� �ı��մϴ�.
            if (!IsWithinMapBounds(obj.transform.position))
            {
                Destroy(obj);
                currentStem = null;
                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }

    private bool IsWithinMapBounds(Vector3 position)
    {
        float minX = -10f;
        float maxX = 12f;
        float minY = -5f;
        float maxY = 5f;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}

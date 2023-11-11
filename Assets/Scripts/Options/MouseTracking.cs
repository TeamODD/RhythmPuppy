using UnityEngine;

public class MouseTracking : MonoBehaviour
{
    [SerializeField]
    GameObject targetObjectToActivate;

    private void OnMouseEnter()
    {
        // ���콺�� ������Ʈ ���� �ö���� �� ȣ��Ǵ� �Լ�
        // Ư�� ������Ʈ�� Ȱ��ȭ�մϴ�.
        targetObjectToActivate.SetActive(true);
    }

    private void OnMouseExit()
    {
        // ���콺�� ������Ʈ ������ ����� �� ȣ��Ǵ� �Լ�
        // Ư�� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
        targetObjectToActivate.SetActive(false);
    }
}

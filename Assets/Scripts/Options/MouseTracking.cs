using UnityEngine;

public class MouseTracking : MonoBehaviour
{
    [SerializeField]
    GameObject targetObjectToActivate;

    private void OnMouseEnter()
    {
        // 마우스가 오브젝트 위에 올라왔을 때 호출되는 함수
        // 특정 오브젝트를 활성화합니다.
        targetObjectToActivate.SetActive(true);
    }

    private void OnMouseExit()
    {
        // 마우스가 오브젝트 위에서 벗어났을 때 호출되는 함수
        // 특정 오브젝트를 비활성화합니다.
        targetObjectToActivate.SetActive(false);
    }
}

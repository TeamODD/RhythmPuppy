using UnityEngine;

public class Utils : MonoBehaviour
{
    // 0~maxcount���� ��ġ�� �ʴ� n���� ������ ����
    public static int[] RandomNumbers(int maxCount, int n)
    {
        int[] defaults = new int[maxCount];
        int[] results = new int[n];

        //0���� maxCount���� �迭�� ���� ����
        for(int i = 0; i < maxCount; ++i)
        {
            defaults[i] = i;
        }

        //n���� ���� ����
        for(int i = 0; i < n; ++i)
        {
            int index = Random.Range(0, maxCount);

            results[i] = defaults[index];
            defaults[index] = defaults[maxCount - 1];

            maxCount--;
        }

        return results;
    }
}

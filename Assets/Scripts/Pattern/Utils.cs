using UnityEngine;

public class Utils : MonoBehaviour
{
    // 0~maxcount까지 겹치지 않는 n개의 난수를 생성
    public static int[] RandomNumbers(int maxCount, int n)
    {
        int[] defaults = new int[maxCount];
        int[] results = new int[n];

        //0부터 maxCount까지 배열에 숫자 지정
        for(int i = 0; i < maxCount; ++i)
        {
            defaults[i] = i;
        }

        //n개의 난수 생성
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

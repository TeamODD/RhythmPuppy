using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GrayFilmEffect : MonoBehaviour
{
    Material CameraMaterial;
    public float grayScale;
    float applytime;

    void Start()
    {
        grayScale = 0f; 
        applytime = 2f;
        CameraMaterial = new Material(Shader.Find("Custom/Grayscale"));
        //StartCoroutine(GrayPatternOn());
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        CameraMaterial.SetFloat("_Grayscale", grayScale);
        Graphics.Blit(src, dest, CameraMaterial);
    }

    public void StopGrayEffect()
    {
        StopAllCoroutines();
        grayScale = 0f;
    }

    public IEnumerator GrayPatternOn()
    {

        float elapsedtime = 0f;
        while (elapsedtime < applytime)
        {
            elapsedtime += Time.deltaTime;

            grayScale = elapsedtime / applytime;
            yield return null;
        }
        yield return new WaitForSeconds(30f);

        //Èæ¹é È¿°ú ²ô±â
        elapsedtime = 0f;
        while (elapsedtime < applytime)
        {
            elapsedtime += Time.deltaTime;

            grayScale = 1 - (elapsedtime / applytime);
            yield return null;
        }
        grayScale = 0f;
        yield break;

    }

    
}

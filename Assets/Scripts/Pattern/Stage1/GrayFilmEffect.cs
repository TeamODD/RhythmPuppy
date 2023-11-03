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
        GrayEffect();
    }
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        CameraMaterial.SetFloat("_Grayscale", grayScale);
        Graphics.Blit(src, dest, CameraMaterial);
    }

    public void GrayEffect()
    {
        StartCoroutine(CameraGrayEffect());
    }
    
    IEnumerator CameraGrayEffect()
    {
        float elapsedtime = 0f;
        while (elapsedtime < applytime)
        {
            elapsedtime += Time.deltaTime;

            grayScale = elapsedtime / applytime;
            yield return null;
        }
        yield break;
    }
}

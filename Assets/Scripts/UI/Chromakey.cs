using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chromakey : MonoBehaviour
{
    Material CameraMaterial;

    void Start()
    {
        CameraMaterial = new Material(Shader.Find("Unlit/ChromaKeyUnlit"));
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        //CameraMaterial.SetFloat("_Grayscale", 1f);
        Graphics.Blit(src, dest, CameraMaterial);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedDoG : MonoBehaviour {
    public Shader extendedDoG;

    [Range(0.0f, 5.0f)]
    public float stdev = 2.0f;

    [Range(0.1f, 5.0f)]
    public float stdevScale = 1.6f;

    [Range(0.0f, 100.0f)]
    public float Sharpness = 1.0f;

    public bool thresholding = true;

    [Range(0.0f, 100.0f)]
    public float threshold = 0.005f;

    [Range(0.0f, 5.0f)]
    public float softThreshold = 1.0f;

    public bool invert = false;

    private Material dogMat;
    
    void OnEnable() {
        dogMat = new Material(extendedDoG);
        dogMat.hideFlags = HideFlags.HideAndDontSave;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        dogMat.SetFloat("_Sigma", stdev);
        dogMat.SetFloat("_K", stdevScale);
        dogMat.SetFloat("_Tau", Sharpness);
        dogMat.SetFloat("_Phi", softThreshold);
        dogMat.SetFloat("_Threshold", threshold);
        dogMat.SetInt("_Thresholding", thresholding ? 1 : 0);
        dogMat.SetInt("_Invert", invert ? 1 : 0);

        var gaussian1 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.RG32);
        Graphics.Blit(source, gaussian1, dogMat, 0);
        var gaussian2 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.RG32);
        Graphics.Blit(gaussian1, gaussian2, dogMat, 1);

        dogMat.SetTexture("_GaussianTex", gaussian2);

        Graphics.Blit(source, destination, dogMat, 2);
        RenderTexture.ReleaseTemporary(gaussian1);
        RenderTexture.ReleaseTemporary(gaussian2);
    }
}

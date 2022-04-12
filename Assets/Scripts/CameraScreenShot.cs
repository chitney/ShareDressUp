using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreenShot : MonoBehaviour
{
    public Camera myCamera;

    public Texture2D TakeScreenShot()
    {
        gameObject.SetActive(true);
        var targetTexture = myCamera.targetTexture;
        Texture2D screenShot = new Texture2D(targetTexture.width, targetTexture.height, TextureFormat.RGB24, false);
        myCamera.Render();
        RenderTexture.active = targetTexture;
        screenShot.ReadPixels(new Rect(0, 0, targetTexture.width, targetTexture.height), 0, 0);
        RenderTexture.active = null;
        gameObject.SetActive(false);
        return screenShot;
    }
   
}

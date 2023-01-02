using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    private string m_FileName = "WinterCat_ScreenShot";
    public void PlayScreenShot()
    {
        StartCoroutine(CaptureScreenForMobile(m_FileName));
    }
    private IEnumerator CaptureScreenForMobile(string fileName)
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

        // do something with texture
        string albumName = "BRUNCH";
        NativeGallery.SaveImageToGallery(texture, albumName, fileName, (success, path) =>
        {
            Debug.Log(success);
            Debug.Log(path);
        });

        // cleanup
        Destroy(texture);
    }
}

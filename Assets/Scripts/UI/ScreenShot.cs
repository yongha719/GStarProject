using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class ScreenShot : MonoBehaviour
{
    private string m_FileName = "WinterCat_ScreenShot";

    [Header("captureHud")]
    [SerializeField] private GameObject captureHudUpParent;
    [SerializeField] private Transform captureHudUpParentPos;
    [SerializeField] private GameObject captureHudDownParent;
    [SerializeField] private Transform captureHudDownParentPos;

    [Header("captureHudAnimate")]
    private bool isHudAnimated;
    private bool isHudOn;
    public void PlayScreenShot()
    {
        StartCoroutine(CaptureScreenForMobile(m_FileName));
    }
    private IEnumerator CaptureScreenForMobile(string fileName)
    {
        NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Write);
        if (permission == NativeGallery.Permission.Denied)
        {
            if (NativeGallery.CanOpenSettings())
            {
                NativeGallery.OpenSettings();
            }
        }

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

    //UI편집관련 함수
    /// <summary>
    /// 유니티 버튼에서 사용되는 함수
    /// </summary>
    public void CaptureHudOn()
    {
        if (isHudAnimated == false)
        {
            StartCoroutine(CaptureHudOnCoroutine(3));
        }
    }
    private IEnumerator CaptureHudOnCoroutine(float animTime)
    {
        isHudAnimated = true;
        isHudOn = true;

        float time = 0;
        while (time >= animTime)
        {
            captureHudUpParent.transform.position =
                Vector3.Lerp(captureHudUpParent.transform.position, captureHudUpParentPos.position, easeOutQuint(animTime / time));
            captureHudDownParent.transform.position =
                Vector3.Lerp(captureHudDownParent.transform.position, captureHudDownParentPos.position, easeOutQuint(animTime / time));

            time += Time.deltaTime;
            yield return null;
        }
        isHudAnimated = false;
    }

    private float easeOutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }
}

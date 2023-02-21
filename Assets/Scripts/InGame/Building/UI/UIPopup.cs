using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using NativeGalleryNamespace;

/// <summary>
/// UI Background에 있는 스크립트
/// </summary>
[RequireComponent(typeof(Button))]
public class UIPopup : MonoBehaviour
{
    private RectTransform rect;
    // UI Popup의 백그라운드 이미지를 누르면 Popup이 닫히게 하려고 버튼달았음
    private Button Button;

    readonly WaitForSeconds DisableDelay = new WaitForSeconds(0.2f);

    protected virtual void Awake()
    {
        rect = transform.GetChild(0).GetComponent<RectTransform>();

        //Button = GetComponent<Button>();
        //Button.onClick.AddListener(() =>
        //{
        //    print("da");
        //    CloseUIPopup();
        //});
    }

    protected virtual void OnEnable()
    {
        rect.localScale = Vector3.zero;
        rect.DOScale(1, 0.3f);
    }

    protected virtual void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseUIPopup();
            }
        }
    }

    public void OpenUIPopup() => gameObject.SetActive(true);

    public void CloseUIPopup() => StartCoroutine(UIDisableAnimationCoroutine());

    private IEnumerator UIDisableAnimationCoroutine()
    {
        rect.DOScale(0f, 0.3f);
        yield return DisableDelay;
        gameObject.SetActive(false);
    }
}
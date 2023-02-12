using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// UI Background에 있는 스크립트
/// </summary>
[RequireComponent(typeof(Button))]
public class UIPopup : MonoBehaviour
{
    private RectTransform rect;
    // UI Popup의 백그라운드 이미지를 누르면 Popup이 닫히게 하려고 버튼달았음
    private Button Button;

    protected virtual void Awake()
    {
        rect = transform.GetChild(0).GetComponent<RectTransform>();

        Button = GetComponent<Button>();
        Button.onClick.AddListener(() => UIDisable());
    }

    protected virtual void OnEnable()
    {
        rect.localScale = Vector3.zero;
        rect.DOScale(1, 0.3f);
    }

    public void UIDisable()
    {
        StartCoroutine(DisableCoroutine());
    }

    private IEnumerator DisableCoroutine()
    {
        rect.DOScale(0f, 0.3f);
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
}

public interface IUIPopup
{
    void OpenUIPopup();
    void CloseUIPopup();
}

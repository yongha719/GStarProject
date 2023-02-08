using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// UI Background에 있는 스크립트
/// </summary>
public class UIClose : MonoBehaviour, IPointerDownHandler
{
    private RectTransform rect;

    private void Awake()
    {
        rect = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        rect.localScale = Vector3.zero;
        rect.DOScale(1, 0.3f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject == EventSystem.current.currentSelectedGameObject)
            UIPopUpHandler.Instance.UIDisable(gameObject, rect);
    }

}

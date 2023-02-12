using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// UI Background에 있는 스크립트
/// </summary>
public class UIClose : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rect;
    private GraphicRaycaster graphicRaycaster;
    CanvasRenderer CanvasRenderer;

    protected virtual void Awake()
    {
        rect = transform.GetChild(0).GetComponent<RectTransform>();
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        CanvasRenderer.
    }

    protected virtual void OnEnable()
    {
        rect.localScale = Vector3.zero;
        rect.DOScale(1, 0.3f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObject.Equals(EventSystem.current.currentSelectedGameObject))
            print("Equals");

        if (gameObject.Equals(eventData.pointerClick))
            print("pointerClick");

        if (gameObject == EventSystem.current.currentSelectedGameObject)
            print("==");

        UIDisable();
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

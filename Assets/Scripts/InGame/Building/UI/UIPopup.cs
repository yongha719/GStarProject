using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIPopup : MonoBehaviour
{
    // 뒷배경 오브젝트가 따로 있어서 그 오브젝트를 Active false로 하면
    // 꺼질때 연출이 안보여서 Popup를 active false로 하고
    // 그 뒤에 뒷배경 오브젝트를 끄는 걸로 함
    public RectTransform BackgroundRt;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        rect.localScale = new Vector3(0.4f, 0.4f, 1);
        rect.DOScale(1, 0.3f);
    }

    public void Disable()
    {
        StartCoroutine(PopupDisable());
    }

    IEnumerator PopupDisable()
    {
        rect.DOScale(0.4f, 0.3f).WaitForCompletion();
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
        BackgroundRt.gameObject.SetActive(false);
    }
}

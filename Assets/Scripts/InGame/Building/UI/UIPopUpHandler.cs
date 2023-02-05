using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public static class UIPopUpHandler
{
    public static void UIClose(this RectTransform rect)
    {
        rect.DOScale(new Vector2(0.3f, 0.3f), 1);
    }
}
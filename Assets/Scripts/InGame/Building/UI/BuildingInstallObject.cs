using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BuildingInstallObject : MonoBehaviour
{
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        rect.localScale = new Vector3(0.4f, 0.4f, 1);
        rect.DOScale(1, 0.1f);
    }
}

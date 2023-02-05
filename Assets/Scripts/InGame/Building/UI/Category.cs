using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Category : MonoBehaviour
{
    [SerializeField] private Color OnColor;
    [SerializeField] private Color OffColor;

    [SerializeField] protected GameObject CurCategoryObject;
    [SerializeField] protected bool isSelected;

    #region Component

    protected Image image;
    protected Button button;
    protected RectTransform rect;

    #endregion

    protected virtual void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();

        button.onClick.AddListener(Select);
    }

    protected virtual void Select()
    {
        image.color = OnColor;
    }

    public virtual void Unselect()
    {
        image.color = OffColor;
    }
}

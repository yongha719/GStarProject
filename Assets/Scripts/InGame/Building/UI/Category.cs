using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Category : MonoBehaviour
{
    public Sprite OnSprite;
    public Sprite OffSprite;

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
        image.sprite = OnSprite;
    }

    public virtual void Unselect()
    {
        image.sprite = OffSprite;
    }
}

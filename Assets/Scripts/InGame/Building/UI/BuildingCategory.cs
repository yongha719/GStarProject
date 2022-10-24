using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCategory : MonoBehaviour
{
    public Sprite OnSprite;
    public Sprite OffSprite;

    private static BuildingCategory s_CurCategory;
    [SerializeField] private bool isSelected;
    [SerializeField] private GameObject CurBuildings;

    #region Component

    private Image image;
    private Button button;
    private RectTransform rect;

    #endregion

    private void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();

        button.onClick.AddListener(Select);

        if (isSelected)
        {
            s_CurCategory = this;
        }
    }

    private void Select()
    {
        if (s_CurCategory == this)
            return;

        s_CurCategory.Unselect();
        s_CurCategory = this;
        image.sprite = OnSprite;
        rect.DOAnchorPosY(-70f, 0.3f);
    }

    private void Unselect()
    {
        image.sprite = OffSprite;
        rect.DOAnchorPosY(60f, 0.3f);
    }
}

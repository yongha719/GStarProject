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
    private static GameObject s_CurCategoryBuildings;

    [SerializeField] private GameObject CurCategoryBuildings;
    [SerializeField] private bool isSelected;

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
            s_CurCategoryBuildings = CurCategoryBuildings;
        }
    }

    private void Select()
    {
        if (s_CurCategory == this)
            return;

        s_CurCategory.Unselect();
        s_CurCategory = this;


        s_CurCategoryBuildings.SetActive(false);
        CurCategoryBuildings.SetActive(true);
        s_CurCategoryBuildings = CurCategoryBuildings;

        image.sprite = OnSprite;
        rect.DOAnchorPosY(-70f, 0.3f);
    }

    private void Unselect()
    {
        image.sprite = OffSprite;
        rect.DOAnchorPosY(60f, 0.3f);
    }
}

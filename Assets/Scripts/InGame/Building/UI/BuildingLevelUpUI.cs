using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingLevelUpUI : UIPopup
{
    [SerializeField] private Canvas canvas;

    [Header("UI")]
    [SerializeField] private Image buildingImage;
    [SerializeField] private TaskText WorkingCatText;
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI curLevelText;
    [SerializeField] private TextMeshProUGUI curLevelUpCostText;
    [SerializeField] private TextMeshProUGUI nextLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelUpCostText;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private TextMeshProUGUI levelUpCostText;
    private RectTransform levelUpButtonRt;

    [SerializeField] private List<GameObject> WorkingCatPlacements = new List<GameObject>();

    [Space]
    [SerializeField] private GameObject LevelUpEffect;
    [SerializeField] private Transform WoringcatParent;

    private const string WORKING = "이(가) 일하는 중";

    private ProductionBuilding building;

    void Start()
    {
        levelUpButtonRt = levelUpButton.GetComponent<RectTransform>();
        levelUpButton.onClick.AddListener(() =>
        {
            // Canvas Render Mode가 Screen Space - Camera일때 캔버스상의 좌표를 월드 좌표로 얻어내는 공식
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, levelUpButtonRt.position);
            Vector3 result = Vector3.zero;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(levelUpButtonRt, screenPos, canvas.worldCamera, out result);
            //Instantiate(LevelUpEffect, result, Quaternion.identity);

            if (building.LevelUpCostToString(building.Level).returnValue() <= GameManager.Instance._coin)
                building.Level++;
            else
            {
                // 골드 경고창
            }

            ValueChange();
        });
    }

    /// <summary>
    /// 상세정보창에 띄울 건물 정보들
    /// </summary>
    public void SetData(ProductionBuilding building)
    {
        this.building = building;
        buildingNameText.text = building.BuildingName;
        buildingImage.sprite = building.BuildingSprite;
        WorkingCatText.SetText();

        if (building.WorkingCatsUI != null)
        {
            var workingCat = Instantiate(building.WorkingCatsUI.gameObject, WoringcatParent).GetComponent<CatPlacementWorkingCatsUI>();

            if (building.PlacedInBuildingCats != null)
            {
                WorkingCatText.SetText(building.PlacedInBuildingCats[0].catData.Name + WORKING);

                for (int i = 0; i < building.PlacedInBuildingCats.Count; i++)
                {
                    workingCat.SetData(i, building.PlacedInBuildingCats[i].catData);
                }
            }
        }

        ValueChange();
    }

    void ValueChange()
    {
        curLevelText.text = $"Lv. {building.Level}";
        curLevelUpCostText.text = building.LevelUpCostToString(building.Level);
        nextLevelText.text = $"Lv. {building.Level + 1}";
        nextLevelUpCostText.text = building.LevelUpCostToString(building.Level + 1);
        levelUpCostText.text = building.LevelUpCostToString(building.Level);
    }
}

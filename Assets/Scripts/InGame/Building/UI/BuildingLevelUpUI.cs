using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingLevelUpUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    [Header("UI")]
    [SerializeField] private Image buildingImage;
    [SerializeField] private TextMeshProUGUI curLevelText;
    [SerializeField] private TextMeshProUGUI curLevelUpCostText;
    [SerializeField] private TextMeshProUGUI nextLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelUpCostText;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private TextMeshProUGUI levelUpCostText;
    private RectTransform levelUpButtonRt;

    [Space]
    [SerializeField] private GameObject LevelUpEffect;
    [SerializeField] private Transform WoringcatParent;
    [SerializeField] List<GameObject> GoldBuildingWorkingCats;
    [SerializeField] List<GameObject> EnergyBuildingWorkingCats;

    private BuildingType buildingType;
    private GoldProductionBuilding goldBuilding;
    private EnergyProductionBuilding energyBuilding;
    
    void Start()
    {
        levelUpButtonRt = levelUpButton.GetComponent<RectTransform>();
        levelUpButton.onClick.AddListener(() =>
        {
            // Canvas Render Mode가 Screen Space - Camera일때 캔버스상의 좌표를 월드 좌표로 얻어내는 공식
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, levelUpButtonRt.position);
            Vector3 result = Vector3.zero;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(levelUpButtonRt, screenPos, canvas.worldCamera, out result);
            Instantiate(LevelUpEffect, result, Quaternion.identity);

            if(buildingType == BuildingType.Gold)
            {
                goldBuilding.Level++;
            }
            else if(buildingType == BuildingType.Energy)
            {
                energyBuilding.Level++;
            }

            ValueChange();
        });
    }

    /// <summary>
    /// 
    /// </summary>
    void ValueChange()
    {
        if (buildingType == BuildingType.Gold)
        {
            curLevelText.text = $"Lv. {goldBuilding.Level}";
            curLevelUpCostText.text = goldBuilding.LevelUpCostToString(goldBuilding.Level);
            nextLevelText.text = $"Lv. {goldBuilding.Level + 1}";
            nextLevelUpCostText.text = goldBuilding.LevelUpCostToString(goldBuilding.Level + 1);
            levelUpCostText.text = goldBuilding.LevelUpCostToString(goldBuilding.Level);
        }
        else if (buildingType == BuildingType.Energy)
        {
            curLevelText.text = $"Lv. {energyBuilding.Level}";
            curLevelUpCostText.text = energyBuilding.LevelUpCostToString(energyBuilding.Level);
            nextLevelText.text = $"Lv. {energyBuilding.Level + 1}";
            nextLevelUpCostText.text = energyBuilding.LevelUpCostToString(energyBuilding.Level + 1);
            levelUpCostText.text = goldBuilding.LevelUpCostToString(energyBuilding.Level);
        }
    }

    /// <summary>
    /// 상세정보창에 띄울 건물 정보들
    /// </summary>
    public void SetData(BuildingType buildingType, ProductionBuilding building, List<Cat> placedInBuildingCats, Sprite builldingSprite)
    {
        this.buildingType = buildingType;
        buildingImage.sprite = builldingSprite;

        if (building is GoldProductionBuilding)
        {
            goldBuilding = building as GoldProductionBuilding;
            var workingcat = Instantiate(GoldBuildingWorkingCats[goldBuilding.BuildinTypeToInt], WoringcatParent).GetComponent<CatPlacementWorkingCats>();
            workingcat.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);

            if (placedInBuildingCats != null)
                for (int i = 0; i < placedInBuildingCats.Count; i++)
                {
                    workingcat.SetData(i, placedInBuildingCats[i].Animator);
                }
        }
        else if (building is EnergyProductionBuilding)
        {
            energyBuilding = building as EnergyProductionBuilding;
            var workingcat = Instantiate(EnergyBuildingWorkingCats[energyBuilding.BuildinTypeToInt], WoringcatParent).GetComponent<CatPlacementWorkingCats>();

            if (placedInBuildingCats != null)
                for (int i = 0; i < placedInBuildingCats.Count; i++)
                {
                    workingcat.SetData(i, placedInBuildingCats[i].Animator);
                }
        }

        ValueChange();
    }
}

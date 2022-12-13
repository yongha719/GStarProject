using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingInfomation : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    [Header("UI")]
    [SerializeField] private Image BuildingImage;
    [SerializeField] private Animator Animator;
    [SerializeField] private TextMeshProUGUI CurLevelText;
    [SerializeField] private TextMeshProUGUI CurLevelUpCostText;
    [SerializeField] private TextMeshProUGUI NextLevelText;
    [SerializeField] private TextMeshProUGUI NextLevelUpCostText;
    [SerializeField] private Button LevelUpButton;
    [SerializeField] private TextMeshProUGUI LevelUpCostText;
    private RectTransform LevelUpButtonRt;

    [Space]
    [SerializeField] private GameObject LevelUpEffect;
    [SerializeField] private Transform WoringcatParent;
    [SerializeField] List<GameObject> GoldBuildingWorkingCats;
    [SerializeField] List<GameObject> EnergyBuildingWorkingCats;
    [SerializeField] private NotEnoughGold NotEnoughGold;

    private BuildingType BuildingType;
    private GoldProductionBuilding goldBuilding;
    private EnergyProductionBuilding energyBuilding;

    void Start()
    {
        LevelUpButtonRt = LevelUpButton.GetComponent<RectTransform>();
        LevelUpButton.onClick.AddListener(() =>
        {
            // Canvas Render Mode가 Screen Space - Camera일때 캔버스상의 좌표를 월드 좌표로 얻어내는 공식
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, LevelUpButtonRt.position);
            Vector3 result = Vector3.zero;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(LevelUpButtonRt, screenPos, canvas.worldCamera, out result);
            Instantiate(LevelUpEffect, result, Quaternion.identity);

            if(BuildingType == BuildingType.Gold)
            {
                goldBuilding.Level++;
            }
            else if(BuildingType == BuildingType.Energy)
            {
                energyBuilding.Level++;

            }

            ValueChange();
        });
    }

    void ValueChange()
    {
        print("Daw");
        if (BuildingType == BuildingType.Gold)
        {
            CurLevelText.text = $"Lv. {goldBuilding.Level}";
            CurLevelUpCostText.text = goldBuilding.LevelUpCost(goldBuilding.Level);
            NextLevelText.text = $"Lv. {goldBuilding.Level + 1}";
            NextLevelUpCostText.text = goldBuilding.LevelUpCost(goldBuilding.Level + 1);
            LevelUpCostText.text = goldBuilding.LevelUpCost(goldBuilding.Level);
        }
        else if (BuildingType == BuildingType.Energy)
        {
            CurLevelText.text = $"Lv. {energyBuilding.Level}";
            CurLevelUpCostText.text = energyBuilding.LevelUpCost(energyBuilding.Level);
            NextLevelText.text = $"Lv. {energyBuilding.Level + 1}";
            NextLevelUpCostText.text = energyBuilding.LevelUpCost(energyBuilding.Level + 1);
            LevelUpCostText.text = goldBuilding.LevelUpCost(energyBuilding.Level);
        }
    }

    public void SetData(BuildingType buildingType, ProductionBuilding building, List<Cat> placedInBuildingCats, Sprite builldingSprite)
    {
        BuildingType = buildingType;
        BuildingImage.sprite = builldingSprite;

        if (building is GoldProductionBuilding)
        {
            goldBuilding = building as GoldProductionBuilding;
            var workingcat = Instantiate(GoldBuildingWorkingCats[(int)goldBuilding.buildingType], WoringcatParent).GetComponent<CatPlacementWorkingCats>();
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
            var workingcat = Instantiate(EnergyBuildingWorkingCats[(int)energyBuilding.buildingType], WoringcatParent).GetComponent<CatPlacementWorkingCats>();

            if (placedInBuildingCats != null)
                for (int i = 0; i < placedInBuildingCats.Count; i++)
                {
                    workingcat.SetData(i, placedInBuildingCats[i].Animator);
                }
        }

        ValueChange();
    }
}

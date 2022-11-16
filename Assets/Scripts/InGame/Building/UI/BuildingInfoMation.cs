using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

            ValueChange();
        });
    }

    private void OnEnable()
    {
        ValueChange();
    }

    void ValueChange()
    {
        if (BuildingType == BuildingType.Gold)
        {
            CurLevelText.text = $"Lv. {goldBuilding.Rating}";
            CurLevelUpCostText.text = goldBuilding.LevelUpCost(goldBuilding.Rating);
            NextLevelText.text = $"Lv. {goldBuilding.Rating + 1}";
            NextLevelUpCostText.text = goldBuilding.LevelUpCost(goldBuilding.Rating + 1);
        }
        else if (BuildingType == BuildingType.Energy)
        {
            CurLevelText.text = $"Lv. {energyBuilding.Rating}";
            CurLevelUpCostText.text = energyBuilding.LevelUpCost(energyBuilding.Rating);
            NextLevelText.text = $"Lv. {energyBuilding.Rating + 1}";
            NextLevelUpCostText.text = energyBuilding.LevelUpCost(energyBuilding.Rating + 1);
        }
    }

    public void SetData(BuildingType buildingType, IResourceProductionBuilding building, List<Cat> placedInBuildingCats, Sprite builldingSprite)
    {
        BuildingType = buildingType;
        BuildingImage.sprite = builldingSprite;

        if (building is GoldProductionBuilding)
        {
            goldBuilding = building as GoldProductionBuilding;
            var workingcat = Instantiate(GoldBuildingWorkingCats[(int)goldBuilding.buildingType], WoringcatParent).GetComponent<CatPlacementWorkingCats>();

            for (int i = 0; i < placedInBuildingCats.Count; i++)
            {
                workingcat.SetData(i, placedInBuildingCats[i].Animator);
            }

        }
        else if (building is EnergyProductionBuilding)
        {
            energyBuilding = building as EnergyProductionBuilding;
            var workingcat = Instantiate(EnergyBuildingWorkingCats[(int)energyBuilding.buildingType], WoringcatParent).GetComponent<CatPlacementWorkingCats>();

            for (int i = 0; i < placedInBuildingCats.Count; i++)
            {
                workingcat.SetData(i, placedInBuildingCats[i].Animator);
            }
        }
    }
}

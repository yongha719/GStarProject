using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BuildingInfomation : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI LevelUpCostText;
    [SerializeField] private Button LevelUpButton;
    private RectTransform LevelUpButtonRt;
    [SerializeField] private GameObject LevelUpEffect;

    [Space]
    [SerializeField] private Transform WoringcatParent;
    [SerializeField] List<GameObject> GoldBuildingWorkingCats;
    [SerializeField] List<GameObject> EnergyBuildingWorkingCats;
    [SerializeField] private NotEnoughGold NotEnoughGold;



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

    }

    void SetData(IResourceProductionBuilding building, List<Cat> placedInBuildingCats)
    {
        if (building is GoldProductionBuilding gold)
        {
            gold = building as GoldProductionBuilding;
            var workingcat = Instantiate(GoldBuildingWorkingCats[(int)gold.buildingType], WoringcatParent).GetComponent<CatPlacementWorkingCats>();

            for (int i = 0; i < placedInBuildingCats.Count; i++)
            {
                workingcat.SetData(i, placedInBuildingCats[i].Animator);
            }
        }
        else if (building is EnergyProductionBuilding energy)
        {
            energy = building as EnergyProductionBuilding;
            var workingcat = Instantiate(GoldBuildingWorkingCats[(int)energy.buildingType], WoringcatParent).GetComponent<CatPlacementWorkingCats>();

            for (int i = 0; i < placedInBuildingCats.Count; i++)
            {
                workingcat.SetData(i, placedInBuildingCats[i].Animator);
            }
        }
    }
}

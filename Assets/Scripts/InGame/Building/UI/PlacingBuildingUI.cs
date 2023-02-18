using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 건물 설치하는 UI
/// </summary>
public class PlacingBuildingUI : UIPopup
{
    #region Buildings
    [SerializeField] private Button CloseButton;

    [Header("Placing Building Infos")]
    [SerializeField] private List<BuildingBuyInfoUI> GoldProductionBuildingInfos;
    [SerializeField] private List<BuildingBuyInfoUI> EnergyProductionBuildingInfos;

    [Header("Warning UI")]
    [SerializeField] private PlacingBuildingWarningUI Warning;
    [SerializeField] private NotEnoughGoldWarningUI NotEnoughGold;


    #endregion

    [SerializeField] private CatPlacementUI CatPlacement;

    private GridBuildingSystem GridBuildingSystem;
    private GameManager GameManager;



    void Start()
    {
        GridBuildingSystem = GridBuildingSystem.Instance;
        GameManager = GameManager.Instance;
        UIPopUpHandler.Instance.BuildingPlacingPopup = gameObject; 

        UISetting();
    }


    private void UISetting()
    {
        SetProductBuildingInfo<GoldProductionBuilding>(GoldProductionBuildingInfos);
        SetProductBuildingInfo<EnergyProductionBuilding>(EnergyProductionBuildingInfos);

        CloseButton.onClick.AddListener(() => CloseUIPopup());
    }

    void SetProductBuildingInfo<T>(List<BuildingBuyInfoUI> ProductionBuildingInfos) where T : ProductionBuilding
    {
        foreach (var buildingInfo in ProductionBuildingInfos)
        {
            buildingInfo.BuyButtonOnclick((Building) =>
            {
                var produtionbuilding = Building as T;

                if (produtionbuilding == null)
                    Debug.Assert(false, "설치할 건물 정보가 잘못됨");

                produtionbuilding.BuildingInfo = buildingInfo;

                Building.CatPlacement = CatPlacement;

                if (GameManager._coin > 0 && GameManager._coin >= produtionbuilding.PlacingPrice.returnValue())
                {
                    Warning.OpenUIPopup();
                    Warning.SetWarningData(buildingInfo.BuildingPrefab, produtionbuilding.BuildingName, this);
                }
                else
                    NotEnoughGold.OpenUIPopup();
            });
        }
    }
}

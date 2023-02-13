using System.Collections.Generic;
using UnityEngine;

public class PlacingBuildingUI : MonoBehaviour
{
    #region Buildings

    [Header("Placing Building Infos")]
    [SerializeField] private List<BuildingBuyInfoUI> GoldProductionBuildingInfos;
    [SerializeField] private List<BuildingBuyInfoUI> EnergyProductionBuildingInfos;

    [Header("Warning UI")]
    [SerializeField] private PlacingBuildingWarningUI Warning;
    [SerializeField] private NotEnoughGoldWarningUI NotEnoughGold;

    #endregion

    [SerializeField] private CatPlacement CatPlacement;

    private GridBuildingSystem GridBuildingSystem;
    private GameManager GameManager;


    void Start()
    {
        print("UI");
        GridBuildingSystem = GridBuildingSystem.Instance;
        GameManager = GameManager.Instance;
        UIPopUpHandler.Instance.BuildingPlacingPopup = gameObject; 

        UISetting();
    }


    private void UISetting()
    {
        SetProductBuildingInfo<GoldProductionBuilding>(GoldProductionBuildingInfos);
        SetProductBuildingInfo<EnergyProductionBuilding>(EnergyProductionBuildingInfos);
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
                buildingInfo.Building.FirstTimeInstallation = true;

                Building.CatPlacement = CatPlacement;

                if (GameManager._coin > 0 && GameManager._coin >= produtionbuilding.PlacingPrice.returnValue())
                {
                    Warning.WarningUISetActive(true);
                    Warning.SetWarningData(buildingInfo.BuildingPrefab, produtionbuilding.BuildingName);
                    //UIPopUpHandler.Instance.OnUIWarningPopUp(UIWarningPopupType.PlacingBuildingWarning);
                }
                else
                    NotEnoughGold.gameObject.SetActive(true);
            });
        }
    }
}

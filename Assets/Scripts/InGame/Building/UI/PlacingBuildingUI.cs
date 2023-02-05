using System.Collections.Generic;
using UnityEngine;

public class PlacingBuildingUI : MonoBehaviour
{
    #region Buildings

    [Header("WorkShop Buildings")]
    [SerializeField] private List<BuildingBuyInfoUI> GoldProductionBuildingInfos;
    [SerializeField] private List<BuildingBuyInfoUI> EnergyProductionBuildingInfos;

    [Header("Warning UIs")]
    [SerializeField] private PlacingBuildingWarningUI Warning;
    [SerializeField] private NotEnoughGoldWarningUI NotEnoughGold;

    #endregion

    #region UI Object

    [SerializeField] private GameObject PlacingBuilding;
    [SerializeField] private CatPlacement CatPlacement;
    [SerializeField] private BuildingInfomationUI BuildingInfomation;
    #endregion

    private GridBuildingSystem GridBuildingSystem;
    private GameManager GameManager;

    private void Awake()
    {
        BuildingManager.Init();
    }

    void Start()
    {
        GridBuildingSystem = GridBuildingSystem.Instance;
        GameManager = GameManager.Instance;

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
                var produtionbuilding = Building as T; ;

                if (produtionbuilding == null)
                    Debug.Assert(false, "설치할 건물 정보가 잘못됨");

                produtionbuilding.BuildingInfo = buildingInfo;
                buildingInfo.BuildingInstalltionUI = PlacingBuilding;
                buildingInfo.Building.FirstTimeInstallation = true;

                Building.CatPlacement = CatPlacement;

                if (GameManager._coin > 0 && GameManager._coin >= produtionbuilding.ConstructionCost.returnValue())
                {
                    Warning.WarningUI.SetActive(true);
                    Warning.SetWarningData(buildingInfo.BuildingPrefab, produtionbuilding.BuildingName, PlacingBuilding);
                }
                else
                    NotEnoughGold.gameObject.SetActive(true);
            });
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BuildingInstallationUI : MonoBehaviour
{
    private GameObject CurBuilding;
    private string CurBuildingName;

    #region Buildings

    [Header("WorkShop Buildings")]
    [SerializeField] private List<BuildingInfo> GoldProductionBuildingInfos;
    [SerializeField] private List<BuildingInfo> EnergyProductionBuildingInfos;

    [Header("Warning UIs")]
    [SerializeField] private BuildingInstallWarning Warning;
    [SerializeField] private NotEnoughGold NotEnoughGold;



    #endregion

    #region Category

         

    #endregion

    #region UI Object

    [SerializeField] private GameObject BuildingInstallation;
    [SerializeField] private CatPlacement CatPlacement;
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
        foreach (var buildingInfo in GoldProductionBuildingInfos)
        {
            buildingInfo.BuyButtonOnclick((Building) =>
            {
                GoldProductionBuilding goldprodutionbuilding = null;

                if (Building is GoldProductionBuilding)
                    goldprodutionbuilding = Building as GoldProductionBuilding;

                CurBuilding = buildingInfo.BuildingPrefab;
                CurBuildingName = goldprodutionbuilding.BuildingName;

                goldprodutionbuilding.BuildingInfo = buildingInfo;
                buildingInfo.BuildingInstalltionUI = BuildingInstallation;
                buildingInfo.Building.FirstTimeInstallation = true;

                Building.CatPlacement = CatPlacement;

                if (GameManager._coin > 0 && GameManager._coin >= goldprodutionbuilding.ConstructionCost.returnValue())
                {
                    Warning.WarningUI.SetActive(true);
                    Warning.SetWarningData(CurBuilding, CurBuildingName, BuildingInstallation);
                }
                else
                {
                    NotEnoughGold.NotEnoughGoldUI.SetActive(true);
                }
            });
        }

        foreach (var buildingInfo in EnergyProductionBuildingInfos)
        {
            buildingInfo.BuyButtonOnclick((Building) =>
            {
                EnergyProductionBuilding energyprodutionbuilding = null;

                if (Building is EnergyProductionBuilding)
                    energyprodutionbuilding = Building as EnergyProductionBuilding;

                CurBuilding = buildingInfo.BuildingPrefab;
                CurBuildingName = energyprodutionbuilding.BuildingName;

                energyprodutionbuilding.BuildingInfo = buildingInfo;
                buildingInfo.BuildingInstalltionUI = BuildingInstallation;
                buildingInfo.Building.FirstTimeInstallation = true;

                Building.CatPlacement = CatPlacement;

                if (GameManager._coin > 0 && GameManager._coin >= energyprodutionbuilding.ConstructionCost.returnValue())
                {
                    Warning.WarningUI.SetActive(true);
                    Warning.SetWarningData(CurBuilding, CurBuildingName, BuildingInstallation);
                }
                else
                {
                    NotEnoughGold.NotEnoughGoldUI.SetActive(true);
                }
            });
        }
    }
}

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

    [Header("Warning UIs")]
    [SerializeField] private Warning Warning;
    [SerializeField] private NotEnoughGold NotEnoughGold;

    #endregion

    #region UI Object

    [SerializeField] private GameObject BuildingInstallation;

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

    void Update()
    {

    }

    private void UISetting()
    {
        foreach (var buildingInfo in GoldProductionBuildingInfos)
        {
            buildingInfo.BuyButtonOnclick((Building) =>
            {
                var goldprodutionbuilding = Building as GoldProductionBuilding;

                CurBuilding = buildingInfo.BuildingPrefab;
                CurBuildingName = goldprodutionbuilding.BuildingName;

                goldprodutionbuilding.BuildingInfo = buildingInfo;
                buildingInfo.BuildingInstalltionUI = BuildingInstallation;
                buildingInfo.Building.FirstTimeInstallation = true;


                print(GameManager._coin);
                print(GameManager._coin >= goldprodutionbuilding.ConstructionCost.returnValue());
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


    }
}

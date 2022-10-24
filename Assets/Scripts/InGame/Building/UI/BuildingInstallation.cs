using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class BuildingInstallation : MonoBehaviour
{
    private GameObject CurBuilding;
    private string CurBuildingName;

    #region Buildings

    [Header("WorkShop Buildings")]
    [SerializeField] private List<BuildingInfo> WorkShopBuildingInfos;

    [Header("Warning UIs")]
    [SerializeField] private Warning Warning;
    [SerializeField] private NotEnoughGold NotEnoughGold;

    #endregion

    #region UI Object

    [SerializeField] private GameObject BuildingInstallationUI;

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
        foreach (var buildingInfo in WorkShopBuildingInfos)
        {
            buildingInfo.BuyButtonOnclick((Building) =>
            {
                CurBuilding = buildingInfo.BuildingPrefab;
                CurBuildingName = Building.BuildingName;

                Building.BuildingInfo = buildingInfo;
                buildingInfo.BuildingInstalltionUI = BuildingInstallationUI;
                buildingInfo.Building.FirstTimeInstallation = true;

                if (GameManager._coin > 0 /*&& GameManager._coin >= Building.ProductionGold*/)
                {
                    Warning.WarningUI.SetActive(true);
                    Warning.SetWarningData(CurBuilding, CurBuildingName, BuildingInstallationUI);
                }
                else
                {
                    NotEnoughGold.NotEnoughGoldUI.SetActive(true);
                }
            });
        }


    }
}

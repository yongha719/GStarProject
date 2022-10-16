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

    [Header("Warning")]
    [SerializeField] private Warning Warning;
    [SerializeField] private NotEnoughGold NotEnoughGold;

    #endregion

    #region UI Object

    [SerializeField] private GameObject BuildingInstallationUI;

    #endregion

    private GridBuildingSystem GridBuildingSystem;

    void Start()
    {
        GridBuildingSystem = GridBuildingSystem.Instance;

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
                CurBuilding = buildingInfo.buildingPrefab;
                CurBuildingName = buildingInfo.buildingName;

                Building.BuildingInfo = buildingInfo;
                buildingInfo.BuildingInstalltionUI = BuildingInstallationUI;
                buildingInfo.Building.FirstTimeInstallation = true;

                print(GameManager.Instance._coin >= buildingInfo.Gold);
                if (GameManager.Instance._coin >= buildingInfo.Gold)
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

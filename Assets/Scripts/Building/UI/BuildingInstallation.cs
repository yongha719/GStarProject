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

    [Header("")]

    [Header("WorkShop Buildings")]
    [SerializeField] private List<BuildingInfo> WorkShopBuildings;

    [Header("Warning")]
    [SerializeField] private Warning Warning;

    #endregion

    #region UI Object

    [SerializeField] private GameObject BuildingInstallationObject;

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
        foreach (var building in WorkShopBuildings)
        {
            building.BuyButtonOnclick(() =>
            {
                CurBuilding = building.buildingPrefab;
                building.Building.BuildingInfo = building;
                CurBuildingName = building.buildingName;

                Warning.SetWarningData(CurBuilding, CurBuildingName,
                action: () =>
                {
                    BuildingInstallationObject.SetActive(true);
                });
            });
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class BuildingInstallation : MonoBehaviour
{
    private GameObject CurBuilding;
    private string CurBuildingName;

    #region WorkShop Buildings

    [Header("WorkShop Buildings")]
    [SerializeField] private List<BuildingInfo> WorkShopBuildings;

    [Header("Warning")]
    [SerializeField] private Warning Warning;

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
            print(building.name);
            building.Onclick(() =>
            {
                CurBuildingName = building.buildingName;
                CurBuilding = building.building;

                //Warning.gameObject.SetActive(true);
                Warning.SetWarningData(CurBuilding, CurBuildingName);
            });
        }
    }
}

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

    /// <summary>
    /// BuildingInstallation UI Object SetActive
    /// </summary>
    public void BuildingInstallationUISetActive(bool value)
    {
        BuildingInstallationObject.SetActive(value);
    }

    private void UISetting()
    {
        foreach (var building in WorkShopBuildings)
        {
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

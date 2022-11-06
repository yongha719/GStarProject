using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuildingInstallWarning : Warning
{
    private GameObject CurBuilding;

    // 건물 설치 UI
    private GameObject BuildingInstallationUI;

    private const string WARNING_PHRASE = "(를)을\n설치하시겠습니까?";

    private void Awake()
    {

    }
    private void Start()
    {
        YesButton.onClick.AddListener(() =>
        {
            GridBuildingSystem.Instance.InitializeWithBuilding(CurBuilding);
            CurBuilding.GetComponent<Building>().IsDeploying = true;
            gameObject.SetActive(false);
        });

        NoButton.onClick.AddListener(() =>
        {
            BuildingInstallationUI.SetActive(true);
            gameObject.SetActive(false);
        });
    }

    public void SetWarningData(GameObject building, string buildingname, GameObject buildingInstalltionUI)
    {
        CurBuilding = building;
        WarningText.text = $"{buildingname}{WARNING_PHRASE}";

        BuildingInstallationUI = buildingInstalltionUI;
    }
}

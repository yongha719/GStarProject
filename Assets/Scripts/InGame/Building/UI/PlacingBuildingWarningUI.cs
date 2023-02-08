using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlacingBuildingWarningUI : Warning
{
    private GameObject CurBuilding;

    // 건물 설치 UI
    private GameObject BuildingInstallationUI;

    private const string WARNING_PHRASE = "(를)을\n설치하시겠습니까?";

    private void Start()
    {
        BuildingInstallationUI = UIPopUpHandler.Instance.BuildingPlacingPopup;

        YesButton.onClick.AddListener(() =>
        {
            GridBuildingSystem.Instance.InitializeWithBuilding(CurBuilding);
            CurBuilding.GetComponent<Building>().IsDeploying = true;
            BuildingInstallationUI.SetActive(false);
            gameObject.SetActive(false);
        });

        NoButton.onClick.AddListener(() =>
        {
            BuildingInstallationUI.SetActive(true);
            gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 경고창에 띄울 건물 정보
    /// </summary>
    /// <param name="building">설치하려고 하는 건물</param>
    /// <param name="buildingname">설치하려고 하는 건물 이름</param>
    /// <param name="buildingInstalltionUI"></param>
    public void SetWarningData(GameObject building, string buildingname)
    {
        CurBuilding = building;
        WarningText.text = $"{buildingname}{WARNING_PHRASE}";
    }
}

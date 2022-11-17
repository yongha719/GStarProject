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
    [SerializeField] private List<BuyBuildingInfo> GoldProductionBuildingInfos;
    [SerializeField] private List<BuyBuildingInfo> EnergyProductionBuildingInfos;

    [Header("Warning UIs")]
    [SerializeField] private BuildingInstallWarning Warning;
    [SerializeField] private NotEnoughGold NotEnoughGold;



    #endregion

    #region Category

         

    #endregion

    #region UI Object

    [SerializeField] private GameObject BuildingInstallation;
    [SerializeField] private CatPlacement CatPlacement;
    [SerializeField] private BuildingInfomation BuildingInfomation;
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

        //SetResolution();
    }

    private void SetResolution()
    {
        int setWidth = 1440; // 사용자 설정 너비
        int setHeight = 2960; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
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
                //Building.BuildingInfomation = BuildingInfomation;

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
                //Building.BuildingInfomation = BuildingInfomation;

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

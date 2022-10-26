using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EnergyProductionBuilding : Building
{
    [Header("Energy Production Building")]
    public EnergyBuildingType buildingType;

    #region Energy
    [Header("Energy")]
    [SerializeField] private Button CollectEnergyButton;
    [SerializeField] private string DefaultEnergy;
    [SerializeField] private float DefaultEnergyChargingTime;
    [SerializeField] private float IncreasePerLevelUp;
    [Tooltip("회복하기까지 필요한 생산량")]
    [SerializeField] private int ProductionsNeededToRecover;

    public string ProductionGold
    {
        get
        {
            var energy = DefaultEnergy.returnValue();

            for (var i = 0; i < Rating - 1; i++)
            {
                energy += energy * Math.Round((double)(IncreasePerLevelUp / 100f), 3);
            }

            return energy.returnStr();
        }
    }


    public override string ConstructionCost
    {
        get
        {
            if (BuildingManager.s_EnergyBuildings[buildingType] == 0)
                return DefaultConstructionCost;
            return (DefaultConstructionCost.returnValue() * (BuildingManager.s_EnergyBuildings[buildingType] * 3)).returnStr();
        }
    }


    private bool didGetEnergy;

    private WaitForSeconds waitEnergyChargingTime;
    private const float AUTO_GET_ENERGY_DELAY = 10f;

    [SerializeField] private TextMeshProUGUI ConstructionGoldText;

    #endregion

    [SerializeField] private Button BuildingButton;
    [SerializeField] private GameObject BuildingUI;
}

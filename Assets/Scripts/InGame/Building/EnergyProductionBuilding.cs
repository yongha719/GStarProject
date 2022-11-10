using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class EnergyProductionBuilding : Building, IResourceProductionBuilding
{
    [Header("Energy Production Building")]
    public EnergyBuildingType buildingType;

    #region Energy
    [Header("Energy")]
    [SerializeField] private Button CollectEnergyButton;
    [SerializeField] private string DefaultEnergy;
    [SerializeField] private float DefaultEnergyChargingTime;
    [SerializeField] private float IncreasePerLevelUp;
    [SerializeField] private GameObject EnergyAcquisitionEffect;

    [Tooltip("회복하기까지 필요한 생산량")]
    [SerializeField] private int ProductionsNeededToRecover;

    private List<Cat> PlacedInBuildingCat;

    private bool isProducting;

    // 생산 에너지
    public string ProductionEnergy
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

    // 건설 비용
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

    [SerializeField] private GameObject BuildingUI;
    [SerializeField] private Button CatPlacementButton;

    protected override void Start()
    {
        base.Start();

        CatPlacementButton?.onClick.AddListener(() =>
        {
            CatPlacement.gameObject.SetActive(true);

            if (PlacedInBuildingCat.Count == 0)
            {
                CatPlacement.SetBuildingInfo(BuildingType.Gold, this, null, SpriteRenderer.sprite);
            }
            else
            {
                var cats = PlacedInBuildingCat.Where(x => x.catData != null).Select(x => x.catData).ToArray();
                CatPlacement.SetBuildingInfo(BuildingType.Gold, this, cats, SpriteRenderer.sprite);
            }

        });

    }

    protected override IEnumerator BuildingInstalltionEffect()
    {
        return base.BuildingInstalltionEffect();


    }

    public IEnumerator ResourceProduction
    {
        get
        {
            while (true)
            {
                yield return waitEnergyChargingTime;

                CollectEnergyButton.gameObject.SetActive(true);

                for (int i = 0; i < MaxDeployableCat; i++)
                {
                    if (PlacedInBuildingCat[i] == null)
                        continue;

                    // 에너지 생산 10번하면 쉬러 가야 함
                    if (PlacedInBuildingCat[i].NumberOfGoldProduction++ >= 3)
                    {
                        PlacedInBuildingCat[i].GoToRest();
                    }
                }

                yield return StartCoroutine(WaitGetResource());
            }
        }
    }

    public IEnumerator WaitGetResource()
    {
        var autogetenergy = false;

        while (true)
        {
            if (didGetEnergy)
            {
                GameManager.Instance._coin += autogetenergy ? ProductionEnergy.returnValue() : ProductionEnergy.returnValue() * 0.5f;
                didGetEnergy = false;
                CollectEnergyButton.gameObject.SetActive(false);

                // 골드 획득 연출
                Instantiate(EnergyAcquisitionEffect, transform.position, Quaternion.identity);

                yield break;
            }

            yield return null;
        }
    }

    public void OnCatMemberChange(CatData catData,int index, Action action)
    {
        PlacedInBuildingCat.Add(catData.Cat);

        int decreasingfigure = 0;

        for (int i = 0; i < MaxDeployableCat; i++)
        {
            if (PlacedInBuildingCat[i] != null)
            {
                if ((int)buildingType == (int)PlacedInBuildingCat[i].catData.GoldAbilityType)
                {
                    decreasingfigure += PlacedInBuildingCat[i].PercentageReductionbyRating;
                }
            }
        }

        if (decreasingfigure != 0)
        {
            waitEnergyChargingTime = new WaitForSeconds((float)(DefaultEnergyChargingTime * Math.Round(decreasingfigure / 100f, 3)));
            StartCoroutine(ResourceProduction);
        }
        else
        {
            StopCoroutine(ResourceProduction);
        }

        action?.Invoke();
    }

    private void OnMouseDown()
    {
        if (isDeploying && EventSystem.current.IsPointerOverGameObject())
            return;

        if (CollectEnergyButton.gameObject.activeSelf)
        {
            didGetEnergy = true;
        }
        else if (BuildingUI.activeSelf)
        {
            BuildingUI.SetActive(false);
        }
        else
        {
            BuildingUI.SetActive(true);
        }
    }
}

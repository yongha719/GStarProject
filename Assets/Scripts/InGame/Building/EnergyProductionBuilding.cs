using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using DG.Tweening;

public class EnergyProductionBuilding : Building, IResourceProductionBuilding
{
    public override bool IsDeploying
    {
        get
        {
            return isDeploying;
        }

        set
        {
            isDeploying = value;

            if (isDeploying)
            {
                SpriteRenderer.color = new Color(1, 1, 1, 0.5f);
                SpriteRenderer.sortingOrder = 3;
                DeployingUIParent.SetActive(true);
                CollectEnergyButton.gameObject.SetActive(false);
            }
            else
            {
                SpriteRenderer.color = Color.white;
                SpriteRenderer.sortingOrder = 0;
                DeployingUIParent.SetActive(false);
            }
        }
    }

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
            if (BuildingManager.s_EnergyBuildingCount[buildingType] == 0)
                return DefaultConstructionCost;
            return (DefaultConstructionCost.returnValue() * (BuildingManager.s_EnergyBuildingCount[buildingType] * 3)).returnStr();
        }
    }


    private bool didGetEnergy;

    private WaitForSeconds waitEnergyChargingTime;
    private const float AUTO_GET_ENERGY_DELAY = 10f;

    [SerializeField] private TextMeshProUGUI ConstructionGoldText;
    [SerializeField] private GameObject EnergyAcquisitionEffect;

    #endregion

    [SerializeField] private GameObject BuildingUI;
    [SerializeField] private Button CatPlacementButton;

    private List<Cat> PlacedInBuildingCat = new List<Cat>();
    public CatPlacementWorkingCats WorkingCats;


    protected override void Start()
    {
        base.Start();

        waitEnergyChargingTime = new WaitForSeconds(DefaultEnergyChargingTime);

        CollectEnergyButton.onClick.AddListener(() =>
        {
            didGetEnergy = true;
        });

        CatPlacementButton?.onClick.AddListener(() =>
        {
            CatPlacement.gameObject.SetActive(true);

            PlacedInBuildingCat = PlacedInBuildingCat.Where(x => (object)x.building == this).ToList();

            if (PlacedInBuildingCat.Count == 0)
            {
                CatPlacement.SetBuildingInfo(BuildingType.Energy, this, null, WorkingCats, SpriteRenderer.sprite);
            }
            else
            {
                var cats = PlacedInBuildingCat.Where(x => x.catData != null).Select(x => x.catData).ToArray();
                CatPlacement.SetBuildingInfo(BuildingType.Energy, this, cats, WorkingCats, SpriteRenderer.sprite);
            }
        });

    }

    public void OnCatMemberChange(CatData catData, int index = 0, Action action = null)
    {
        PlacedInBuildingCat.Add(catData.Cat);
        SetProductionTime();

        action?.Invoke();
    }

    /// <summary>
    /// 생산 시간 재설정
    /// </summary>
    private void SetProductionTime()
    {
        int decreasingfigure = 0;

        for (int i = 0; i < PlacedInBuildingCat.Count; i++)
        {
            if (PlacedInBuildingCat[i] != null)
            {
                // 능력이 건물의 종류와 같을 때
                if ((int)buildingType == (int)PlacedInBuildingCat[i].catData.GoldAbilityType)
                {
                    decreasingfigure += PlacedInBuildingCat[i].PercentageReductionbyRating;
                }
            }
        }

        if (PlacedInBuildingCat.Count != 0)
        {
            if (decreasingfigure != 0)
                waitEnergyChargingTime = new WaitForSeconds((float)(DefaultEnergyChargingTime * Math.Round(decreasingfigure / 100f, 3)));
        }
    }

    public override void Place()
    {
        base.Place();

        StartCoroutine(ResourceProduction());
    }

    public IEnumerator ResourceProduction()
    {
        while (true)
        {
            if (PlacedInBuildingCat.Count == 0)
            {
                yield return null;
                continue;
            }

            CollectEnergyButton.gameObject.SetActive(true);
            yield return StartCoroutine(WaitGetResource());

            for (int i = 0; i < MaxDeployableCat; i++)
            {
                if (PlacedInBuildingCat[i] == null)
                    continue;

                // 에너지 생산 3번하면 일하러 가야함
                if (++PlacedInBuildingCat[i].NumberOfEnergyProduction >= 3)
                {
                    PlacedInBuildingCat[i].NumberOfEnergyProduction = 0;
                    PlacedInBuildingCat[i].GoWorking = true;
                    PlacedInBuildingCat[i].GoResting = false;
                    PlacedInBuildingCat[i].IsResting= false;
                    PlacedInBuildingCat[i].GoToWork(PlacedInBuildingCat[i].building.transform.position);

                    PlacedInBuildingCat.RemoveAt(i);
                }
            }

            yield return waitEnergyChargingTime;
        }
    }

    public IEnumerator WaitGetResource()
    {
        var curtime = 0f;
        var autogetenergy = false;

        while (true)
        {
            if (didGetEnergy)
            {
                GameManager.Instance._energy += autogetenergy ? ProductionEnergy.returnValue() : ProductionEnergy.returnValue() * 0.5f;
                didGetEnergy = false;
                CollectEnergyButton.gameObject.SetActive(false);

                // 골드 획득 연출
                Destroy(Instantiate(EnergyAcquisitionEffect, transform.position + (Vector3.up * 0.5f), Quaternion.identity, CanvasRt), 1.5f);

                yield break;
            }

            curtime += Time.deltaTime;

            if (curtime >= AUTO_GET_ENERGY_DELAY)
            {
                curtime = 0;

                autogetenergy = true;
                didGetEnergy = true;
            }

            yield return null;
        }
    }

    protected override IEnumerator BuildingInstalltionEffect()
    {
        yield return base.BuildingInstalltionEffect();

        ConstructionGoldText.gameObject.SetActive(true);
        ConstructionGoldText.text = ConstructionCost;
        ConstructionGoldText.rectTransform.DOAnchorPosY(ConstructionGoldText.rectTransform.anchoredPosition.y + 150, 1);
        yield return ConstructionGoldText.DOFade(0f, 0.7f).WaitForCompletion();

        ConstructionGoldText.gameObject.SetActive(false);

        BuildingManager.s_EnergyBuildingCount[buildingType]++;
        BuildingManager.s_EnergyProductionBuildings.Add(this);

        GridBuildingSystem.InitializeWithBuilding(BuildingInfo.BuildingPrefab);

    }

    public bool CanDeploy()
    {
        return PlacedInBuildingCat.Count < MaxDeployableCat;
    }

    private void OnMouseDown()
    {
        if (isDeploying || IsPointerOverGameObject())
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

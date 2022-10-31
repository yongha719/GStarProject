using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GoldProductionBuilding : Building, IResourceProductionBuilding
{
    [Header("Gold Production Building")]
    public GoldBuildingType buildingType;

    #region Gold
    [Header("Gold")]
    [SerializeField] private Button CollectMoneyButton;
    [SerializeField] private string DefaultGold;
    [SerializeField] private float DefaultGoldChargingTime;
    [SerializeField] private float IncreasePerLevelUp;

    public string ProductionGold
    {
        get
        {
            var gold = DefaultGold.returnValue();

            for (var i = 0; i < Rating - 1; i++)
            {
                gold += gold * Math.Round((double)(IncreasePerLevelUp / 100f), 3);
            }

            return gold.returnStr();
        }
    }


    public override string ConstructionCost
    {
        get
        {
            if (BuildingManager.s_GoldBuildings[buildingType] == 0)
                return DefaultConstructionCost;
            return (DefaultConstructionCost.returnValue() * (BuildingManager.s_GoldBuildings[buildingType] * 3)).returnStr();
        }
    }


    private bool didGetMoney;

    private WaitForSeconds waitGoldChargingTime;
    private const float AUTO_GET_GOLD_DELAY = 20f;

    [SerializeField] private TextMeshProUGUI ConstructionGoldText;

    #endregion

    [SerializeField] private Button BuildingButton;
    [SerializeField] private GameObject BuildingUI;

    // Coin먹는 연출
    [SerializeField] private GameObject CoinEffect;

    // 건물에 배치된 고양이
    private Cat[] PlacedInBuildingCat;
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
                DeployingUIParent.SetActive(true);
                CollectMoneyButton.gameObject.SetActive(false);
            }
            else
            {
                SpriteRenderer.color = Color.white;
                DeployingUIParent.SetActive(false);
                CollectMoneyButton.gameObject.SetActive(true);

            }
        }
    }

    private void Awake()
    {

    }


    protected override void Start()
    {
        base.Start();

        PlacedInBuildingCat = new Cat[MaxDeployableCat];

        waitGoldChargingTime = new WaitForSeconds(DefaultGoldChargingTime);

        CollectMoneyButton.onClick.AddListener(() =>
        {
            didGetMoney = true;
        });

        BuildingButton?.onClick.AddListener(() =>
        {
            if (CollectMoneyButton.gameObject.activeSelf)
            {
                didGetMoney = true;
            }
            else if (BuildingUI.activeSelf)
            {
                BuildingUI.SetActive(false);
            }
            else
            {
                BuildingUI.SetActive(true);
            }

        });
    }

    // TODO : 너무 긴 거 같음 추후 리팩토링
    public void OnCatMemberChange(Action action)
    {
        // 생산 시간 감소 수치
        int decreasingfigure = 0;

        for (int i = 0; i < MaxDeployableCat; i++)
        {
            if (PlacedInBuildingCat[i] != null)
            {
                if ((int)buildingType == (int)PlacedInBuildingCat[i].catData.GoldAbilityType)
                {
                    decreasingfigure += PlacedInBuildingCat[i].ReductionTimebyGrade;
                }
            }
        }

        double productiondelay = 0;

        if (decreasingfigure != 0)
        {
            productiondelay = DefaultGoldChargingTime * Math.Round(decreasingfigure / 100f, 3);
        }
        else
        {
            productiondelay = DefaultGoldChargingTime;
        }

        waitGoldChargingTime = new WaitForSeconds((float)productiondelay);

        action?.Invoke();
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
            yield return waitGoldChargingTime;

            CollectMoneyButton.gameObject.SetActive(true);

            for(int i = 0; i < MaxDeployableCat; i++)
            {
                if (PlacedInBuildingCat[i] == null)
                    continue;

                //PlacedInBuildingCat[i].Status++;

                // 골드 생산 10번하면 쉬러 가야 함
                //if (PlacedInBuildingCat[i].Status >= 10)
                //{
                //    PlacedInBuildingCat[i].GoToRest();
                //}
            }

            yield return StartCoroutine(WaitGetResource());
        }
    }

    public IEnumerator WaitGetResource()
    {
        var curtime = 0f;
        var autogetmoney = false;

        while (true)
        {
            if (didGetMoney)
            {
                GameManager.Instance._coin += autogetmoney ? ProductionGold.returnValue() : ProductionGold.returnValue() * 0.5f;
                didGetMoney = false;
                CollectMoneyButton.gameObject.SetActive(false);

                // 골드 획득 연출
                Instantiate(CoinEffect, transform.position, Quaternion.identity);
                                
                yield break;
            }

            curtime += Time.deltaTime;

            if (curtime >= AUTO_GET_GOLD_DELAY)
            {
                autogetmoney = true;
                didGetMoney = true;
            }

            yield return null;
        }
    }


    protected override IEnumerator BuildingInstalltionEffect()
    {
        yield return base.BuildingInstalltionEffect();

        ConstructionGoldText.gameObject.SetActive(true);
        ConstructionGoldText.rectTransform.DOAnchorPosY(ConstructionGoldText.rectTransform.anchoredPosition.y + 150, 1);
        yield return ConstructionGoldText.DOFade(0f, 0.7f).WaitForCompletion();

        ConstructionGoldText.gameObject.SetActive(false);

        GridBuildingSystem.InitializeWithBuilding(BuildingInfo.BuildingPrefab);
        BuildingManager.s_GoldBuildings[buildingType]++;
    }
}

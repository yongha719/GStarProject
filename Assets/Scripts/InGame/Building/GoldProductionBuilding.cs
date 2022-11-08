using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Linq;
using UnityEngine.EventSystems;

public class GoldProductionBuilding : Building, IResourceProductionBuilding
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
                CollectMoneyButton.gameObject.SetActive(false);
            }
            else
            {
                SpriteRenderer.color = Color.white;
                SpriteRenderer.sortingOrder = 0;
                DeployingUIParent.SetActive(false);
            }
        }
    }

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
    [SerializeField] private GameObject GoldAcquisitionEffect;

    [Space(10)]
    #endregion

    [SerializeField] private GameObject BuildingUI;

    [SerializeField] private Button CatPlacementButton;

    // 건물에 배치된 고양이
    private List<Cat> PlacedInBuildingCat = new List<Cat>();


    private void Awake()
    {
        //CatPlacement = FindObjectOfType<CatPlacement>();
    }

    protected override void Start()
    {
        base.Start();


        waitGoldChargingTime = new WaitForSeconds(DefaultGoldChargingTime);

        CollectMoneyButton.onClick.AddListener(() =>
        {
            didGetMoney = true;
        });

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
                print(cats.Length);
                CatPlacement.SetBuildingInfo(BuildingType.Gold, this, cats, SpriteRenderer.sprite);
            }

        });
    }

    // TODO : 너무 긴 거 같음 추후 리팩토링
    public void OnCatMemberChange(CatData catData, Action action)
    {
        catData.Cat.GoToWork();

        PlacedInBuildingCat.Add(catData.Cat);

        // 생산 시간 감소 수치
        int decreasingfigure = 0;

        for (int i = 0; i < MaxDeployableCat; i++)
        {
            if (PlacedInBuildingCat[i] != null)
            {
                // 능력이 건물의 종류와 같을 때
                if ((int)buildingType == (int)PlacedInBuildingCat[i].catData.GoldAbilityType)
                {
                    decreasingfigure += PlacedInBuildingCat[i].PercentageReductionbyGrade;
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

        StartCoroutine(ResourceProduction);
    }

    public IEnumerator ResourceProduction
    {
        get
        {
            while (true)
            {
                if (PlacedInBuildingCat.Count == 0)
                {
                    yield return null;
                    continue;
                }

                yield return StartCoroutine(WaitGetResource());


                for (int i = 0; i < MaxDeployableCat; i++)
                {
                    if (PlacedInBuildingCat[i] == null)
                        continue;

                    // 골드 생산 10번하면 쉬러 가야 함
                    if (PlacedInBuildingCat[i].NumberOfGoldProduction++ >= 10)
                    {
                        PlacedInBuildingCat[i].GoToRest();
                    }
                }

                yield return waitGoldChargingTime;

                CollectMoneyButton.gameObject.SetActive(true);
            }
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
                print("coin effect");
                Destroy(Instantiate(GoldAcquisitionEffect, transform.position + (Vector3.up * 0.5f), Quaternion.identity, CanvasRt), 1.5f);

                yield break;
            }

            curtime += Time.deltaTime;

            if (curtime >= AUTO_GET_GOLD_DELAY)
            {
                curtime = 0;

                autogetmoney = true;
                didGetMoney = true;
                print("auto get money");
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

    private void OnMouseDown()
    {
        if (IsDeploying && EventSystem.current.IsPointerOverGameObject())
            return;

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
    }
}

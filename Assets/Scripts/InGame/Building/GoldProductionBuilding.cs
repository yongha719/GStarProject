using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Linq;

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
            if (BuildingManager.s_GoldBuildingCount[buildingType] == 0)
                return DefaultConstructionCost;
            return (DefaultConstructionCost.returnValue() * (BuildingManager.s_GoldBuildingCount[buildingType] * 3)).returnStr();
        }
    }

    [Header("Level Up")]
    [SerializeField] private string DefaultLevelUpCost;

    public string LevelUpCost(int level)
    {
        var cost = DefaultLevelUpCost.returnValue();

        for (int i = 0; i < level - 1; i++)
        {
            cost += cost * Math.Round((double)(8f / 100f), 3);
        }

        return cost.returnStr();
    }

    private bool didGetMoney;

    private WaitForSeconds waitGoldChargingTime;
    private const float AUTO_GET_GOLD_DELAY = 20f;

    [Space]
    [SerializeField] private TextMeshProUGUI ConstructionGoldText;
    [SerializeField] private GameObject GoldAcquisitionEffect;

    [Space(10)]
    #endregion

    [SerializeField] private GameObject BuildingUI;

    [SerializeField] private Button CatPlacementButton;
    [SerializeField] private Button BuildingInfomationButton;

    private bool IsClicked;
    private static GameObject s_buildingUI;

    // 건물에 배치된 고양이
    private List<Cat> PlacedInBuildingCats = new List<Cat>();
    public CatPlacementWorkingCats WorkingCats;

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

            PlacedInBuildingCats = PlacedInBuildingCats.Where(x => (object)x.building == this).ToList();

            if (PlacedInBuildingCats.Count == 0)
            {
                CatPlacement.SetBuildingInfo(BuildingType.Gold, this, null, WorkingCats, SpriteRenderer.sprite);
            }
            else
            {
                var cats = PlacedInBuildingCats.Where(x => x.catData != null).Select(x => x.catData).ToArray();
                CatPlacement.SetBuildingInfo(BuildingType.Gold, this, cats, WorkingCats, SpriteRenderer.sprite);
            }
        });

        BuildingInfomationButton.onClick.AddListener(() =>
        {
            BuildingInfomation.gameObject.SetActive(true);

            PlacedInBuildingCats = PlacedInBuildingCats.Where(x => (object)x.building == this).ToList();

            if (PlacedInBuildingCats.Count == 0)
            {
                BuildingInfomation.SetData(BuildingType.Gold, this, null, SpriteRenderer.sprite);
            }
            else
            {
                BuildingInfomation.SetData(BuildingType.Gold, this, PlacedInBuildingCats, SpriteRenderer.sprite);
            }
        });
    }

    void Update()
    {
    }

    public void OnCatMemberChange(CatData catData, int index, Action action = null)
    {
        if (PlacedInBuildingCats.Count < MaxDeployableCat)
        {
            PlacedInBuildingCats.Add(catData.Cat);
        }
        else
        {
            var cat = PlacedInBuildingCats[index];
            cat.RandomMoveCoroutine = StartCoroutine(PlacedInBuildingCats[index].RandomMove());
            cat.GoWorking = true;
            if (cat.IsWorking)
            {
                cat.IsWorking = false;
            }
            cat = catData.Cat;
        }

        SetProductionTime();

        action?.Invoke();
    }


    /// <summary>
    /// 생산 시간 재설정
    /// </summary>
    private void SetProductionTime()
    {
        int decreasingfigure = 0;

        for (int i = 0; i < PlacedInBuildingCats.Count; i++)
        {
            if (PlacedInBuildingCats[i] != null)
            {
                // 능력이 건물의 종류와 같을 때
                if ((int)buildingType == (int)PlacedInBuildingCats[i].catData.GoldAbilityType)
                {
                    decreasingfigure += PlacedInBuildingCats[i].PercentageReductionbyRating;
                }
            }
        }

        if (PlacedInBuildingCats.Count != 0)
        {
            if (decreasingfigure != 0)
                waitGoldChargingTime = new WaitForSeconds((float)(DefaultGoldChargingTime * Math.Round(decreasingfigure / 100f, 3)));
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
            if (PlacedInBuildingCats.Count == 0)
            {
                yield return null;
                continue;
            }

            CollectMoneyButton.gameObject.SetActive(true);
            yield return StartCoroutine(WaitGetResource());

            for (int i = 0; i < PlacedInBuildingCats.Count; i++)
            {
                if (PlacedInBuildingCats[i] == null)
                    continue;

                print(BuildingManager.s_EnergyProductionBuildings.Count);
                // 골드 생산 10번하면 쉬러 가야 함
                if (++PlacedInBuildingCats[i].NumberOfGoldProduction >= 10 && BuildingManager.CanGoRest(out Vector3 buildingPos))
                {
                    print("rest");
                    PlacedInBuildingCats[i].NumberOfGoldProduction = 0;
                    PlacedInBuildingCats[i].GoWorking = false;
                    PlacedInBuildingCats[i].GoResting = true;

                    // 랜덤한 에너지 생산 건물 위치로 가기
                    PlacedInBuildingCats[i].GoToRest(buildingPos);
                    PlacedInBuildingCats.RemoveAt(i);
                }
            }

            yield return waitGoldChargingTime;
        }
    }

    public IEnumerator WaitGetResource()
    {
        print(nameof(WaitGetResource));

        var curtime = 0f;
        var autogetmoney = false;

        while (true)
        {
            if (didGetMoney)
            {
                GameManager.Instance._coin += autogetmoney ? ProductionGold.returnValue() : ProductionGold.returnValue() * 0.5f;
                didGetMoney = false;
                CollectMoneyButton.gameObject.SetActive(false);

                SoundManager.Instance.PlaySoundClip("SFX_Goods", SoundType.SFX);
                DailyQuestManager.dailyQuests.quests[(int)QuestType.Gold]._index++;
                // 골드 획득 연출
                Destroy(Instantiate(GoldAcquisitionEffect, transform.position + (Vector3.up * 0.5f), Quaternion.identity, CanvasRt), 1.5f);

                yield break;
            }

            curtime += Time.deltaTime;

            if (curtime >= AUTO_GET_GOLD_DELAY)
            {
                curtime = 0;

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
        ConstructionGoldText.text = ConstructionCost;
        ConstructionGoldText.rectTransform.DOAnchorPosY(ConstructionGoldText.rectTransform.anchoredPosition.y + 150, 1);
        yield return ConstructionGoldText.DOFade(0f, 0.7f).WaitForCompletion();

        ConstructionGoldText.gameObject.SetActive(false);

        BuildingManager.s_GoldBuildingCount[buildingType]++;
        BuildingManager.s_GoldProductionBuildings.Add(this);

        GridBuildingSystem.InitializeWithBuilding(BuildingInfo.BuildingPrefab);
    }

    private void OnMouseDown()
    {
        if (isDeploying || IsPointerOverGameObject())
            return;

        if (CollectMoneyButton.gameObject.activeSelf)
        {
            didGetMoney = true;
        }
        else if (BuildingUI.activeSelf)
        {
            BuildingUI.SetActive(false);
            s_buildingUI = BuildingUI;
        }
        else
        {
            if (s_buildingUI != null)
            {
                s_buildingUI.SetActive(false);
            }
            s_buildingUI = BuildingUI;
            BuildingUI.SetActive(true);
        }
    }
}

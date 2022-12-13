using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class GoldProductionBuilding : ProductionBuilding
{
    private const float AUTO_GET_GOLD_DELAY = 20f;
    public override int BuildinTypeToInt => (int)buildingType;

    [Header("Gold Production Building")]
    public GoldBuildingType buildingType;

    public override string ConstructionCost
    {
        get
        {
            if (BuildingManager.s_GoldBuildingCount[buildingType] == 0)
                return DefaultConstructionCost;
            return (DefaultConstructionCost.returnValue() * (BuildingManager.s_GoldBuildingCount[buildingType] * 3)).returnStr();
        }
    }

    [SerializeField] private UnityEngine.UI.Button CatPlacementButton;

    protected override void Start()
    {
        base.Start();


        CatPlacementButton?.onClick.AddListener(() =>
        {
            CatPlacement.gameObject.SetActive(true);

            PlacedInBuildingCats = PlacedInBuildingCats.Where(x => x.building == this).ToList();

            if (PlacedInBuildingCats.Count == 0)
            {
                CatPlacement.SetBuildingInfo(BuildingType.Gold, this, null);
            }
            else
            {
                var cats = PlacedInBuildingCats.Where(x => x.catData != null).Select(x => x.catData).ToArray();
                CatPlacement.SetBuildingInfo(BuildingType.Gold, this, cats);
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


    protected virtual void OnCatMemberChange(int index) { }

    public void OnCatMemberChange(CatData catData, int index)
    {
        CatPlacementWorkingCats workingCats = catData.Cat.building.WorkingCats;

        if (PlacedInBuildingCats.Count < MaxDeployableCat)
        {
            PlacedInBuildingCats.Add(catData.Cat);
        }
        else
        {
            switch (buildingType)
            {
                case GoldBuildingType.GoldMine:
                case GoldBuildingType.PotatoFarming:
                case GoldBuildingType.PowerPlant:
                    PlacedInBuildingCats[index].gameObject.SetActive(true);
                    break;
            }

            PlacedInBuildingCats[index].FinishWork();
            PlacedInBuildingCats[index] = catData.Cat;
        }

        SetProductionTime();

        catData.Cat.catNum = index;

        var catBuilding = catData.Cat.building;

        if (catBuilding != null)
        {
            if (catBuilding.PlacedInBuildingCats.Contains(catData.Cat))
            {
                catBuilding.PlacedInBuildingCats.Remove(catData.Cat);
                workingCats.CatDatas.Remove(catData);
            }

            if (catBuilding.WorkingCats.CatDatas.Contains(catData))
            {
                catBuilding.WorkingCats.RemoveCat(catData);
            }
        }

        catBuilding = this;
        catData.Cat.GoToWork(transform.position);

        OnCatMemberChange(index);
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
                waitResourceChargingTime = new WaitForSeconds(DefaultResourceChargingTime - (float)(DefaultResourceChargingTime * Math.Round(decreasingfigure / 100f, 3)));
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

            CollectResourceButton.gameObject.SetActive(true);
            yield return StartCoroutine(WaitGetResource());

            for (int i = 0; i < PlacedInBuildingCats.Count; i++)
            {
                if (PlacedInBuildingCats[i] == null)
                    continue;

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

            yield return waitResourceChargingTime;
        }
    }

    public IEnumerator WaitGetResource()
    {
        print(nameof(WaitGetResource));

        var curtime = 0f;
        var autogetmoney = false;

        while (true)
        {
            if (didGetResource)
            {
                GameManager.Instance._coin += autogetmoney ? ProductionResource.returnValue() : ProductionResource.returnValue() * 0.5f;
                didGetResource = false;
                CollectResourceButton.gameObject.SetActive(false);

                SoundManager.Instance.PlaySoundClip("SFX_Goods", SoundType.SFX);
                //DailyQuestManager.dailyQuests.quests[(int)QuestType.Gold]._index++;
                // 골드 획득 연출
                Destroy(Instantiate(ResourceAcquisitionEffect, transform.position + (Vector3.up * 0.5f), Quaternion.identity, CanvasRt), 1.5f);

                yield break;
            }

            curtime += Time.deltaTime;

            if (curtime >= AUTO_GET_GOLD_DELAY)
            {
                curtime = 0;

                autogetmoney = true;
                didGetResource = true;
            }

            yield return null;
        }
    }


    protected override IEnumerator BuildingInstalltionEffect()
    {
        yield return base.BuildingInstalltionEffect();

        ConstructionResourceText.gameObject.SetActive(true);
        ConstructionResourceText.text = ConstructionCost;
        ConstructionResourceText.rectTransform.DOAnchorPosY(ConstructionResourceText.rectTransform.anchoredPosition.y + 150, 1);
        yield return ConstructionResourceText.DOFade(0f, 0.7f).WaitForCompletion();

        ConstructionResourceText.gameObject.SetActive(false);

        BuildingManager.s_GoldBuildingCount[buildingType]++;
        BuildingManager.s_GoldProductionBuildings.Add(this);

        GridBuildingSystem.InitializeWithBuilding(BuildingInfo.BuildingPrefab);
    }

    public void SetPos(int index = 0)
    {
        var cat = PlacedInBuildingCats[index];

        print(buildingType);
        switch (buildingType)
        {
            case GoldBuildingType.IceFishing:
                if (cat.catData.CatSkinType == CatSkinType.beanieCat)
                {
                    cat.transform.position = transform.position + new Vector3(0.15f, 1.2f, 0);
                }
                else if (cat.catData.CatSkinType == CatSkinType.PinkCloakCat)
                {

                }
                else
                    cat.transform.position = transform.position + new Vector3(0.07f, 1.2f, 0);
                break;
            case GoldBuildingType.FirewoodChopping:
                cat.transform.position = transform.position + new Vector3(0.13f, 0.8f, 0);
                break;
            case GoldBuildingType.BlastFurnace:
                cat.transform.position = transform.position + new Vector3(0.4f, 0.7f, 0);
                break;
            case GoldBuildingType.WinterClothesWorkshop:
                if (index == 0)
                {
                    PlacedInBuildingCats[0].transform.position = transform.position + new Vector3(0.4f, 0.8f);
                    PlacedInBuildingCats[0].Animator.SetInteger("Clothes", index);
                }
                else
                {
                    PlacedInBuildingCats[1].transform.position = transform.position + new Vector3(-0.24f, 0.8f);
                    PlacedInBuildingCats[1].Animator.SetInteger("Clothes", index);
                }
                break;
            case GoldBuildingType.Cauldron:
                PlacedInBuildingCats[0].transform.position = transform.position + new Vector3(-0.026f, 0.86f);
                break;
            case GoldBuildingType.GoldMine:
            case GoldBuildingType.PotatoFarming:
            case GoldBuildingType.PowerPlant:
                cat.gameObject.SetActive(false);
                break;
            case GoldBuildingType.End:
                break;
        }
    }
}

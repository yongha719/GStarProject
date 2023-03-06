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

    public override string PlacingPrice
    {
        get
        {
            if (BuildingManager.s_GoldBuildingCount[buildingType] == 0)
                return DefaultPlacingPrice;
            return (DefaultPlacingPrice.returnValue() * (BuildingManager.s_GoldBuildingCount[buildingType] * 3)).returnStr();
        }
    }

    [SerializeField] private UnityEngine.UI.Button CatPlacementButton;

    protected override void Start()
    {
        base.Start();

        BuildingLevelUpUI = UIPopUpHandler.Instance.BuildingLevelUpPopup.GetComponent<BuildingLevelUpUI>();

        CatPlacementButton.onClick.AddListener(() =>
        {
            CatPlacement.OpenUIPopup();

            PlacedInBuildingCats = PlacedInBuildingCats.Where(x => x.goldBuilding == this).ToList();

            if (PlacedInBuildingCats.Count == 0)
            {
                CatPlacement.SetBuildingInfo(BuildingType.Gold, this);
            }
            else
            {
                var cats = PlacedInBuildingCats.Where(x => x.catData != null).Select(x => x.catData).ToArray();
                CatPlacement.SetBuildingInfo(BuildingType.Gold, this, cats);
            }
        });

        BuildingLevelUpButton.onClick.AddListener(() =>
        {
            print("dada");
            BuildingLevelUpUI.OpenUIPopup();

            PlacedInBuildingCats = PlacedInBuildingCats.Where(x => x.goldBuilding == this).ToList();

            BuildingLevelUpUI.SetData(this);
        });
    }


    protected virtual void ChangeCat(int index) { }

    //
    public virtual void ChangeCat(CatData catData, int index)
    {
        // 건물에 고양이를 추가하기만 하면 됨
        if (PlacedInBuildingCats.Count < MaxDeployableCat)
        {
            PlacedInBuildingCats.Add(catData.Cat);
        }
        // 고양이를 바꿔줘야함
        else
        {
            // 얘네는 건물에서 일할때 모션이 없고 들어가서 일하는 컨셉이라
            // 꺼져있기 때문에 다시 켜줘야 함
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

        if (PlacedInBuildingCats.Count != 0)
        {
            SetProductionTime();
        }

        catData.Cat.catNum = index;

        catData.Cat.GoToWork(transform.position);
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

        if (decreasingfigure != 0)
            waitResourceChargingTime = new WaitForSeconds(DefaultResourceChargingTime - (float)(DefaultResourceChargingTime * Math.Round(decreasingfigure / 100f, 3)));
    }

    public override void Place()
    {
        base.Place();

        StartCoroutine(ResourceProduction());
    }

    protected override IEnumerator ResourceProduction()
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

            // TODO : Cat 스크립트로 옮기기
            for (int i = 0; i < PlacedInBuildingCats.Count; i++)
            {
                if (PlacedInBuildingCats[i] == null)
                    continue;

                // 골드 생산 10번하면 쉬러 가야 함
                if (++PlacedInBuildingCats[i].NumberOfGoldProduction >= 10 && BuildingManager.CanGoRest(out Vector3 buildingPos))
                {
                    PlacedInBuildingCats[i].NumberOfGoldProduction = 0;
                    PlacedInBuildingCats[i].GoWorking = false;
                    PlacedInBuildingCats[i].GoResting = true;

                    // 랜덤한 에너지 생산 건물 위치로 가기
                    PlacedInBuildingCats[i].GoToRest(buildingPos);
                    RemoveCat(PlacedInBuildingCats[i].catData);
                }
            }

            yield return waitResourceChargingTime;
        }
    }

    public override IEnumerator WaitGetResource()
    {
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
                Destroy(Instantiate(ResourceAcquisitionEffect, transform.position + (Vector3.up * 0.5f), Quaternion.identity, ParticleCanvasRt), 1.5f);

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
        ConstructionResourceText.text = PlacingPrice;
        ConstructionResourceText.rectTransform.DOAnchorPosY(ConstructionResourceText.rectTransform.anchoredPosition.y + 150, 1);
        yield return ConstructionResourceText.DOFade(0f, 0.7f).WaitForCompletion();

        ConstructionResourceText.gameObject.SetActive(false);

        BuildingManager.s_GoldBuildingCount[buildingType]++;
        BuildingManager.s_GoldProductionBuildings.Add(this);

        GridBuildingSystem.InitializeWithBuilding(BuildingInfo.BuildingPrefab);
    }

    [Obsolete("고양이 스크립트로 옮기기")]
    public void SetPos(int index = 0)
    {
        var cat = PlacedInBuildingCats[index];

        switch (buildingType)
        {
            case GoldBuildingType.IceFishing:
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using DG.Tweening;

public class EnergyProductionBuilding : ProductionBuilding
{
    [Header("Energy Production Building")]
    public EnergyBuildingType buildingType;

    private const float AUTO_GET_ENERGY_DELAY = 10f;
    public override string ConstructionCost
    {
        get
        {
            if (BuildingManager.s_EnergyBuildingCount[buildingType] == 0)
                return DefaultConstructionCost;
            return (DefaultConstructionCost.returnValue() * (BuildingManager.s_EnergyBuildingCount[buildingType] * 3)).returnStr();
        }
    }


    protected override void Start()
    {
        base.Start();

        BuildingInfomationButton.onClick.AddListener(() =>
        {
            BuildingInfomation.gameObject.SetActive(true);

            PlacedInBuildingCats = PlacedInBuildingCats.Where(x => x.building == this).ToList();

            if (PlacedInBuildingCats.Count == 0)
            {
                BuildingInfomation.SetData(BuildingType.Energy, this, null, SpriteRenderer.sprite);
            }
            else
            {
                BuildingInfomation.SetData(BuildingType.Energy, this, PlacedInBuildingCats, SpriteRenderer.sprite);
            }
        });
    }

    public override void OnCatMemberChange(CatData catData, int index = 0, Action action = null)
    {
        PlacedInBuildingCats.Add(catData.Cat);
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
                waitResourceChargingTime = new WaitForSeconds((float)(DefaultResourceChargingTime * Math.Round(decreasingfigure / 100f, 3)));
        }
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

            for (int i = 0; i < MaxDeployableCat; i++)
            {
                if (PlacedInBuildingCats[i] == null)
                    continue;

                // 에너지 생산 3번하면 일하러 가야함
                if (++PlacedInBuildingCats[i].NumberOfEnergyProduction >= 3)
                {
                    PlacedInBuildingCats[i].GoToWork(PlacedInBuildingCats[i].building.transform.position);
                    PlacedInBuildingCats[i].building.OnCatMemberChange(PlacedInBuildingCats[i].catData, PlacedInBuildingCats[i].catNum);

                    PlacedInBuildingCats.RemoveAt(i);
                }
            }

            yield return waitResourceChargingTime;
        }
    }

    public override IEnumerator WaitGetResource()
    {
        var curtime = 0f;
        var autogetenergy = false;

        while (true)
        {
            if (didGetResource)
            {
                GameManager.Instance._energy += autogetenergy ? ProductionResource.returnValue() : ProductionResource.returnValue() * 0.5f;
                didGetResource = false;
                CollectResourceButton.gameObject.SetActive(false);

                // 골드 획득 연출
                DailyQuestManager.dailyQuests.quests[(int)QuestType.Stamina]._index++;
                SoundManager.Instance.PlaySoundClip("SFX_Goods", SoundType.SFX);
                Destroy(Instantiate(ResourceAcquisitionEffect, transform.position + (Vector3.up * 0.5f), Quaternion.identity, CanvasRt), 1.5f);

                yield break;
            }

            curtime += Time.deltaTime;

            if (curtime >= AUTO_GET_ENERGY_DELAY)
            {
                curtime = 0;

                autogetenergy = true;
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

        BuildingManager.s_EnergyBuildingCount[buildingType]++;
        BuildingManager.s_EnergyProductionBuildings.Add(this);

        GridBuildingSystem.InitializeWithBuilding(BuildingInfo.BuildingPrefab);

    }

    public bool CanDeploy()
    {
        return PlacedInBuildingCats.Count < MaxDeployableCat;
    }
}

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

            for (var i = 0; i < Rating; i++)
            {
                gold *= IncreasePerLevelUp;
            }

            return gold.returnStr();
        }
    }


    public override string ConstructionCost => (DefaultConstructionCost.returnValue() * (BuildingManager.s_GoldBuildings[buildingType] * 3)).returnStr();

    private bool didGetMoney;

    private WaitForSeconds waitGoldChargingTime;

    [SerializeField] private TextMeshProUGUI ConstructionGoldText;

    #endregion

    [SerializeField] private Button BuildingButton;
    [SerializeField] private GameObject BuildingUI;

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


    protected override void Start()
    {
        base.Start();

        waitGoldChargingTime = new WaitForSeconds(DefaultGoldChargingTime);

        CollectMoneyButton.onClick.AddListener(() =>
        {
            didGetMoney = true;
        });

        BuildingButton.onClick.AddListener(() =>
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


    public IEnumerator ResourceProduction()
    {
        while (true)
        {
            yield return waitGoldChargingTime;

            yield return StartCoroutine(CWaitClick());
        }
    }

    public IEnumerator CWaitClick()
    {
        while (true)
        {
            if (didGetMoney)
            {
                GameManager.Instance._coin += ProductionGold.returnValue();
                didGetMoney = false;
                yield break;
            }

            yield return null;
        }
    }

    public override void Place()
    {
        base.Place();

        StartCoroutine(ResourceProduction());
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

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionBuilding : Building
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
                CollectResourceButton.gameObject.SetActive(false);
            }
            else
            {
                SpriteRenderer.color = Color.white;
                SpriteRenderer.sortingOrder = 0;
                DeployingUIParent.SetActive(false);
            }
        }
    }


    [SerializeField] protected Button CollectResourceButton;
    [Header("Resource")]
    [SerializeField] protected string DefaultResource;
    [SerializeField] protected float DefaultResourceChargingTime;
    [SerializeField] protected float IncreasePerLevelUp;


    public string ProductionResource
    {
        get
        {
            var resource = DefaultResource.returnValue();

            for (var i = 0; i < Level - 1; i++)
            {
                resource += resource * Math.Round((double)(IncreasePerLevelUp / 100f), 3);
            }

            return resource.returnStr();
        }
    }


    [Header("Level Up")]
    [SerializeField] protected string DefaultLevelUpCost;

    public string LevelUpCost(int level)
    {
        var cost = DefaultLevelUpCost.returnValue();

        for (int i = 0; i < level - 1; i++)
        {
            cost += cost * Math.Round((double)(8f / 100f), 3);
        }

        return cost.returnStr();
    }

    protected bool didGetResource;

    protected WaitForSeconds waitResourceChargingTime;


    [Space]
    [SerializeField] protected TextMeshProUGUI ConstructionResourceText;
    [SerializeField] protected GameObject ResourceAcquisitionEffect;

    [Space(10)]
    [SerializeField] protected GameObject BuildingUI;
    [SerializeField] protected Button BuildingInfomationButton;

    protected static GameObject s_buildingUI;

    // 건물에 배치된 고양이
    public List<Cat> PlacedInBuildingCats = new List<Cat>();
    public CatPlacementWorkingCats WorkingCats;

    protected override void Start()
    {
        base.Start();

        waitResourceChargingTime = new WaitForSeconds(DefaultResourceChargingTime);

        CollectResourceButton.onClick.AddListener(() =>
        {
            didGetResource = true;
        });
    }
    public virtual void OnCatMemberChange(CatData catData, int index, Action action) { }
    protected virtual IEnumerator ResourceProduction() { yield return null; }
    public virtual IEnumerator WaitGetResource() { yield return null; }

    private void OnMouseDown()
    {
        if (isDeploying || IsPointerOverGameObject())
            return;

        if (CollectResourceButton.gameObject.activeSelf)
        {
            didGetResource = true;
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

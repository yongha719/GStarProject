using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingInfo : MonoBehaviour
{
    public GameObject BuildingPrefab;
    private string constructionCost;

    public Button BuyButton;
    private TextMeshProUGUI buildingCostText;

    [HideInInspector] public GameObject BuildingInstalltionUI;

    [HideInInspector] public Building Building;

    private void Awake()
    {
        Building = BuildingPrefab.GetComponent<Building>();
        constructionCost = Building.ConstructionCost;

        buildingCostText = BuyButton.GetComponentInChildren<TextMeshProUGUI>();
        buildingCostText.text = constructionCost;
    }

    public void BuyButtonOnclick(Action<Building> call)
    {
        BuyButton.onClick.AddListener(() => call(Building));
    }
}

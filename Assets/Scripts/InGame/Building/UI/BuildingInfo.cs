using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingInfo : MonoBehaviour
{
    public GameObject BuildingPrefab;
    private string gold;

    public Button BuyButton;
    private TextMeshProUGUI buildingCostText;

    [HideInInspector] public GameObject BuildingInstalltionUI;

    [HideInInspector] public Building Building;

    private void Awake()
    {
        Building = BuildingPrefab.GetComponent<Building>();
        gold = Building.ConstructionCost;

        buildingCostText = BuyButton.GetComponentInChildren<TextMeshProUGUI>();
        buildingCostText.text = gold;
    }

    public void BuyButtonOnclick(Action<Building> call)
    {
        BuyButton.onClick.AddListener(() => call(Building));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingBuyInfoUI : MonoBehaviour
{
    public GameObject BuildingPrefab;

    public Button BuyButton;
    private TextMeshProUGUI buildingCostText;

    [HideInInspector] public GameObject BuildingInstalltionUI;

    [HideInInspector] public Building Building;

    private void Awake()
    {
        Building = BuildingPrefab.GetComponent<Building>();

        buildingCostText = BuyButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        buildingCostText.text = Building.ConstructionCost;
    }

    public void BuyButtonOnclick(Action<Building> call)
    {
        BuyButton.onClick.AddListener(() => call(Building));
    }
}

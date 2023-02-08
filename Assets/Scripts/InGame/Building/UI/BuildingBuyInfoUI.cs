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
    private TextMeshProUGUI buildingPriceText;

    [HideInInspector] public GameObject BuildingInstalltionUI;

    [HideInInspector] public Building Building;

    private void Awake()
    {
        Building = BuildingPrefab.GetComponent<Building>();

        buildingPriceText = BuyButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        Debug.Log(nameof(BuildingBuyInfoUI));

        buildingPriceText.text = Building.PlacingPrice;
    }

    public void BuyButtonOnclick(Action<Building> call)
    {
        BuyButton.onClick.AddListener(() => call(Building));
    }
}

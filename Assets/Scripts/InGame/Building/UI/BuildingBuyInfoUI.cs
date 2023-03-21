using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBuyInfoUI : MonoBehaviour
{
    public GameObject BuildingPrefab;

    public Image BuildingIcon;
    public Button BuyButton;
    private TextMeshProUGUI buildingPriceText;

    [HideInInspector] public GameObject BuildingInstalltionUI;

    [HideInInspector] public Building Building;

    private void Awake()
    {
        Building = BuildingPrefab.GetComponent<Building>();

        buildingPriceText = BuyButton.GetComponentInChildren<TextMeshProUGUI>();

        (Building as ProductionBuilding).BuildingIcon = BuildingIcon.sprite;
    }

    private void OnEnable()
    {
        buildingPriceText.text = Building.PlacingPrice;
    }

    public void BuyButtonOnclick(Action<Building> call)
    {
        BuyButton.onClick.AddListener(() =>
        {
            call(Building);
        });
    }
}

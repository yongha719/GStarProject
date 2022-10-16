using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfo : MonoBehaviour
{
    public string buildingID;
    public string buildingName;
    public int Gold;
    public GameObject buildingPrefab;
    public Button BuyButton;
    [HideInInspector] public GameObject BuildingInstalltionUI;

    [HideInInspector] public Building Building;

    private void Start()
    {
        Building = buildingPrefab.GetComponent<Building>();
    }

    public void BuyButtonOnclick(Action<Building> call)
    {
        BuyButton.onClick.AddListener(() => call(Building));
    }
}

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
    public GameObject BuildingInstalltionUI;

    public Building Building;

    public void BuyButtonOnclick(Action<Building> call)
    {
        BuyButton.onClick.AddListener(() => call(Building));
    }
}

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
    public GameObject building;
    public Button BuyButton;

    public void Onclick(Action call)
    {
        BuyButton.onClick.AddListener(() => call());
    }
}

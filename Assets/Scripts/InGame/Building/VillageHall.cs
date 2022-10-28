using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageHall : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;

    private bool isDeploying;
    public bool IsDeploying
    {
        get
        {
            return isDeploying;
        }

        set
        {
            isDeploying = value;
        }
    }

    public int Level = 1;

    private const string DefaultLevelUpCost = "1000a";
    public string GetLevelUpCost => (DefaultLevelUpCost.returnValue() * Mathf.Pow(1000, Level - 1)).returnStr();

    private GridBuildingSystem GridBuildingSystem;

    void Start()
    {
        GridBuildingSystem = GridBuildingSystem.Instance;

        if (CanBePlaced())
        {
            var cellPos = GridBuildingSystem.gridLayout.LocalToCell(Vector2.zero);

            transform.localPosition = GridBuildingSystem.gridLayout.CellToLocalInterpolated(cellPos);
            Place();
        }
    }

    void Update()
    {

    }

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        return GridBuildingSystem.CanTakeArea(areaTemp);
    }


    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;

        GridBuildingSystem.TakeArea(areaTemp);

        IsDeploying = false;

    }
}

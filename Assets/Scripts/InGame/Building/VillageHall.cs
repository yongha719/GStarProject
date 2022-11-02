using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int CurAreaSize => (Level * 2) + 2;


    private const string DefaultLevelUpCost = "1000a";
    public string GetLevelUpCost => (DefaultLevelUpCost.returnValue() * Mathf.Pow(1000, Level - 1)).returnStr();

    [Tooltip("마을 회관 UI"), SerializeField] private GameObject VillageHallUI;

    private GridBuildingSystem GridBuildingSystem;

    void Start()
    {
        GridBuildingSystem = GridBuildingSystem.Instance;

        // z 값 조정 잘하자
        area.position = new Vector3Int(-1, -1, 0);

        GridBuildingSystem.SetTilesBlock(area, TileType.Installed, GridBuildingSystem.BuildingTilemap);

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


    public void LevelUp()
    {
        Level++;
        
        // 레벨업 했을때 이벤트들
        GridBuildingSystem.ExpandArea(Level);
    }

    private void OnMouseDown()
    {
        if (VillageHallUI.activeSelf)
        {
            VillageHallUI.SetActive(false);
        }
        else
        {
            VillageHallUI.SetActive(true);
        }
    }
}

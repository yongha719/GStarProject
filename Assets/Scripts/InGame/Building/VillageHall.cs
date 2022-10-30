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

    private const string DefaultLevelUpCost = "1000a";
    public string GetLevelUpCost => (DefaultLevelUpCost.returnValue() * Mathf.Pow(1000, Level - 1)).returnStr();

    [Header("Building UI")]
    [SerializeField] private GameObject BuildingUi;
    [SerializeField] private Button CatsRecruitmentButton;
    [SerializeField] private Button BuildingDetailsButton;

    private GridBuildingSystem GridBuildingSystem;

    void Start()
    {
        GridBuildingSystem = GridBuildingSystem.Instance;

        //GridBuildingSystem.ExpandArea(Level);

        // z 값 조정 잘하자
        area.position = new Vector3Int(-1, -1, 0);

        GridBuildingSystem.SetTilesBlock(area, TileType.Installed, GridBuildingSystem.BuildingTilemap);

        CatsRecruitmentButton.onClick.AddListener(() =>
        {

        });

        BuildingDetailsButton.onClick.AddListener(() =>
        {

        });
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

    private void OnMouseDown()
    {
        BuildingUi.SetActive(true);
    }
}

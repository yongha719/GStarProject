using UnityEngine;

public class VillageHall : Building
{
    public override bool IsDeploying
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

    //public int Level = 1;

    private const string DefaultLevelUpCost = "1000a";

    // 레벨업 비용 = 기본 레벨업 비용 * 1000의 (Level - 1) 제곱
    public string GetLevelUpCost => (DefaultLevelUpCost.returnValue() * Mathf.Pow(1000, Level - 1)).returnStr();

    [Tooltip("마을 회관 UI"), SerializeField] private GameObject VillageHallUI;

    protected override void Start()
    {
        base.Start();

        //GridBuildingSystem.CurBuilding = this;

        // z 값 조정 잘하자
        area.position = new Vector3Int(-1, -1, 0);

        GridBuildingSystem.SetTilesBlock(area, TileType.Installed, GridBuildingSystem.BuildingTilemap);
    }

    public void LevelUp()
    {
        Level++;

        // 레벨업 했을때 이벤트들
        GridBuildingSystem.ExpandArea(Level);
    }

    private void OnMouseDown()
    {
        if (IsPointerOverGameObject())
        {
            return;
        }

        print("ddaddda");

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

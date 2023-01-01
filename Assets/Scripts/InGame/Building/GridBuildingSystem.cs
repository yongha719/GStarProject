using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Empty,                 // 빈 타일
    Uninstalled,           // 확장된 범위안에 있지만 빈 타일
    Installed,             // 확장된 범위안에 건물이 설치된 타일
    NotExpanded,           // 확장되지 않은 영역
    Green,                 // 설치 가능한 영역을 표시할 때 사용
    Red                    // 설치 불가능한 영역을 표시할 때 사용
}

public class GridBuildingSystem : Singleton<GridBuildingSystem>
{
    public GridLayout gridLayout;
    public Tilemap TempTilemap;
    public Tilemap BuildingTilemap;
    public Tilemap TreeTilemap;
    [SerializeField] private GameObject MainCanvas;

    private Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    [Space]
    public string CurBuildingName;
    [HideInInspector]
    public Building CurBuilding;
    [SerializeField] private ParticleSystem BuildingInstallationEffect;



    private Vector3 prevPos;
    private BoundsInt prevArea;
    private Vector3Int cellPos;

    // 마을 땅 크기
    public Vector2 ViliageAreaSize;

    private const string path = "Tile/";

    public static bool IsDeploying = false;

    public VillageHall VillageHall;

    private void Awake()
    {
        VillageHall = FindObjectOfType<VillageHall>();

        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.Uninstalled, Resources.Load<TileBase>(path + nameof(TileType.Uninstalled)));
        tileBases.Add(TileType.Installed, Resources.Load<TileBase>(path + nameof(TileType.Installed)));
        tileBases.Add(TileType.NotExpanded, Resources.Load<TileBase>(path + nameof(TileType.NotExpanded)));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(path + nameof(TileType.Green)));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(path + nameof(TileType.Red)));

        ViliageAreaSize = new Vector2(4, 4);

        MainCanvas = GameObject.Find("MainUICanvas");
    }

    void Start()
    {
    }

    // 마을회관 체크
    public bool WallCheck(Vector2Int pos)
    {
        var max = gridLayout.CellToLocal(VillageHall.area.position - VillageHall.area.max) * -2;
        var min = gridLayout.CellToLocal(VillageHall.area.position) * 2;

        if ((pos.x < max.x && pos.y < max.y) && (pos.x >= min.x && pos.y >= min.y))
            return true;

        return false;
    }

    void Update()
    {
        #region Building Installation
        if (CurBuilding != null)
        {
            // 클릭한 오브젝트가 UI면 return
            if (IsPointerOverGameObject())
                return;

            if (Input.GetMouseButton(0))
            {
                if (CurBuilding.Placed == false)
                {
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    cellPos = gridLayout.LocalToCell(touchPos);

                    if (prevPos != cellPos)
                    {
                        CurBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);
                        prevPos = cellPos;
                        FollowBuiliding();
                    }
                }
            }
        }
        #endregion
    }

    private TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y];

        int cnt = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[cnt++] = tilemap.GetTile(pos);
        }

        return array;
    }

    public void BuildingClear(bool Destroy = false)
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y];

        TileBase[] tempbase = GetTilesBlock(prevArea, BuildingTilemap);

        int size = tempbase.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < size; i++)
        {
            if (tempbase[i] != tileBases[TileType.NotExpanded])
            {
                tileArray[i] = tileBases[TileType.Uninstalled];
            }
        }

        TempTilemap.SetTilesBlock(prevArea, tileArray);

        if (Destroy)
        {
            IsDeploying = false;
            MainCanvas.SetActive(true);
        }
    }

    private void AllClearArea()
    {
        BoundsInt area = TempTilemap.cellBounds;
        TileBase[] toClear = new TileBase[area.size.x * area.size.y * area.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(area, toClear);
    }

    public void InitializeWithBuilding(GameObject building)
    {
        CurBuilding = Instantiate(building, Vector3.zero, Quaternion.identity, transform).GetComponent<Building>();
        FollowBuiliding();

        IsDeploying = true;
    }

    private void FollowBuiliding()
    {
        MainCanvas.SetActive(false);
        BuildingClear();

        CurBuilding.area.position = gridLayout.WorldToCell(CurBuilding.gameObject.transform.position);
        BoundsInt buildingArea = CurBuilding.area;

        TileBase[] tempbase = GetTilesBlock(buildingArea, BuildingTilemap);

        int size = tempbase.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < size; i++)
        {
            if (tempbase[i] == tileBases[TileType.Uninstalled])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                tileArray[i] = tileBases[TileType.Red];
            }
        }

        TempTilemap.SetTilesBlock(buildingArea, tileArray);

        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] tempbase = GetTilesBlock(area, BuildingTilemap);

        for (int i = 0; i < tempbase.Length; i++)
        {
            if (tempbase[i] != tileBases[TileType.Uninstalled])
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Installed, BuildingTilemap);
    }


    public void BuildingInstallEffectPlay(Vector2 pos)
    {
        Instantiate(BuildingInstallationEffect, pos, Quaternion.identity);
    }

    /// <summary>
    /// 마을 회관 레벨업시 영역 확장
    /// </summary>
    public void ExpandArea(int level)
    {
        ViliageAreaSize = Vector2.one * (level * 2 + 2);

        // 레벨당 영역 나누기 2
        var areaDividedby2 = ((level * 2) + 2) / 2;
        // 바꿀 영역                                               
        BoundsInt toChangeArea = new BoundsInt(new Vector3Int(-areaDividedby2 - 1, -areaDividedby2 - 1, 0)
            // 땅 공간 여유남겨두기
            , new Vector3Int((level * 2) + 3, (level * 2) + 3, 1));

        // 바뀐 영역
        BoundsInt changedArea = new BoundsInt(new Vector3Int(-(level * 2) / 2, -(level * 2) / 2, 0)
            // 땅 공간 여유남겨두기
            , new Vector3Int((level * 2), (level * 2), 1));

        // 고양이가 이동 가능한 범위 변경
        CatManager.Instance.ChangeRangeCatMovement(level * 2 + 2);

        // 타일 정보 바꿔주기
        var tile = GetTilesBlock(changedArea, BuildingTilemap);
        SetTilesBlock(toChangeArea, TileType.Empty, TreeTilemap);
        SetTilesBlock(toChangeArea, TileType.NotExpanded, BuildingTilemap);

        toChangeArea.position = toChangeArea.position + new Vector3Int(1, 1, 0);
        toChangeArea.size = new Vector3Int((level * 2) + 2, (level * 2) + 2, 1);
        SetTilesBlock(toChangeArea, TileType.Uninstalled, BuildingTilemap);
        BuildingTilemap.SetTilesBlock(changedArea, tile);

        prevArea = toChangeArea;
    }

    public void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y;

        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private void FillTiles(TileBase[] arr, TileType type)
    {
        int arrLength = arr.Length;

        for (int i = 0; i < arrLength; i++)
            arr[i] = tileBases[type];
    }

    public void Place()
    {
        if (CurBuilding.CanBePlaced())
        {
            CurBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);
            CurBuilding.Place();

            MainCanvas.SetActive(true);
            IsDeploying = false;
        }
    }

    private bool IsPointerOverGameObject()
    {
        #if UNITY_EDITOR
                // Check mouse
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    return true;
                }
        #elif UNITY_ANDROID

        // Check touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return true;
            }
        }
#endif
        return false;
    }
}


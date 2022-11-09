using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System.Diagnostics.SymbolStore;

public enum TileType
{
    Empty, Uninstalled, Installed, Green, Red
}

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance;

    public GridLayout gridLayout;
    public Tilemap TempTilemap;
    public Tilemap BuildingTilemap;
    public Tilemap TreeTilemap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    public string CurBuildingName;
    [SerializeField] private Building CurBuilding;
    [SerializeField] private ParticleSystem BuildingInstallationEffect;

    private Vector3 prevPos;
    private BoundsInt prevArea;
    private Vector3Int cellPos;


    private const string path = "Tile/";

    public static bool IsDeploying = false;

    private void Awake()
    {
        Instance = this;

        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.Uninstalled, Resources.Load<TileBase>(path + nameof(TileType.Uninstalled)));
        tileBases.Add(TileType.Installed, Resources.Load<TileBase>(path + nameof(TileType.Installed)));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(path + nameof(TileType.Green)));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(path + nameof(TileType.Red)));

    }

    void Start()
    {
    }

    void Update()
    {
        #region Building Installation
        if (CurBuilding != null)
        {
            if (Input.GetMouseButton(0))
            {
                // 클릭한 오브젝트가 UI면 return
#if UNITY_EDITOR
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
#elif UNITY_ANDROID
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                       return;
                    }
                }
#endif
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

        if (Input.GetKeyDown(KeyCode.G))
        {
            ExpandArea(2);
            print("key");
        }
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

    public void BuildingClear()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y];
        FillTiles(toClear, TileType.Uninstalled);
        TempTilemap.SetTilesBlock(prevArea, toClear);
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
        //MainTilemap.color = new Color(1, 1, 1, 0.5f);
        CurBuilding = Instantiate(building, Vector3.zero, Quaternion.identity, transform).GetComponent<Building>();
        FollowBuiliding();
    }

    private void FollowBuiliding()
    {
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
        var areaincrementdividedby2 = ((level * 2) + 2) / 2;

        BoundsInt area = new BoundsInt(new Vector3Int(areaincrementdividedby2 * -1, areaincrementdividedby2 * -1, 0), new Vector3Int((level * 2) + 2, (level * 2) + 2, 1));
        // 영역 증가량 나누기 2
        //area.position = Vector3Int.zero;
        //area.min = new Vector3Int(areaincrementdividedby2 * -1, areaincrementdividedby2 * -1, 1);
        //area.max = new Vector3Int(areaincrementdividedby2, areaincrementdividedby2, 1);

        // TODO : 이미 설치된 건물들 판별해야 함
        var tile = GetTilesBlock(area, BuildingTilemap);
        SetTilesBlock(area, TileType.Green, BuildingTilemap);
        SetTilesBlock(area, TileType.Empty, TreeTilemap);
        BuildingTilemap.SetTilesBlock(area, tile);
    }



    public void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y;

        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private void FillTile(TileBase[] tile, TileType tiletype, int index) => tile[index] = tileBases[tiletype];

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
        }
    }

}


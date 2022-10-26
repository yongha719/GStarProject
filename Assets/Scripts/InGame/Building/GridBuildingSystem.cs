using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public enum TileType
{
    Empty, EnableZone, Green, Red
}

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance;

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    public string CurBuildingName;
    [SerializeField] private Building CurBuilding;
    [SerializeField] private ParticleSystem BuildingInstallationEffect;

    private Vector3 prevPos;
    private BoundsInt prevArea;
    private Vector3Int cellPos;

    private const string path = "Tile/";
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tileBases.Add(TileType.Empty, Resources.Load<TileBase>($"{path}DefaultTile"));
        tileBases.Add(TileType.EnableZone, Resources.Load<TileBase>($"{path}white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>($"{path}green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>($"{path}red"));


        BuildingInstallEffectPlay(Vector2.zero);
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
    }

    #region TileMap
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

    private void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private void FillTile(TileBase[] tile, TileType tiletype, int index)
    {
        tile[index] = tileBases[tiletype];
    }

    private void FillTiles(TileBase[] arr, TileType type)
    {
        int arrLength = arr.Length;

        for (int i = 0; i < arrLength; i++)
            arr[i] = tileBases[type];
    }
    #endregion

    public void InitializeWithBuilding(GameObject building)
    {
        //MainTilemap.color = new Color(1, 1, 1, 0.5f);
        CurBuilding = Instantiate(building, Vector3.zero, Quaternion.identity, transform).GetComponent<Building>();
        FollowBuiliding();
    }

    public void Place()
    {
        if (CurBuilding.CanBePlaced())
        {
            CurBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);
            CurBuilding.Place();
            //MainTilemap.color = Color.clear;
        }
    }

    public void BuildingClear()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void AllClearArea()
    {
        BoundsInt area = TempTilemap.cellBounds;
        TileBase[] toClear = new TileBase[area.size.x * area.size.y * area.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(area, toClear);


    }

    private void FollowBuiliding()
    {
        BuildingClear();

        CurBuilding.area.position = gridLayout.WorldToCell(CurBuilding.gameObject.transform.position);
        BoundsInt buildingArea = CurBuilding.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < size; i++)
        {
            tileArray[i] = (baseArray[i] == tileBases[TileType.Empty]) ? tileBases[TileType.Green] : tileBases[TileType.Red];
        }

        TempTilemap.SetTilesBlock(buildingArea, tileArray);

        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        foreach (var tilebase in baseArray)
        {
            if (tilebase != tileBases[TileType.Empty])
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        //SetTilesBlock(area, TileType.Green, MainTilemap);
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

    }
}


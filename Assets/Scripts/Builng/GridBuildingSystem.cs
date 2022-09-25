using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public enum TileType
{
    Empty, White, Green, Red
}

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance;

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;
    public Transform BuildingParent;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private Building temp;
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
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>($"{path}white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>($"{path}green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>($"{path}red"));
    }

    void Update()
    {
        if (!temp)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(0))
            {
                return;
            }

            if (temp.Placed == false)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cellPos = gridLayout.LocalToCell(touchPos);

                if (prevPos != cellPos)
                {
                    temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);
                    prevPos = cellPos;
                    FollowBuiliding();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (temp.CanBePlaced())
            {
                temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);
                temp.Place();
                MainTilemap.color = Color.clear;
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            BuildingClear();
            Destroy(temp.gameObject);
            MainTilemap.color = Color.clear;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            BuildingClear();
        }
    }

    #region TileMap
    private TileBase[] GetTilesBlock(in BoundsInt area, Tilemap tilemap)
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

    private void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
            arr[i] = tileBases[type];
    }
    #endregion

    //Button
    public void InitializeWithBuilding(GameObject building)
    {
        MainTilemap.color = new Color(1, 1, 1, 0.5f);
        temp = Instantiate(building, Vector3.zero, Quaternion.identity, BuildingParent).GetComponent<Building>();
        FollowBuiliding();
    }

    private void BuildingClear()
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

        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        print(size);
        for (int i = 0; i < size; i++)
        {
            if (baseArray[i] == tileBases[TileType.Empty])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);

                break;
            }
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
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }
}


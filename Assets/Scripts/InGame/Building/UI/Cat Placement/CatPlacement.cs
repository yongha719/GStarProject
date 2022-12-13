using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CatPlacement : MonoBehaviour
{
    [SerializeField] private GameObject CatPlacementUI;
    [SerializeField] private UnityEngine.UI.Image BuildingImage;
    [SerializeField] private TaskText WorkText;
    [SerializeField] private CatPlacementWarning CatPlacementWarning;

    [Header("Parent Transform")]
    // 배치할 수 있는 고양이 Scroll Rect Content Transform
    [SerializeField] private RectTransform CatToPlacementContentTr;
    private Transform WorkingCatParentTr;

    [Header("고양이 배치")]
    // 건물마다 일할 수 있는 고양이들의 수가 다르기 때문에 고양이들의 배치가 달라서 프리팹으로 따로 만들었음
    [Tooltip("건물당 일하는 고양이들의 배치"), SerializeField] private List<GameObject> GoldBuildingWorkingCatPlacements = new List<GameObject>();
    [SerializeField] private List<GameObject> EnergyBuildingWorkingCatPlacements = new List<GameObject>();
    [SerializeField] private GameObject CatToPlacementPrefab;
    [SerializeField] private GameObject AbilityPrefab;

    /// <summary>           
    /// 배치된 고양이 중 현재 클릭된 고양이
    /// => 바꿀 수 있는 고양이
    /// </summary>
    private CatData CurSelectedCat;

    /// <summary>
    /// 현재 선택된 고양이의 인덱스
    /// </summary>
    private int CurSelectedCatIndex;
    private CatPlacementWorkingCats workingCats;

    private const string WORKING_TEXT = "(이)가 일하는 중.";

    ProductionBuilding productionBuilding;
    BuildingType BuildingType;
    List<CatAbilityUI> catAbilityUIs = new List<CatAbilityUI>();

    private CatManager CatManager;
    private GridBuildingSystem GridBuildingSystem;

    private void Awake()
    {
        CatManager = CatManager.Instance;
        GridBuildingSystem = GridBuildingSystem.Instance;

        WorkingCatParentTr = BuildingImage.transform.parent;
    }

    /// <summary>
    /// 배치창에 보여줄 건물 정보와 그 건물에서 일하고 있는 고양이 정보
    /// </summary>
    /// <param name="catData">고양이 정보</param>
    /// <param name="buildingSprite">건물 이미지</param>
    public void SetBuildingInfo(BuildingType buildingType, in ProductionBuilding building, in CatData[] catData)
    {
        productionBuilding = building;
        BuildingType = buildingType;

        SetWorkingCat(building);

        if (catData == null)
        {
            WorkText.gameObject.SetActive(false);
            //BuildingImage.sprite = building.Spri;
            SetCatList();

            return;
        }

        WorkText.gameObject.SetActive(true);
        WorkText.SetText(catData[0].Name + WORKING_TEXT);

        SetCatList();
    }

    // Create Working cat
    private bool CheckCreateWorkingCat<T>(in ProductionBuilding productionbuilding)
        where T : ProductionBuilding
    {
        var building = productionbuilding as T;

        if (building == null) return false;

        // TODO: ProductionBuilding 클래스를 만들어서 Gold~, Energy~ 클래스의 부모로 만들고
        // 같이 사용하는 변수들을 ProductionBuilding로 옮기기 (코드 중복 줄이기)

        if (building.WorkingCats == null)
        {
            if (BuildingType == BuildingType.Gold)
                workingCats = Instantiate(GoldBuildingWorkingCatPlacements[(int)building.BuildinTypeToInt], WorkingCatParentTr).GetComponent<CatPlacementWorkingCats>();
            else if (BuildingType == BuildingType.Energy)
                workingCats = Instantiate(EnergyBuildingWorkingCatPlacements[(int)building.BuildinTypeToInt], WorkingCatParentTr).GetComponent<CatPlacementWorkingCats>();

            building.WorkingCats = workingCats;
        }
        else
        {
            workingCats = building.WorkingCats;
            workingCats.gameObject.SetActive(true);
        }

        workingCats.MaxDeployableCat = building.MaxDeployableCat;

        return true;
    }

    /// <summary>
    /// 배치된 고양이 생성
    /// </summary>
    private void SetWorkingCat(in ProductionBuilding Productionbuilding)
    {
        if (CheckCreateWorkingCat<GoldProductionBuilding>(Productionbuilding) == false)
        {
            Debug.Assert(false, $"{nameof(GoldProductionBuilding)} {nameof(CheckCreateWorkingCat)} failed");
            return;
        }

        if (CheckCreateWorkingCat<EnergyProductionBuilding>(Productionbuilding) == false)
        {
            Debug.Assert(false, $"{nameof(EnergyProductionBuilding)} {nameof(CheckCreateWorkingCat)} failed");
            return;
        }
    }

    public bool CheckCatWorking(in List<CatData> catDatas, int index)
    {
        if (workingCats.CatDatas.Count <= 0) return false;

        CurSelectedCat = workingCats.CatDatas[0];

        for (int j = 0; j < workingCats.CatDatas.Count; j++)
        {
            if (workingCats.CatDatas[j] == catDatas[index])
            {
                return true;
            }
        }

        return false;
    }

    private void CreateYesButtonFromCatData(in CatToPlaceUI cattoplace, in CatData catData)
    {
        // 건물에서 일하는 고양이가 없을 때
        if (CurSelectedCat == null || workingCats.CatDatas.Count < workingCats.MaxDeployableCat)
        {
            CurSelectedCat = catData;

            WorkText.SetText(catData.Name + WORKING_TEXT);

            Destroy(cattoplace.gameObject);

            var ability = Instantiate(AbilityPrefab, workingCats.AbilityParent).GetComponent<CatAbilityUI>();
            ability.SetAbility(catData.AbilitySprite, catData.AbilityRating);
            workingCats.CatAbilitys.Add(ability);

            workingCats.SetData(workingCats.CatDatas.Count, in catData, call: (catnum) => CurSelectedCatIndex = catnum);
        }
        else
        {
            // 고양이를 바꿔야 하는거임

            // 배치된 고양이와 정보 교체
            workingCats.SetData(CurSelectedCatIndex, in catData);
            cattoplace.SetData(CurSelectedCat);
        }

        CurSelectedCat = catData;

        // 건물에서 일하는 고양이 바꾸기
        BuildingManager.GetGoldProductionBuilding(productionBuilding).OnCatMemberChange(catData, CurSelectedCatIndex);
    }

    private void PlacementOnClick(CatToPlaceUI cattoplace, CatData catData)
    {
        // 경고창
        CatPlacementWarning.gameObject.SetActive(true);
        CatPlacementWarning.SetWaringData(catData.Name);

        // 경고창 Yes 버튼
        CatPlacementWarning.OnClickYesButton(() => { CreateYesButtonFromCatData(in cattoplace, in catData); });
    }

    /// <summary>
    /// 배치할 수 있는 고양이 리스트 생성
    /// </summary>
    private void SetCatList()
    {
        CurSelectedCat = null;

        DestroyChild(CatToPlacementContentTr);

        // 고양이 리스트 출력
        var CatList = CatManager.CatList.Select(x => x.catData).ToList();
        var cnt = CatList.Count;

        //bool jump;

        for (int index = 0; index < cnt; index++)
        {
            if (CheckCatWorking(CatList, index)) continue;

            var catToPlacement = Instantiate(CatToPlacementPrefab, CatToPlacementContentTr).GetComponent<CatToPlaceUI>();

            catToPlacement.SetData(CatList[index],
            // 배치 버튼에 들어갈 이벤트들
            onclick: PlacementOnClick);
        }
    }

    void DestroyChild(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    private void OnDisable()
    {
        WorkText.SetText();
        workingCats.gameObject.SetActive(false);
        workingCats = null;
    }
}

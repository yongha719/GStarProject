using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CatPlacementUI : UIPopup
{
    [SerializeField] private GameObject CatPlacementUIObj;
    [SerializeField] private UnityEngine.UI.Image BuildingImage;
    [SerializeField] private TMPro.TextMeshProUGUI BuildingNameText;
    [SerializeField] private TaskText WorkText;
    [SerializeField] private CatPlacementWarningUI CatPlacementWarning;

    // 배치할 수 있는 고양이 Scroll Rect Content Transform
    [SerializeField] private RectTransform CatToPlacementContentTr;
    private Transform WorkingCatParentTr;

    [Header("고양이 배치")]
    // 건물마다 일할 수 있는 고양이들의 수가 다르기 때문에 고양이들의 배치가 달라서 프리팹으로 따로 만들었음
    [SerializeField] private List<GameObject> GoldBuildingWorkingCatPlacements = new List<GameObject>();
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
    private int CurSelectedCatIndex = 0;
    private CatPlacementWorkingCatsUI workingCatsUI;

    private const string WORKING_TEXT = "(이)가 일하는 중.";

    private ProductionBuilding productionBuilding;
    private BuildingType BuildingType;
    private List<CatAbilityUI> catAbilityUIs = new List<CatAbilityUI>();

    private CatManager CatManager;
    private GridBuildingSystem GridBuildingSystem;

    protected override void Awake()
    {
        base.Awake();

        CatManager = CatManager.Instance;
        GridBuildingSystem = GridBuildingSystem.Instance;

        WorkingCatParentTr = BuildingImage.transform.parent;
    }

    /// <summary>
    /// 배치창에 보여줄 건물 정보와 그 건물에서 일하고 있는 고양이 정보
    /// </summary>
    /// <param name="catData">고양이 정보</param>
    /// <param name="buildingSprite">건물 이미지</param>
    public void SetBuildingInfo(BuildingType buildingType, in ProductionBuilding building, in CatData[] catData = null)
    {
        productionBuilding = building;
        BuildingType = buildingType;
        BuildingImage.sprite = building.BuildingSprite;
        BuildingNameText.text = building.BuildingName;

        SetWorkingCat();


        if (catData == null)
        {
            WorkText.gameObject.SetActive(false);

            SetCatList();
            return;
        }

        WorkText.gameObject.SetActive(true);
        WorkText.SetText(catData[0].Name + WORKING_TEXT);

        SetCatList();
    }


    /// <summary>
    /// 배치된 고양이 생성
    /// </summary>
    private void SetWorkingCat()
    {
        if (BuildingType == BuildingType.Gold)
            if (CheckCreateWorkingCat<GoldProductionBuilding>() == false)
            {
                Debug.Assert(false, $"{nameof(GoldProductionBuilding)} {nameof(CheckCreateWorkingCat)} failed");
                return;
            }

        if (BuildingType == BuildingType.Energy)
            if (CheckCreateWorkingCat<EnergyProductionBuilding>() == false)
            {
                Debug.Assert(false, $"{nameof(EnergyProductionBuilding)} {nameof(CheckCreateWorkingCat)} failed");
                return;
            }
    }


    // Create Working cat
    private bool CheckCreateWorkingCat<T>() where T : ProductionBuilding
    {
        T building = productionBuilding as T;

        if (building == null) return false;

        // 건물에서 일하는 고양이가 없을 때
        if (building.WorkingCats == null)
        {
            if (BuildingType == BuildingType.Gold)
                workingCatsUI = Instantiate(GoldBuildingWorkingCatPlacements[building.BuildinTypeToInt], WorkingCatParentTr).GetComponent<CatPlacementWorkingCatsUI>();
            else if (BuildingType == BuildingType.Energy)
                workingCatsUI = Instantiate(EnergyBuildingWorkingCatPlacements[building.BuildinTypeToInt], WorkingCatParentTr).GetComponent<CatPlacementWorkingCatsUI>();

            building.WorkingCats = workingCatsUI;
        }
        else
        {
            workingCatsUI = building.WorkingCats;
            workingCatsUI.gameObject.SetActive(true);
        }


        workingCatsUI.MaxDeployableCat = building.MaxDeployableCat;

        return true;
    }


    /// <summary>
    /// 배치할 수 있는 고양이 리스트 생성
    /// </summary>
    private void SetCatList()
    {
        CurSelectedCat = null;

        DestroyChild(CatToPlacementContentTr);

        // 고양이 리스트 출력
        var CatDataList = CatManager.CatList.Select(x => x.catData).ToList();
        var cnt = CatDataList.Count;

        //bool jump;

        for (int index = 0; index < cnt; index++)
        {
            if (AlreadyCatWorking(CatDataList[index])) continue;

            var catToPlacement = Instantiate(CatToPlacementPrefab, CatToPlacementContentTr).GetComponent<CatToPlaceUI>();

            catToPlacement.SetData(CatDataList[index], onclick: PlacementOnClick);
        }
    }

    public bool AlreadyCatWorking(CatData catData)
    {
        if (workingCatsUI.CatDatas.Count == 0) return false;

        CurSelectedCat = workingCatsUI.CatDatas[0];

        for (int i = 0; i < workingCatsUI.CatDatas.Count; i++)
        {
            if (workingCatsUI.CatDatas[i] == catData)
            {
                return true;
            }
        }
         
        return false;
    }

    private void PlacementOnClick(CatToPlaceUI cattoplace, CatData catData)
    {
        // 경고창
        CatPlacementWarning.WarningUISetActive(true);
        CatPlacementWarning.SetWaringData(catData.Name);

        // 경고창 Yes 버튼
        CatPlacementWarning.OnClickYesButton(() => { CreateYesButtonFromCatData(in cattoplace, in catData); });
    }

    /// <summary>
    /// 배치 버튼 눌렀을때 호출할 함수
    /// </summary>
    /// <param name="cattoplace">배치할 수 있는 고양이</param> 
    private void CreateYesButtonFromCatData(in CatToPlaceUI cattoplace, in CatData catData)
    {
        // 건물에서 일하는 고양이가 없을 때
        if (CurSelectedCat == null || workingCatsUI.CatDatas.Count < workingCatsUI.MaxDeployableCat)
        {
            // 배치한 고양이 정보 셋팅

            WorkText.SetText(catData.Name + WORKING_TEXT);

            // 배치했으니까 지워주고
            Destroy(cattoplace.gameObject);

            // 배치한 고양이 스킬 정보 띄워주고
            var ability = Instantiate(AbilityPrefab, workingCatsUI.AbilityParent).GetComponent<CatAbilityUI>();
            ability.SetAbility(catData);
            workingCatsUI.CatAbilitys.Add(ability);

            // 일하고 있는 고양이 UI 정보 셋팅
            workingCatsUI.SetData(workingCatsUI.CatDatas.Count, in catData, call: (catnum) => CurSelectedCatIndex = catnum);
        }
        else
        {
            // 고양이를 바꿔야 하는거임

            // 배치된 고양이와 정보 교체
            workingCatsUI.SetData(CurSelectedCatIndex, in catData);
            cattoplace.SetData(CurSelectedCat);
        }

        CurSelectedCat = catData;

        // 건물에서 일하는 고양이 바꾸기
        BuildingManager.GetGoldProductionBuilding(productionBuilding).OnCatMemberChange(catData, CurSelectedCatIndex);
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

        if (workingCatsUI != null)
        {
            workingCatsUI.gameObject.SetActive(false);
            workingCatsUI = null;
        }
    }
}

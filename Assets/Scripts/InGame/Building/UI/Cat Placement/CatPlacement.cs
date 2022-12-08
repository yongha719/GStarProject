using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CatPlacement : MonoBehaviour
{
    [SerializeField] private GameObject CatPlacementUI;
    [SerializeField] private Image BuildingImage;
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

    IResourceProductionBuilding productionBuilding;
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
    public void SetBuildingInfo(BuildingType buildingType, IResourceProductionBuilding building, CatData[] catData, CatPlacementWorkingCats _workingCats, Sprite buildingSprite)
    {
        productionBuilding = building;
        BuildingType = buildingType;

        SetWorkingCat(building);

        if (catData == null)
        {
            WorkText.gameObject.SetActive(false);
            BuildingImage.sprite = buildingSprite;
            SetCatList();

            return;
        }

        WorkText.gameObject.SetActive(true);
        WorkText.SetText(catData[0].Name + WORKING_TEXT);

        SetCatList();
    }

    private bool Unknown<T>(Building source)
        where T : Building
    {
        var value = source as T;

        if (value == null) return false;

        // TODO: ProductionBuilding 클래스를 만들어서 Gold~, Energy~ 클래스의 부모로 만들고
        // 같이 사용하는 변수들을 ProductionBuilding로 옮기기 (코드의 중복을 줄이기)

        // WorkingCats ( ProductionBuilding.WorkingCats )
        //if (value.WorkingCats == null)
        //{
        //    workingCats = Instantiate(GoldBuildingWorkingCatPlacements[(int)value.buildingType], WorkingCatParentTr).GetComponent<CatPlacementWorkingCats>();
        //    value.WorkingCats = workingCats;
        //}
        //else
        //{
        //    workingCats = value.WorkingCats;
        //    workingCats.gameObject.SetActive(true);
        //}

        workingCats.MaxDeployableCat = value.MaxDeployableCat;

        return true;
    }

    /// <summary>
    /// 배치된 고양이 생성
    /// </summary>
    /// <param name="null">working cat이 null인지 체크</param>
    private void SetWorkingCat(IResourceProductionBuilding Productionbuilding)
    {
        {
            //if (Productionbuilding is GoldProductionBuilding gold)
            //{
            //    gold = Productionbuilding as GoldProductionBuilding;
            //    
            //    if (gold.WorkingCats == null)
            //    {
            //        workingCats = Instantiate(GoldBuildingWorkingCatPlacements[(int)gold.buildingType], WorkingCatParentTr).GetComponent<CatPlacementWorkingCats>();
            //        gold.WorkingCats = workingCats;
            //    }
            //    else
            //    {
            //        workingCats = gold.WorkingCats;
            //        workingCats.gameObject.SetActive(true);
            //    }
            //
            //    workingCats.MaxDeployableCat = gold.MaxDeployableCat;
            //}
            //else if (Productionbuilding is EnergyProductionBuilding energy)
            //{
            //    energy = Productionbuilding as EnergyProductionBuilding;
            //
            //    if (energy.WorkingCats == null)
            //    {
            //        workingCats = Instantiate(GoldBuildingWorkingCatPlacements[(int)energy.buildingType], WorkingCatParentTr).GetComponent<CatPlacementWorkingCats>();
            //        energy.WorkingCats = workingCats;
            //    }
            //    else
            //    {
            //        workingCats = energy.WorkingCats;
            //        workingCats.gameObject.SetActive(true);
            //    }
            //
            //    workingCats.MaxDeployableCat = energy.MaxDeployableCat;
            //}
        }

        // TODO: ProductionBuilding을 클래스로 바꾸자
        //if (!Unknown<GoldProductionBuilding>(Productionbuilding))
        //{
        //  처리
        //  return;
        //}

        //if (!Unknown<EnergyProductionBuilding>(Productionbuilding))
        //{
        //  처리
        //  return;
        //}
    }

    public bool CheckCatWorking(ref List<CatData> ref_cat_datas, in int index)
    {
        if (workingCats.CatDatas.Count <= 0) return false;

        CurSelectedCat = workingCats.CatDatas[0];

        for (int j = 0; j < workingCats.CatDatas.Count; j++)
        {
            if (workingCats.CatDatas[j] == ref_cat_datas[index])
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

            workingCats.SetData(workingCats.CatDatas.Count, catData, call: (catnum) => CurSelectedCatIndex = catnum);
        }
        else
        {
            // 고양이를 바꿔야 하는거임

            // 배치된 고양이와 정보 교체
            workingCats.SetData(CurSelectedCatIndex, catData, null);
            cattoplace.SetData(CurSelectedCat);
        }

        CurSelectedCat = catData;

        // 건물에서 일하는 고양이 바꾸기
        BuildingManager.GetBuilding(productionBuilding).OnCatMemberChange(catData, CurSelectedCatIndex, workingCats);
    }

    private void PlacementOnClick(CatToPlaceUI cattoplace, CatData catData)
    {
        // 경고창
        CatPlacementWarning.gameObject.SetActive(true);
        CatPlacementWarning.SetWaringData(catData.Name);

        // 경고창 Yes 버튼
        CatPlacementWarning.OnClickYesButton(() => { CreateYesButtonFromCatData(cattoplace, catData); });
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
            {
                //jump = false;

                //for (; j < workingCats.CatDatas.Count; j++)
                //{
                //    if (workingCats.CatDatas[j] == CatList[index])
                //    {
                //        jump = true;
                //        break;
                //    }
                //}

                //if (workingCats.CatDatas.Count != 0)
                //    CurSelectedCat = workingCats.CatDatas[0];

                //if (jump) continue;
            }

            if (CheckCatWorking(ref CatList, in index)) continue;

            var catToPlacement = Instantiate(CatToPlacementPrefab, CatToPlacementContentTr).GetComponent<CatToPlaceUI>();

            catToPlacement.SetData(CatList[index],
            // 배치 버튼에 들어갈 이벤트들
            onclick: PlacementOnClick);
        }

        switch (cnt)
        {
            case int i when i > 10:
                break;  
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

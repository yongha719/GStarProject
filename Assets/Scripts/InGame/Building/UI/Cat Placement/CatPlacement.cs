using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine.Assertions.Must;
using System.Reflection;
using UnityEngine.Scripting;

public class CatPlacement : MonoBehaviour
{
    [SerializeField] private GameObject CatPlacementUI;
    [SerializeField] private Image BuildingImage;
    [SerializeField] private TaskText WorkText;
    [SerializeField] private CatPlacementWarning CatPlacementWarning;

    [Header("Parent Transform")]
    // 배치할 수 있는 고양이 Scroll Rect Content Transform
    [SerializeField] private RectTransform CatToPlacementContentTr;
    [SerializeField] private Transform AbilityParentTr;
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
    private CatPlacementWorkingCats workingCat;

    private const string WORKING_TEXT = "(이)가 일하는 중.";

    private CatManager CatManager;
    Building dadada;
    IResourceProductionBuilding productionBuilding;
    BuildingType BuildingType;
    List<CatAbilityUI> catAbilityUIs = new List<CatAbilityUI>();

    private void Awake()
    {
        CatManager = CatManager.Instance;

        WorkingCatParentTr = BuildingImage.transform.parent;
    }

    private void OnEnable()
    {
        // 고양이 리스트 출력
        var CatList = CatManager.CatList.Select(x => x.catData).ToList();
        var cnt = CatList.Count;


        for (int i = 0; i < CatToPlacementContentTr.childCount; i++)
        {
            Destroy(CatToPlacementContentTr.GetChild(i).gameObject);
        }

        bool jump = false;

        print("one");

        for (int i = 0; i < cnt; i++)
        {
            print(workingCat == null);
            if (workingCat != null)
            {

                for (int j = 0; j < workingCat.CatDatas.Count; j++)
                {
                    if (workingCat.CatDatas[i] == CatList[i])
                    {
                        i++;
                        jump = true;
                        break;
                    }
                }

                workingCat.SetData(i, CatList[i],
                        call: (catnum) =>
                        {
                            CurSelectedCat = CatList[i];
                            CurSelectedCatIndex = catnum;
                        });
            }


            if (jump) continue;

            // TODO : 리팩토링
            var catToPlacement = Instantiate(CatToPlacementPrefab, CatToPlacementContentTr).GetComponent<CatToPlace>();

            catToPlacement.SetData(CatList[i],
            onclick: (catData) =>
            {
                // 배치 버튼에 들어갈 이벤트들

                // 경고창
                CatPlacementWarning.gameObject.SetActive(true);
                CatPlacementWarning.SetWaringData(catData.Name);


                // 경고창 Yes 버튼
                CatPlacementWarning.OnClickYesButton(() =>
                {
                    // 건물에서 일하는 고양이가 없을 때
                    if (CurSelectedCat == null)
                    {
                        CurSelectedCat = catData;
                        WorkText.SetText(catData.Name + WORKING_TEXT);

                        Destroy(catToPlacement.gameObject);


                        // 골드 생산 건물 일때
                        if (BuildingType == BuildingType.Gold)
                        {
                            var goldBuilding = CreateWorkingCat<GoldProductionBuilding>(productionBuilding);
                            // 건물에서 일하는 고양이가 바뀌었을때
                            goldBuilding.OnCatMemberChange(catData, CurSelectedCatIndex,
                                action: () =>
                                {

                                });
                        }
                        else if (BuildingType == BuildingType.Energy)
                        {
                            var energyBuilding = CreateWorkingCat<EnergyProductionBuilding>(productionBuilding);
                            // 건물에서 일하는 고양이가 바뀌었을때
                            energyBuilding.OnCatMemberChange(catData, CurSelectedCatIndex,
                                action: () =>
                                {

                                });
                        }


                    }
                    else
                    {
                        // 고양이를 바꿔야 하는거임

                        // 배치된 고양이와 정보 교체
                        workingCat.SetData(catData);
                        catToPlacement.SetData(CurSelectedCat);
                    }
                });
            });
        }
    }

    /// <summary>
    /// 배치창에 보여줄 건물 정보와 그 건물에서 일하고 있는 고양이 정보
    /// null 선 체크후 함수 호출
    /// </summary>
    /// <param name="catData">고양이 정보</param>
    /// <param name="buildingSprite">건물 이미지</param>
    public void SetBuildingInfo(BuildingType buildingType, IResourceProductionBuilding building, CatData[] catData, Sprite buildingSprite)
    {
        print("set");

        productionBuilding = building;
        CreateWorkingCat(building);
        BuildingType = buildingType;

        if (catData == null)
        {
            WorkText.gameObject.SetActive(false);
            BuildingImage.sprite = buildingSprite;

            return;
        }

        int catDataLen = catData.Length;

        catAbilityUIs = new List<CatAbilityUI>();

        bool jump = false;

        for (int index = 0; index < catDataLen; index++)
        {
            for (int j = 0; j < workingCat.CatDatas.Count; j++)
            {
                // 중복이면 생성안함
                if (workingCat.CatDatas[index] == catData[index])
                {
                    jump = true;
                    break;
                }
            }

            if (jump) continue;

            // 스킬 정보 넣어야됨
            var ability = Instantiate(AbilityPrefab, AbilityParentTr).GetComponent<CatAbilityUI>();
            ability.SetAbility(catData[index].AbilitySprite, catData[index].AbilityRating);
            catAbilityUIs.Add(ability);
        }

        WorkText.SetText(catData[0].Name + WORKING_TEXT);


        workingCat.CatAbilitys = catAbilityUIs;

        for (int i = 0; i < catDataLen; i++)
        {
            workingCat.SetData(i, catData[i].CatSprite, call: () =>
            {
                CurSelectedCat = catData[i];

                WorkText.SetText(catData[i].Name + WORKING_TEXT);
            });
        }
    }

    /// <summary>
    /// 배치된 고양이 생성
    /// </summary>
    private void CreateWorkingCat(IResourceProductionBuilding Productionbuilding)
    {
        if (Productionbuilding is GoldProductionBuilding)
        {
            var gold = Productionbuilding as GoldProductionBuilding;

            workingCat = Instantiate(GoldBuildingWorkingCatPlacements[(int)gold.buildingType], WorkingCatParentTr).GetComponent<CatPlacementWorkingCats>();
        }
        else if (Productionbuilding is EnergyProductionBuilding)
        {
            var energy = Productionbuilding as EnergyProductionBuilding;

            workingCat = Instantiate(EnergyBuildingWorkingCatPlacements[(int)energy.buildingType], WorkingCatParentTr).GetComponent<CatPlacementWorkingCats>();
        }
    }

    /// <summary>
    /// 배치된 고양이 생성
    /// </summary>
    private T CreateWorkingCat<T>(IResourceProductionBuilding Productionbuilding) where T : Building, IResourceProductionBuilding
    {
        T building = null;

        if (typeof(T) == typeof(GoldProductionBuilding))
        {
            var gold = Productionbuilding as GoldProductionBuilding;

            workingCat = Instantiate(GoldBuildingWorkingCatPlacements[(int)gold.buildingType], WorkingCatParentTr).GetComponent<CatPlacementWorkingCats>();
            building = gold as T;
        }
        else if (typeof(T) == typeof(EnergyProductionBuilding))
        {
            var energy = Productionbuilding as EnergyProductionBuilding;

            workingCat = Instantiate(EnergyBuildingWorkingCatPlacements[(int)energy.buildingType], WorkingCatParentTr).GetComponent<CatPlacementWorkingCats>();
            building = energy as T;
        }
        else
        {
            print($"{nameof(CreateWorkingCat)} 야야 이거 뭔가 잘못됐어");
        }

        return building;
    }

    private void Init()
    {
        for (int i = 0; i < AbilityParentTr.childCount; i++)
        {
            Destroy(AbilityParentTr.GetChild(i).gameObject);
        }
    }

    private void OnDisable()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

public class CatPlacement : MonoBehaviour
{
    [SerializeField] private GameObject CatPlacementUI;
    [SerializeField] private Image BuildingImage;
    [SerializeField] private Image AbilityImage;
    [SerializeField] private TaskText WorkText;

    // 배치할 수 있는 고양이 Scroll Rect Content Transform
    [SerializeField] private Transform CatToPlacementContentTr;
    [SerializeField] private Transform AbilityParentTr;
    private Transform WorkingCatParentTr;

    // 건물마다 일할 수 있는 고양이들의 수가 다르기 때문에 고양이들의 배치가 달라서 프리팹으로 따로 만들었음
    [Tooltip("건물당 일하는 고양이들의 배치")]
    [SerializeField] private List<GameObject> GoldBuildingWorkingCatPlacements = new List<GameObject>();
    [SerializeField] private List<GameObject> EnergyBuildingWorkingCatPlacements = new List<GameObject>();
    [SerializeField] private GameObject CatToPlacementPrefab;
    [SerializeField] private GameObject AbilityPrefab;

    private List<Cat> cats = new List<Cat>();

    private const string WORKING_TEXT = "(이)가 일하는 중.";

    private CatManager CatManager;

    private void Awake()
    {
        CatManager = CatManager.Instance;

        WorkingCatParentTr = BuildingImage.transform.parent;
    }

    private void OnEnable()
    {
        // 고양이 리스트 출력
        //var CatList = CatManager.CatList;
        //var cnt = CatList.Count;

        //for (int i = 0; i < cnt; i++)
        //{
        //    var catToPlacement = Instantiate(CatToPlacementPrefab, CatToPlacementContentTr).GetComponent<CatToPlace>();
        //    catToPlacement.SetData(
        //        catSprite: CatList[i].CatSprite,
        //        catName: CatList[i].Name,
        //        abilitySprite: CatList[i].AbilitySprite,
        //        abilityRating: CatList[i].AbilityRating,
        //        onclick: () =>
        //        {
        //            // 배치 버튼에 들어갈 이벤트들
        //        });
        //}

        var catDatas = new CatData[1];
        catDatas[0] = new CatData();
        catDatas[0].Name = "깜펭바보";

        SetBuildingInfo(BuildingType.GoldBuildingType, 0, catDatas, BuildingImage.sprite);
    }

    /// <summary>
    /// 고양이 배치 창 UI SetActive
    /// </summary>
    public void UISetActive(bool value) => CatPlacementUI.SetActive(value);

    /// <summary>
    /// 배치창에 보여줄 건물 정보와 그 건물에서 일하고 있는 고양이 정보
    /// </summary>
    /// <param name="catData">고양이 정보</param>
    /// <param name="buildingSprite">건물 이미지</param>
    /// 
    /// null 선 체크후 함수 실행
    public void SetBuildingInfo(System.Type buildingType, int buildingNum, CatData[] catData, Sprite buildingSprite)
    {
        BuildingImage.sprite = buildingSprite;

        if (catData == null)
        {
            WorkText.gameObject.SetActive(false);

            return;
        }

        int catDataLen = catData.Length;

        List<CatAbilityUI> catAbilityUIs = new List<CatAbilityUI>();

        for (int index = 0; index < catDataLen; index++)
        {
            // 스킬 정보 넣어야됨
            var ability = Instantiate(AbilityPrefab, AbilityParentTr).GetComponent<CatAbilityUI>();
            ability.SetAbility(catData[index].AbilitySprite, catData[index].AbilityRating);
            catAbilityUIs.Add(ability);
        }


        WorkText.SetText(catData[0].Name + WORKING_TEXT);

        CatPlacementWorkingCats workingCat = null;


        if (buildingType == typeof(GoldBuildingType))
        {
            workingCat = Instantiate(GoldBuildingWorkingCatPlacements[buildingNum], WorkingCatParentTr).GetComponent<CatPlacementWorkingCats>();

            for (int i = 0; i < catDataLen; i++)
            {
                workingCat.SetData(i, catData[i].CatSprite, call: () =>
                {
                    
                });
            }

            workingCat.CatAbilitys = catAbilityUIs;
        }
        else if (buildingType == typeof(EnergyBuildingType))
        {
            workingCat = Instantiate(EnergyBuildingWorkingCatPlacements[buildingNum], WorkingCatParentTr).GetComponent<CatPlacementWorkingCats>();

            workingCat.CatAbilitys = catAbilityUIs;
        }
        else
        {
            throw new System.Exception($"이거 뭐야\n{nameof(CatPlacement)} 건물 종류 없어 이 색기야");
        }
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

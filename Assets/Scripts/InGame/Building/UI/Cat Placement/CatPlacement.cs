using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatPlacement : MonoBehaviour
{
    [SerializeField] private GameObject CatPlacementUI;
    [SerializeField] private Image BuildingImage;
    [SerializeField] private Image AbilityImage;
    [SerializeField] private TextMeshProUGUI WorkText;

    // 배치할 수 있는 고양이 Scroll Rect Content Transform
    [SerializeField] private Transform CatToPlacementContentTr;
    [SerializeField] private Transform AbilityParentTr;


    // 건물마다 일할 수 있는 고양이들의 수가 다르기 때문에 고양이들의 배치가 달라서 프리팹으로 따로 만들었음
    [Tooltip("건물당 일하는 고양이들의 배치")]
    [SerializeField] private List<GameObject> WorkingCatPlacements = new List<GameObject>();
    [SerializeField] private GameObject CatToPlacementPrefab;
    [SerializeField] private GameObject AbilityPrefab;

    private List<Cat> cats = new List<Cat>();

    private const string WORKING_TEXT = "(이)가 일하는 중.";

    private CatManager CatManager;

    private void Awake()
    {
        CatManager = CatManager.Instance;
    }

    private void OnEnable()
    {
        // 고양이 리스트 출력
        var CatList = CatManager.CatList;
        var cnt = CatList.Count;

        for (int i = 0; i < cnt; i++)
        {
            var catToPlacement = Instantiate(CatToPlacementPrefab, CatToPlacementContentTr).GetComponent<CatToPlace>();
            catToPlacement.SetData(
                catSprite: CatList[i].CatSprite,
                catName: CatList[i].Name,
                abilitySprite: CatList[i].AbilitySprite,
                abilityRating: CatList[i].AbilityRating,
                onclick: () =>
                {
                    // 배치 버튼에 들어갈 이벤트들
                });
        }
    }

    /// <summary>
    /// 고양이 배치 창 UI SetActive
    /// </summary>
    public void UISetActive(bool value) => CatPlacementUI.SetActive(value);

    /// <summary>
    /// 배치창에 보여줄 건물 정보와 고양이의 정보
    /// </summary>
    /// <param name="catData">고양이 정보</param>
    /// <param name="buildingSprite">건물 이미지</param>
    public void SetBuildingInfo(CatData[] catData, Sprite buildingSprite)
    {
        int catDataLen = catData.Length;

        for (int index = 0; index < catDataLen; index++)
        {
            AbilityImage.sprite = catData[index].AbilitySprite;

            // 스킬 정보 넣어야됨
            var ability = Instantiate(AbilityPrefab, AbilityParentTr).GetComponent<CatAbilityUI>();
            ability.SetAbility(catData[index].AbilitySprite, catData[index].AbilityRating);
        }

        WorkText.text = catData[0].Name + WORKING_TEXT;
        BuildingImage.sprite = buildingSprite;
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

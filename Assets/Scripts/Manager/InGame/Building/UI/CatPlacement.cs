using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatPlacement : MonoBehaviour
{
    [SerializeField] private GameObject CatPlacementUI;
    [SerializeField] private Image AbilityImage;
    [SerializeField] private Image BuildingImage;
    [SerializeField] private List<GameObject> RatingStar = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI WorkText;

    [SerializeField] private Transform Content;

    private List<Cat> cats = new List<Cat>();

    private const string WORKING = "(가)이 일하는 중.";

    private CatManager CatManager;

    private void Awake()
    {
        CatManager = CatManager.Instance;
    }
    private void OnEnable()
    {
        // 고양이 리스트 출력
        var cnt = CatManager.CatList.Count;

        for(int index = 0; index < cnt; index++)
        {
            
        }
    }

    void Start()
    {

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
    public void SetBuildingInfo(CatData catData, Sprite buildingSprite)
    {
        AbilityImage.sprite = catData.AbilityImage;
        for (int i = 0; i < catData.AbilityRating; i++)
        {
            RatingStar[i].SetActive(true);
        }
        WorkText.text = catData.Name + WORKING;

        BuildingImage.sprite = buildingSprite;
    }

    private void OnDisable()
    {
        for (int i = 0; i < 3; i++)
        {
            RatingStar[i].SetActive(false);
        }
    }
}

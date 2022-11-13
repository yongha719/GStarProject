using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ViliageInfoUI : MonoBehaviour
{
    [Header("주민")]
    [SerializeField] private RectTransform CatsContent;
    [SerializeField] private GameObject CatInfoPrefab;

    [Header("재화")]
    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI EnergyText;
    [SerializeField] private TextMeshProUGUI IceText;
    [SerializeField] private TextMeshProUGUI LevelUpCostText;

    [Header("레벨")]
    [SerializeField] private TextMeshProUGUI CurLevelText;
    [SerializeField] private TextMeshProUGUI NextLevelText;
    [SerializeField] private TextMeshProUGUI CurAreaText;
    [SerializeField] private TextMeshProUGUI NextAreaText;

    private CatManager CatManager;
    private VillageHall VillageHall;
    private GridBuildingSystem GridBuildingSystem;
    void Awake()
    {
        CatManager = CatManager.Instance;
        GridBuildingSystem = GridBuildingSystem.Instance;
        VillageHall = FindObjectOfType<VillageHall>();
    }

    private void OnEnable()
    {
        // 레벨업 텍스트
        CurLevelText.text = $"Lv. {VillageHall.Level}";
        CurAreaText.text = $"{GridBuildingSystem.ViliageAreaSize.x} * {GridBuildingSystem.ViliageAreaSize.y}";
        NextLevelText.text = $"Lv. {VillageHall.Level + 1}";
        NextAreaText.text = $"{GridBuildingSystem.ViliageAreaSize.x + 2} * {GridBuildingSystem.ViliageAreaSize.y + 2}";

        LevelUpCostText.text = VillageHall.GetLevelUpCost;

        if (CatManager.CatList != null)
        {
            for (int i = 0; i < CatsContent.childCount; i++)
                Destroy(CatsContent.GetChild(i).gameObject);

            var CatList = CatManager.CatList;
            var cnt = CatList.Count;

            for (int i = 0; i < cnt; i++)
            {
                var catInfo = Instantiate(CatInfoPrefab, CatsContent).GetComponent<CatInfoUI>();

                catInfo.SetData(CatList[i],
                    call: () =>
                    {
                        CatManager.RemoveCat(CatList[i]);
                    });
            }
        }
    }


    void Update()
    {

    }
}

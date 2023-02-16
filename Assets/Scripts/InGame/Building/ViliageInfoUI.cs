using TMPro;
using UnityEngine;

public class ViliageInfoUI : UIPopup
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

    private GameManager GameManager;
    private CatManager CatManager;
    private VillageHall VillageHall;
    private GridBuildingSystem GridBuildingSystem;
    protected override void Awake()
    {
        base.Awake();   

        CatManager = CatManager.Instance;
        GridBuildingSystem = GridBuildingSystem.Instance;
        GameManager = GameManager.Instance;

        VillageHall = FindObjectOfType<VillageHall>();
    }

    private void Start()
    {
        GameManager.OnCoinValueChanged += () =>
        {
            GoldText.text = GameManager._coin.returnStr();
        };

        GameManager.OnEnergyValueChanged += () =>
        {
            EnergyText.text = GameManager._energy.returnStr();
        };

        GameManager.OnIceValueChanged += () =>
        {
            IceText.text = GameManager._ice.returnStr();
        };

        GoldText.text = GameManager._coin.returnStr();
        EnergyText.text = GameManager._energy.returnStr();
        IceText.text = GameManager._ice.returnStr();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // 레벨업 텍스트
        CurLevelText.text = $"Lv. {VillageHall.Level}";
        CurAreaText.text = $"{GridBuildingSystem.ViliageAreaSize.x} * {GridBuildingSystem.ViliageAreaSize.y}";
        NextLevelText.text = $"Lv. {VillageHall.Level + 1}";
        NextAreaText.text = $"{GridBuildingSystem.ViliageAreaSize.x + 2} * {GridBuildingSystem.ViliageAreaSize.y + 2}";

        LevelUpCostText.text = VillageHall.GetLevelUpCost;

        if (CatManager.CatList != null)
        {
            var CatList = CatManager.CatList;

            for (int i = 0; i < CatList.Count; i++)
            {
                var catInfo = Instantiate(CatInfoPrefab, CatsContent).GetComponent<CatInfoUI>();

                catInfo.SetData(CatList[i],
                    call: () =>
                    {
                        Destroy(catInfo.gameObject);
                    });
            }
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < CatsContent.childCount; i++)
            Destroy(CatsContent.GetChild(i).gameObject);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class VillageHallUI : UIPopup
{
    [SerializeField] private Button LevelUpButton;

    [Header("경고창")]
    [SerializeField] private GameObject LevelUpWarning;
    [SerializeField] private GameObject CatKickWarning;
    [SerializeField] private GameObject NotEnoughGold;

    private VillageHall VillageHall;
    private GameManager GameManager;
    private CatManager CatManager;
    private GridBuildingSystem GridBuildingSystem;

    protected override void Awake()
    {
        base.Awake();

        VillageHall = FindObjectOfType<VillageHall>();

        GameManager = GameManager.Instance;
        CatManager = CatManager.Instance;
        GridBuildingSystem = GridBuildingSystem.Instance;
    }

    void Start()
    {
        LevelUpButton.onClick.AddListener(() =>
        {
            if (GameManager._coin >= VillageHall.GetLevelUpCost.returnValue())
            {
                VillageHall.LevelUp();
            }
        });
    }

}

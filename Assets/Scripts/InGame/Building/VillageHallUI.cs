using UnityEngine;
using UnityEngine.UI;

public class VillageHallUI : MonoBehaviour
{
    [SerializeField] private Button LevelUpButton;

    [Header("경고창")]
    [SerializeField] private GameObject Warning;
    [SerializeField] private GameObject NotEnoughGold;

    private VillageHall VillageHall;
    private GameManager GameManager;
    private CatManager CatManager;
    private GridBuildingSystem GridBuildingSystem;

    private void Awake()
    {
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
                Warning.SetActive(true);
            }
            else
            {
                NotEnoughGold.SetActive(true);
            }
        });
    }

}

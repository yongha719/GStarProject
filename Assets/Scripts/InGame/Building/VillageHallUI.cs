using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VillageHallUI : MonoBehaviour
{
    private VillageHall VillageHall;

    [Header("재화")]
    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI EnergyText;
    [SerializeField] private TextMeshProUGUI IceText;
    [SerializeField] private Button LevelUpButton;
    [SerializeField] private TextMeshProUGUI LevelUpCostText;

    [Header("레벨")]
    [SerializeField] private TextMeshProUGUI CurLevelText;
    [SerializeField] private TextMeshProUGUI NextLevelText;
    [SerializeField] private TextMeshProUGUI CurAreaText;
    [SerializeField] private TextMeshProUGUI NextAreaText;

    [Header("경고창")]
    [SerializeField] private GameObject Warning;
    [SerializeField] private GameObject NotEnoughGold;

    private GameManager GameManager;
    private CatManager CatManager;

    private void Awake()
    {
        VillageHall = FindObjectOfType<VillageHall>();

        GameManager = GameManager.Instance;
        CatManager = CatManager.Instance;
    }
    private void OnEnable()
    {
        LevelUpCostText.text = VillageHall.GetLevelUpCost;

        CurLevelText.text = $"Lv. {VillageHall.Level}";
        CurAreaText.text = $"{VillageHall.CurAreaSize} * {VillageHall.CurAreaSize}";
        NextLevelText.text = $"Lv. {VillageHall.Level + 1}";
        NextAreaText.text = $"{VillageHall.CurAreaSize + 2} * {VillageHall.CurAreaSize + 2}";
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

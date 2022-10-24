using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Runtime.ConstrainedExecution;

public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;

    public int Rating;
    public GoldBuildingType buildingType;

    #region Gold
    [Header("Gold")]
    [SerializeField] private Button CollectMoneyButton;
    [SerializeField] private string DefaultGold;
    [SerializeField] private float DefaultGoldChargingTime;
    [SerializeField] private float IncreasePerLevelUp;

    public string ProductionGold
    {
        get
        {
            var gold = DefaultGold.returnValue();

            for (var i = 0; i < Rating; i++)
            {
                gold *= IncreasePerLevelUp;
            }

            return gold.returnStr();
        }
    }


    [Tooltip("기본 건설 비용")] public string DefaultConstructionCost;
    public string ConstructionCost
    {
        get => (DefaultConstructionCost.returnValue() * (BuildingManager.s_GoldBuildings[buildingType] * 3)).returnStr();
    }

    private bool didGetMoney;

    private WaitForSeconds waitGoldChargingTime;

    #endregion

    #region Deploying
    private bool isDeploying;
    public bool IsDeploying
    {
        get
        {
            return isDeploying;
        }

        set
        {
            isDeploying = value;

            if (isDeploying)
            {
                SpriteRenderer.color = new Color(1, 1, 1, 0.5f);
                DeplyingObject.SetActive(true);
                CollectMoneyButton.gameObject.SetActive(false);
            }
            else
            {
                SpriteRenderer.color = Color.white;
                DeplyingObject.SetActive(false);
                CollectMoneyButton.gameObject.SetActive(true);

            }
        }
    }

    [HideInInspector] public bool FirstTimeInstallation;

    [HideInInspector] public BuildingInfo BuildingInfo;


    [Header("Deploying")]
    [Tooltip("배치 가능한 고양이 수")] public int MaxDeployableCat;

    private GameObject BuildingSprte;
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private TextMeshProUGUI ConstructionGoldText;

    [Space(5f)]
    [SerializeField] private GameObject DeplyingObject;
    [SerializeField] private Button InstallationButton;
    [SerializeField] private Button DemolitionButton;
    [SerializeField] private Button RotateButton;

    #endregion

    private GridBuildingSystem GridBuildingSystem;

    private void Start()
    {
        GridBuildingSystem = GridBuildingSystem.Instance;

        waitGoldChargingTime = new WaitForSeconds(DefaultGoldChargingTime);

        BuildingSprte = SpriteRenderer.gameObject;

        CollectMoneyButton.onClick.AddListener(() =>
        {
            didGetMoney = true;
        });

        InstallationButton.onClick.AddListener(() =>
        {
            GridBuildingSystem.Place();

        });

        DemolitionButton.onClick.AddListener(() =>
        {
            GridBuildingSystem.BuildingClear();
            BuildingInfo.BuildingInstalltionUI.SetActive(true);
            Destroy(gameObject);
        });

        RotateButton.onClick.AddListener(() =>
        {
            SpriteRenderer.flipX = !SpriteRenderer.flipX;
        });


    }

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        return GridBuildingSystem.CanTakeArea(areaTemp);
    }


    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;

        GridBuildingSystem.TakeArea(areaTemp);

        IsDeploying = false;

        if (FirstTimeInstallation)
        {
            FirstTimeInstallation = false;
            StartCoroutine(BuildingInstalltionEffect());
        }

        StartCoroutine(CChargeMoney());
    }

    IEnumerator CChargeMoney()
    {

        while (true)
        {
            yield return waitGoldChargingTime;

            yield return StartCoroutine(CWaitClick());
        }
    }

    IEnumerator CWaitClick()
    {
        while (true)
        {
            if (didGetMoney)
            {
                GameManager.Instance._coin += CalculatorManager.returnValue(DefaultGold);
                didGetMoney = false;
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator BuildingInstalltionEffect()
    {
        BuildingSprte.transform.localScale = new Vector3(0.03f, 0.03f, 1f);
        yield return BuildingSprte.transform.DOScale(new Vector3(0.1f, 0.1f, 1f), 0.4f).WaitForCompletion();

        ConstructionGoldText.gameObject.SetActive(true);
        ConstructionGoldText.rectTransform.DOAnchorPosY(200, 1);
        yield return ConstructionGoldText.DOFade(0f, 0.7f).WaitForCompletion();

        ConstructionGoldText.gameObject.SetActive(true);
        GridBuildingSystem.InitializeWithBuilding(BuildingInfo.BuildingPrefab);

    }
}

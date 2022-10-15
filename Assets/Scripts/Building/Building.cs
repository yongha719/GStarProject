using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;

    #region Gold

    [SerializeField] private Button CollectMoneyButton;
    public float GoldChargingTime;
    private bool didGetMoney;

    private WaitForSeconds waitGoldChargingTime;

    #endregion

    #region Deploying
    [Header("Deploying")]
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
                DeplyingObject.SetActive(true);
                CollectMoneyButton.gameObject.SetActive(false);
            }
            else
            {
                DeplyingObject.SetActive(false);
                CollectMoneyButton.gameObject.SetActive(true);

            }
        }
    }
    public bool FirstTimeInstallation;

    public BuildingInfo BuildingInfo;


    [SerializeField] private GameObject BuildingSprte;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshProUGUI NotEnoughGoldUI;

    [Space(10f)]
    [SerializeField] private GameObject DeplyingObject;
    [SerializeField] private Button InstallationButton;
    [SerializeField] private Button DemolitionButton;
    [SerializeField] private Button RotateButton;

    #endregion

    private GridBuildingSystem GridBuildingSystem;

    private void Start()
    {
        print("start");

        GridBuildingSystem = GridBuildingSystem.Instance;

        waitGoldChargingTime = new WaitForSeconds(GoldChargingTime);

        BuildingSprte = spriteRenderer.gameObject;

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
            spriteRenderer.flipX = !spriteRenderer.flipX;
        });

        if (FirstTimeInstallation)
        {
            FirstTimeInstallation = false;
            StartCoroutine(BuildingInstalltionEffect());
        }
        
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

        NotEnoughGoldUI.gameObject.SetActive(true);
        NotEnoughGoldUI.rectTransform.DOAnchorPosY(200, 1);
        yield return NotEnoughGoldUI.DOFade(0f, 0.7f).WaitForCompletion();

        NotEnoughGoldUI.gameObject.SetActive(true);
    }
}

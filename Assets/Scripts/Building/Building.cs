using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                CollectMoneyButton.gameObject.SetActive(false);
            }
            else
            {
                CollectMoneyButton.gameObject.SetActive(true);

            }
        }
    }

    [SerializeField] private Button InstallationButton;
    [SerializeField] private Button DemolitionButton;
    [SerializeField] private Button RotateButton;

    #endregion

    private GridBuildingSystem GridBuildingSystem;

    private void Start()
    {
        GridBuildingSystem = GridBuildingSystem.Instance;

        waitGoldChargingTime = new WaitForSeconds(GoldChargingTime);

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
            Destroy(gameObject);
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

        IsDeploying = true;
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
                yield break;
            }

            yield return null;
        }
    }
}

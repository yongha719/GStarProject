using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;

    [SerializeField] private Button CollectMoneyButton;
    public float GoldChargingTime; 
    private bool didGetMoney;

    private WaitForSeconds waitGoldChargingTime;


    private void Start()
    {
        waitGoldChargingTime = new WaitForSeconds(GoldChargingTime);

        CollectMoneyButton.onClick.AddListener(() =>
        {
            didGetMoney = true;
        });
    }


    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.Instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        return GridBuildingSystem.Instance.CanTakeArea(areaTemp);
    }

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.Instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;

        GridBuildingSystem.Instance.TakeArea(areaTemp);


        StartCoroutine(CChargeMoney());
    }

    IEnumerator CChargeMoney()
    {

        while (true)
        {
            yield return waitGoldChargingTime;

            CollectMoneyButton.gameObject.SetActive(true);

            yield return StartCoroutine(CWaitClick());
        }
    }

    IEnumerator CWaitClick()
    {
        var wait = new WaitForEndOfFrame();

        while (true)
        {
            if (didGetMoney)
            {
                yield break;
            }
            yield return wait;
        }
    }
}

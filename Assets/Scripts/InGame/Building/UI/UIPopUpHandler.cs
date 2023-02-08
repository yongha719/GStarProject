using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using System.Runtime.InteropServices.WindowsRuntime;

public enum UIPopupType
{
    PlacingBuilding,
}

public enum UIWarningPopupType
{
    GoldWarning,
    PlacingBuildingWarning,
    PlacingCatWarning,
}

public class UIPopUpHandler : Singleton<UIPopUpHandler>
{
    public GameObject BuildingPlacingPopup;
    private Dictionary<UIPopupType, GameObject> UIPopupDic = new Dictionary<UIPopupType, GameObject>();
    private List<GameObject> UIPopupList= new List<GameObject>();
    private Dictionary<UIWarningPopupType, GameObject> UIWarningPopupDic = new Dictionary<UIWarningPopupType, GameObject>();
    private List<GameObject> UIWarningPopupList = new List<GameObject>();

    private void Start()
    {
        var UIPopups = Resources.LoadAll("UIPopup_Prefabs") as GameObject[];
        var UIWarningPopups = Resources.LoadAll("UIWarningPopup_Prefabs") as GameObject[];

        if (UIPopups == null || UIWarningPopups == null)
        {
            Debug.Assert(false, "야야 프리팹이 하나도 없대잖아 뒤질래?");
            return;
        }

        for (int i = 0; i < UIPopups.Length; ++i)
        {
            UIPopupDic.Add((UIPopupType)i, UIPopups[i]);
        }

        for(int i = 0; i < UIWarningPopups.Length; ++i)
        {
            UIWarningPopupDic.Add((UIWarningPopupType)i, UIWarningPopups[i]);
        }
    }

    public void OnUIPopUp(UIPopupType UIPopupType)
    {
        GameObject UIPopup = UIPopupDic[UIPopupType];

        if (UIPopup == null)
        {
            Debug.Assert(false, "야야 없잖아 이거 프리팹 만들긴했어? 폴더 봐봐 이 친구야");
            return;
        }

        if (UIPopupList.Contains(UIPopup) && UIPopup.activeSelf == false)
        {
            UIPopup.SetActive(true);
        }
        else
        {
            Instantiate(UIPopup, Vector3.zero, Quaternion.identity, transform);
        }

    }

    public void OnUIWarningPopUp(UIWarningPopupType UIWarningPopupType)
    {
        GameObject UIWarningPopup = UIWarningPopupDic[UIWarningPopupType];

        if (UIWarningPopup == null)
        {
            Debug.Assert(false, "야야 없잖아 이거 프리팹 만들긴했어? 폴더 봐봐 이 친구야");
            return;
        }

        if (UIWarningPopupList.Contains(UIWarningPopup) && UIWarningPopup.activeSelf == false)
        {
            UIWarningPopup.SetActive(true);
        }
        else
        {
            Instantiate(UIWarningPopup, Vector3.zero, Quaternion.identity, transform);
        }

    }

    public void UIDisable(GameObject Background, RectTransform rect)
    {
        StartCoroutine(DisableCoroutine(Background, rect));
    }

    private IEnumerator DisableCoroutine(GameObject Background, RectTransform rect)
    {
        rect.DOScale(0f, 0.3f);
        yield return new WaitForSeconds(0.2f);
        Background.SetActive(false);
    }
}

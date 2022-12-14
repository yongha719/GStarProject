using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatInfoUI : MonoBehaviour
{
    [SerializeField] private Image CatImage;
    [SerializeField] private TextMeshProUGUI CatNameText;
    [SerializeField] private TaskText CatStateText;
    [SerializeField] private Button ExportButton;

    const string WORKING_TEXT = "에서 작업중.";
    const string RESTING_TEXT = "에서 휴식중.";

    /// <param name="call">고양이 내보내기 버튼 클릭 이벤트</param>
    public void SetData(Cat cat, Action call)
    {
        CatImage.sprite = cat.catData.CatSprite;
        CatNameText.text = cat.catData.Name;

        switch (cat.CatState)
        {
            case CatState.NotProducting:
                CatStateText.SetText();
                break;
            case CatState.Working:
                CatStateText.SetText(cat.BuildingName + WORKING_TEXT);
                break;
            case CatState.Resting:
                CatStateText.SetText(cat.BuildingName + RESTING_TEXT);
                break;
            default:
                break;
        }

        ExportButton?.onClick.AddListener(() =>
        {
            call();
            CatManager.Instance.RemoveCat(cat);
        });
    }
}

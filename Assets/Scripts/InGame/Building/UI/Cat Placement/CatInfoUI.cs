using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatInfoUI : MonoBehaviour
{
    [SerializeField] private Image CatImage;
    [SerializeField] private TextMeshProUGUI CatNameText;
    [SerializeField] private TextMeshProUGUI CatStateText;
    [SerializeField] private Button ExportButton;

    const string WORKING_TEXT = "에서 작업중.";
    const string RESTING_TEXT = "에서 휴식중.";

    /// <param name="call">고양이 내보내기 버튼 클릭 이벤트</param>
    public void SetData(Cat cat, Action call)
    {
        CatImage.sprite = cat.catData.CatSprite;
        CatNameText.text = cat.catData.Name;

        if (cat.CatState == CatState.Working)
        {
            CatStateText.text = cat.BuildingName + WORKING_TEXT;
        }
        else
        {
            CatStateText.text = cat.BuildingName + RESTING_TEXT;
        }

        ExportButton.onClick.AddListener(() =>
        {
            call();
        });
    }
}

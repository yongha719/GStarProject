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

    const string WORKING_TEXT = "에서 작업중.";
    const string RESTING_TEXT = "에서 휴식중.";

    public void SetData(Sprite catSprite, string catName, CatState catState, string buildingName)
    {
        CatImage.sprite = catSprite;
        CatNameText.text = catName;

        if (catState == CatState.Working)
        {
            CatStateText.text = buildingName + WORKING_TEXT;
        }
        else
        {
            CatStateText.text = buildingName + RESTING_TEXT;
        }
    }
}

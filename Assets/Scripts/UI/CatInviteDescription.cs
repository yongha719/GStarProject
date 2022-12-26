using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CatInviteDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    private Image img;
    private Vector2 originalImgSize;
    private void Start()
    {
        img = GetComponent<Image>();
        originalImgSize = img.rectTransform.sizeDelta;
    }
    public void TextBallonActive(bool inOn)
    {
        img.rectTransform.DOSizeDelta(inOn ? originalImgSize : Vector2.zero, 0.3f);
    }

    public void SkillInfoApply(string[] desc, int index)
    {
        text.text = $"{desc[0]} {GetRatingCount(index)}{desc[1]}";
    }

    private int GetRatingCount(int index) => Mathf.Clamp(index, 1, 3) switch
    {
        1 => 10,
        2 => 15,
        3 => 20,
        _ => throw new System.Exception("이게 뭐야")
    };
}

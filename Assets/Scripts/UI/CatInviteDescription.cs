using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CatInviteDescription : MonoBehaviour
{
    [SerializeField]
    [Tooltip
        ("00. Fishing,                // 낚시\n" +
        "01. Mining,                 // 광질\n" +
        "02. Axing,                  // 도끼질\n" +
        "03. Farming,                // 농사\n" +
        "04. Kiln,                   // 가마질\n" +
        "05. Knitting,               // 뜨개질\n" +
        "06. Boiling,                // 끓이기\n" +
        "07. GeneratorOperating,     // 발전기\n")
        ]
    private string[] Desription;

    [SerializeField]
    private TextMeshProUGUI text;
    public void TextBallonActive(bool inOn)
    {
        transform.DOScale(inOn ? 1 : 0, 1);
    }

    private void SkillInfoApply(GoldAbilityType type, int index)
    {
        string[] desc = Desription[(int)type].Split('@');
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

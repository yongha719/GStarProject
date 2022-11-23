using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CatInviteDescription : MonoBehaviour
{
    [SerializeField]
    [Tooltip
        ("01. Fishing,                // 낚시\n" +
        "02. Mining,                 // 광질\n" +
        "03. Axing,                  // 도끼질\n" +
        "04. Farming,                // 농사\n" +
        "05. Kiln,                   // 가마질\n" +
        "06. Knitting,               // 뜨개질 \n" +
        "07. Boiling,                // 끓이기\n" +
        "08. GeneratorOperating,     // 발전기\n")
        ]
    private string[] Desription;
    public void TextBallonActive(bool inOn)
    {
        transform.DOScale(inOn ? 1 : 0, 1);
    }

    private void SkillInfoApply()
    {

    }
}

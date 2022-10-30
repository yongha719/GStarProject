using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CatTaskText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TaskText;

    private WaitForSeconds wait = new WaitForSeconds(0.5f);
    private string period = ".";

    private string OriginalText;

    private void Awake()
    {
        TaskText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        OriginalText = TaskText.text;

        StartCoroutine(TextChange());
    }

    // 텍스트 뒤에 ... 이 바뀌는 연출
    IEnumerator TextChange()
    {

        while (true)
        {
            yield return wait;

            if (period.Length == 3)
            {
                period = ".";
                TaskText.text = OriginalText;
            }
            else
            {
                TaskText.text = OriginalText + period;
                period += ".";
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TaskText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Tasktext;

    private WaitForSeconds wait = new WaitForSeconds(0.5f);
    private string period = ".";

    private string OriginalText;

    private void Awake()
    {
        Tasktext = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        OriginalText = Tasktext.text;

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
                Tasktext.text = OriginalText;
            }
            else
            {
                Tasktext.text = OriginalText + period;
                period += ".";
            }
        }
    }
}

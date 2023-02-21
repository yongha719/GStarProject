using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TaskText : MonoBehaviour
{
    private TextMeshProUGUI Tasktext;

    private WaitForSeconds wait = new WaitForSeconds(0.5f);
    private string period = ".";

    private string OriginalText;

    IEnumerator TaskTextCoroutine;

    private void Awake()
    {
        Tasktext = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (TaskTextCoroutine != null)
            StartCoroutine(TaskTextCoroutine);
    }

    public void SetText(string text = null)
    {
        if (text == null)
        {
            TaskTextCoroutine = null;
            if (Tasktext != null)
                Tasktext.text = "";

            return;
        }

        OriginalText = text;
        TaskTextCoroutine = TextChange();
    }

    // 텍스트 뒤에 ... 이 바뀌는 연출
    IEnumerator TextChange()
    {
        while (true)
        {
            yield return wait;

            if (period.Length != 3)
            {
                Tasktext.text = OriginalText + period;
                period += ".";
            }
            else
            {
                period = ".";
                Tasktext.text = OriginalText;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class TaskText : MonoBehaviour
{
    private TextMeshProUGUI Tasktext;

    private WaitForSeconds wait = new WaitForSeconds(0.5f);
    private StringBuilder stringBuilder = new StringBuilder();
    private const char PERIOD = '.';

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
            Tasktext.text = string.Empty;

            return;
        }

        OriginalText = text;
        TaskTextCoroutine = TextChange();
    }

    // 텍스트 뒤에 ... 이 바뀌는 연출
    IEnumerator TextChange()
    {
        stringBuilder.Clear();
        stringBuilder.Append(OriginalText);

        while (true)
        {
            yield return wait;

            if (stringBuilder.Length == OriginalText.Length + 3)
            {
                stringBuilder.Clear();
                stringBuilder.Append(OriginalText).Append(PERIOD);
            }
            else
            {
                stringBuilder.Append(PERIOD);
            }

            Tasktext.text = stringBuilder.ToString();
        }
    }
}

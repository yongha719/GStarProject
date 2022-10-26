using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DateTime = System.DateTime;

[CreateAssetMenu(fileName = "DailyQuestInfo", menuName = "DailyQuestInfo", order = int.MinValue)]
public class DailyQuestUI_Info : ScriptableObject
{
    public QuestType questType;
    public string questName;
    public string questDescription;
}
public class BaseDailyQuest
{
    public QuestType type;
    public bool isCompleted;
    public int index;
    public int _index
    {
        get { return index; }
        set
        {
            if (value < index)
            {
                index = value;
                return;
            }
            if (value > GetClearCount())
            {
                value = GetClearCount();
            }
            else
            {
                index = value;
            }
        }
    }
    public bool isClear()
    {
        if (GetClearCount() <= _index) return true;
        else return false;
    }
    public int GetClearCount()
    {
        switch (type)
        {
            default:
                Debug.Log("DailyQuestLog Eror");
                break;
        }
        return 0;
    }
}

[System.Serializable]
public enum QuestType
{
    END
}
[System.Serializable]
public class DailyQuests
{
    public List<BaseDailyQuest> quests = new List<BaseDailyQuest>();
    public string nowTimeStr = null;
}
[System.Serializable]
public class DailyQuestInfo
{
    public bool isComplete;
    public int Count;
}
public class DailyQuest : MonoBehaviour
{
    public static DailyQuests dailyQuests = new DailyQuests();
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        CheckTimeToReset();
    }

    /// <summary>
    /// ��������Ʈ�� ���� 4�ø��� ���ŵȴ�
    /// </summary>
    private void CheckTimeToReset()
    {
        string lastTimeData = SaveManager.Instance.saveData.dailyQuests.nowTimeStr;
        if (lastTimeData != null)
        {
            DateTime nowtime = DateTime.Now;
            DateTime lastTime = DateTime.Parse(lastTimeData);
            if (new DateTime(lastTime.Year, lastTime.Month, lastTime.Day + 1, 4, 0, 0) <= nowtime)
            {
                //QuestReset();
            }
            else
            {
                DailyQuests loadData = SaveManager.Instance.saveData.dailyQuests;
                if (loadData != null)
                    dailyQuests = loadData;
                else
                    Debug.Log("QUEST SAVE LOAD ERROR");
            }
        }
        else
        {
            // QuestReset();
        }
    }
}

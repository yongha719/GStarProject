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
}
[System.Serializable]
public class DailyQuestInfo
{
    public bool isComplete;
    public int Count;
}
public class DailyQuest : MonoBehaviour
{
    private string dailyQuestTimeSavePath = "DaillyQuestTime DataPath";
    private string dailyQuestSavePath = "DaillyQuest DataPath";

    public DailyQuests dailyQuests = new DailyQuests();

    public bool DEBUG;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartDailyQuest()
    {
        if (DEBUG)
        {
            PlayerPrefs.DeleteKey(dailyQuestTimeSavePath);
            PlayerPrefs.DeleteKey(dailyQuestSavePath);
        }
        CheckTimeToReset();
    }

    /// <summary>
    /// 일일퀘스트는 새벽 4시마다 갱신된다
    /// </summary>
    private void CheckTimeToReset()
    {
        string lastTimeData = PlayerPrefs.GetString(dailyQuestTimeSavePath, "null");
        if (lastTimeData != "null")
        {
            DateTime nowtime = DateTime.Now;
            DateTime lastTime = DateTime.Parse(lastTimeData);
            if (new DateTime(lastTime.Year, lastTime.Month, lastTime.Day + 1, 4, 0, 0) <= nowtime)
            {
                //QuestReset();
            }
            else
            {
                string dataLoadString = PlayerPrefs.GetString(dailyQuestSavePath, "null");
                if (dataLoadString != "null")
                {
                    Debug.Log(dataLoadString);
                    dailyQuests = JsonUtility.FromJson<DailyQuests>(dataLoadString);
                }
                else
                {
                    Debug.Log("QUEST SAVE LOAD ERROR");
                }
            }
        }
        else
        {
           // QuestReset();
        }
    }
    private void QuestInfoSave()
    {
        string dailyQuestDataString = JsonUtility.ToJson(dailyQuests);
        PlayerPrefs.SetString(dailyQuestSavePath, dailyQuestDataString);

        DateTime nowTime = DateTime.Now;
        PlayerPrefs.SetString(dailyQuestTimeSavePath, nowTime.ToString());

        if (DEBUG)
        {
            PlayerPrefs.DeleteKey(dailyQuestTimeSavePath);
            PlayerPrefs.DeleteKey(dailyQuestSavePath);
        }
    }
    private void OnApplicationQuit()
    {
        QuestInfoSave();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            QuestInfoSave();
        }
    }
}

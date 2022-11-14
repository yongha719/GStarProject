using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DateTime = System.DateTime;


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
    Survive,
    Gold,
    Stamina,
    Gift,
    Photo,
    Adventure,
    END
}
[System.Serializable]
public class DailyQuests
{
    public List<BaseDailyQuest> quests = new List<BaseDailyQuest>();
    public string nowTimeStr = null;
}
[System.Serializable]
public class DailyQuestUIInfo
{
    public TextMeshProUGUI clearText;
    public TextMeshProUGUI clearRewardValue;
    public TextMeshProUGUI processingValue;
    public Image processingBar;

}
public class DailyQuestClearSaveData
{
    public int nowCount;
    public int beforeClearCount;
    public int returnRewardValue(int index)
    {
        index *= (beforeClearCount + 1) / 5;
        return index;
    }
}
public class DailyQuestManager : Singleton<DailyQuestManager>
{
    public List<DailyQuestClearSaveData> questDatas;

    [SerializeField]
    private Transform QuestPrefabParent;
    private DailyQuestUIInfo[] dailyQuestUIInfos = new DailyQuestUIInfo[6];

    public static DailyQuests dailyQuests = new DailyQuests();
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        CheckTimeToReset();
        for (int i = 0; i < QuestPrefabParent.childCount; i++)
        {
            dailyQuestUIInfos[i] = QuestPrefabParent.GetChild(i).GetComponent<QuestUIHave>().QuestUIInfo;
        }
    }


    /// <summary>
    /// 일일퀘스트는 새벽 4시마다 갱신된다
    /// </summary>
    private void CheckTimeToReset()
    {
        string lastTimeData = SaveManager.Instance.saveData.dailyQuests.nowTimeStr;
        if (lastTimeData != null && lastTimeData != "")
        {
            DateTime nowtime = DateTime.Now;
            Debug.Log(lastTimeData);
            DateTime lastTime = DateTime.Parse(lastTimeData);
            if (new DateTime(lastTime.Year, lastTime.Month, lastTime.Day + 1, 4, 0, 0) <= nowtime)
            {
                QuestReset();
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
            QuestReset();
        }
    }
    private void QuestReset()
    {
        dailyQuests.quests = new List<BaseDailyQuest>();
        for (QuestType i = 0; i < QuestType.END; i++)
        {
            dailyQuests.quests.Add(new BaseDailyQuest() { type = i });
        }
    }
}

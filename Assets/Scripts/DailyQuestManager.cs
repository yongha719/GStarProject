using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DateTime = System.DateTime;


public class BaseDailyQuest
{
    public QuestType type;
    public bool isCompleted;
    public int index;
    public int beforeClearCount;
    public int _index
    {
        get { return index; }
        set
        {
            index = Mathf.Clamp(value, 0, GetClearValue());
        }
    }
    public bool isClear()
    {
        if (GetClearValue() <= _index) return true;
        else return false;
    }
    public int GetClearValue()
    {
        return type switch
        {
            QuestType.Survive => 1,
            QuestType.Gold => 50,
            QuestType.Stamina => 50,
            QuestType.Gift => 3,
            QuestType.Photo => 3,
            QuestType.Adventure => 3,
        };
    }
    public double returnRewardValue(double index)
    {
        index *= (float)(beforeClearCount + 1) / 5;
        System.Math.Truncate(index);
        return index;
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
public class QuestUIInfo
{
    public Button clearBtn;
    public TextMeshProUGUI clearText;
    public TextMeshProUGUI clearRewardValue;
    public TextMeshProUGUI processingValue;
    public Image processingBar;
}

public class DailyQuestManager : Singleton<DailyQuestManager>
{

    [SerializeField]
    private Transform QuestPrefabParent;
    private QuestUIInfo[] questUIInfos = new QuestUIInfo[6];
    private DailyQuestInfo[] dailyQuestInfos;

    [SerializeField] private Sprite[] questBtn;

    public static DailyQuests dailyQuests = new DailyQuests();
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnEnable()
    {
        CheckTimeToReset();

        for (int i = 0; i < QuestPrefabParent.childCount; i++)
        {
            questUIInfos[i] = QuestPrefabParent.GetChild(i).GetComponent<QuestUIHave>().QuestUIInfo;
        }
        QuestReset();
        dailyQuestInfos = Resources.LoadAll<DailyQuestInfo>("QuestInfos/DailyQuestInfo");
        UIApply();
    }
    private void UIApply()
    {
        for (QuestType i = 0; i < QuestType.END; i++)
        {
            DailyQuestInfo dailyQuestInfo = dailyQuestInfos[(int)i];
            QuestUIInfo dailyQuestUIInfo = questUIInfos[(int)i];
            BaseDailyQuest curQuest = dailyQuests.quests[(int)i];

            int questNeedValue = curQuest.GetClearValue();
            int questClearValue = curQuest.index;

            dailyQuestUIInfo.processingValue.text = $"{questClearValue} / {questNeedValue}";
            dailyQuestUIInfo.processingBar.fillAmount = questClearValue / questNeedValue;
            dailyQuestUIInfo.clearRewardValue.text = $"<sprite name={returnResourceTypeString(dailyQuestInfo.rewardType)}> {CalculatorManager.returnStr(curQuest.returnRewardValue(dailyQuestInfo.rewardValue))}";

            dailyQuestUIInfo.clearText.DOFade(curQuest.isClear() ? 1f : 0.5f, 0);
            dailyQuestUIInfo.clearRewardValue.DOFade(curQuest.isClear() ? 1f : 0.5f, 0);
        }
    }
    private string returnResourceTypeString(EResourcesType type)
    {
        switch (type)
        {
            case EResourcesType.Coin:
                return "Coin";
            case EResourcesType.Ice:
                return "Ice";
            case EResourcesType.Stamina:
                return "Stamina";
        }
        return null;
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
    public void QuestRewardReceive(int index)
    {
        if (dailyQuests.quests[index].isClear())
        {
            switch (dailyQuestInfos[index].rewardType)
            {
                case EResourcesType.Coin:
                    GameManager.Instance._coin += dailyQuests.quests[index].returnRewardValue(dailyQuestInfos[index].rewardValue);
                    break;
                case EResourcesType.Ice:
                    GameManager.Instance._ice += dailyQuests.quests[index].returnRewardValue(dailyQuestInfos[index].rewardValue);
                    break;
                case EResourcesType.Stamina:
                    GameManager.Instance._energy += dailyQuests.quests[index].returnRewardValue(dailyQuestInfos[index].rewardValue);
                    break;
            }
            int clearCount = dailyQuests.quests[index].beforeClearCount;
            dailyQuests.quests[index] = new BaseDailyQuest() { type = (QuestType)index };
            dailyQuests.quests[index].beforeClearCount = clearCount + 1;
        }
        else
        {
            SoundManager.Instance.PlaySoundClip("SFX_Error", SoundType.SFX);
        }
    }
}

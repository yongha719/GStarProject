using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EAchivementsType
{
    Survive,
    GoldProduction,
    EnergyProduction,
    Gift,
    Photo,
    Adventure,
    Research,
    BuildingUpgrade,
    Trade,
    CatInvite,
    TownUpgrade,
    BuildingPlace,
    End
}
[System.Serializable]
public class BaseAchivement
{
    public EAchivementsType type;
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
    public int GetClearValue() => type switch
    {
        EAchivementsType.Survive => 1 + beforeClearCount,
        EAchivementsType.GoldProduction => 100 + beforeClearCount * 100,
        EAchivementsType.EnergyProduction => 100 + beforeClearCount * 100,
        EAchivementsType.Gift => 10 + beforeClearCount * 5,
        EAchivementsType.Photo => 10 + beforeClearCount * 5,
        EAchivementsType.Adventure => 3 + beforeClearCount * 3,
        EAchivementsType.Research => 3 + beforeClearCount * 3,
        EAchivementsType.BuildingUpgrade => 50 + beforeClearCount * 50,
        EAchivementsType.Trade => 2 + beforeClearCount * 2,
        EAchivementsType.CatInvite => 3 + beforeClearCount * 3,
        EAchivementsType.TownUpgrade => 1 + beforeClearCount,
        EAchivementsType.BuildingPlace => 2 + beforeClearCount * 2,
        _ => throw new Exception("붸")
    };


    public double returnRewardValue(double index)
    {
        return 20;
    }
}

[System.Serializable]
public class AchiveMents
{
    public List<BaseAchivement> baseAchivements;
}

public class AchivementsManager : Singleton<AchivementsManager>
{

    [SerializeField]
    private Transform AchivementsPrefabParent;
    private QuestUIInfo[] questUIInfos = new QuestUIInfo[12];

    [SerializeField] private Sprite[] questBtn;

    public static AchiveMents achiveMents = new AchiveMents();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        for (int i = 0; i < AchivementsPrefabParent.childCount; i++)
        {
            questUIInfos[i] = AchivementsPrefabParent.GetChild(i).GetComponent<QuestUIHave>().QuestUIInfo;
        }
        QuestReset();
        UIApply();
    }
    /// <summary>
    /// For Debugging
    /// </summary>
    private void QuestReset()
    {
        achiveMents.baseAchivements = new List<BaseAchivement>();
        for (EAchivementsType i = 0; i < EAchivementsType.End; i++)
        {
            achiveMents.baseAchivements.Add(new BaseAchivement() { type = i });
        }
    }
    private void UIApply()
    {
        for (EAchivementsType i = 0; i < EAchivementsType.End; i++)
        {
            QuestUIInfo dailyQuestUIInfo = questUIInfos[(int)i];
            BaseAchivement curQuest = achiveMents.baseAchivements[(int)i];

            int questNeedValue = curQuest.GetClearValue();
            int questClearValue = curQuest.index;

            dailyQuestUIInfo.processingValue.text = $"{questClearValue} / {questNeedValue}";
            dailyQuestUIInfo.processingBar.fillAmount = questClearValue / questNeedValue;
            dailyQuestUIInfo.clearRewardValue.text = $"<sprite name=Ice> 20";

            dailyQuestUIInfo.clearText.DOFade(curQuest.isClear() ? 1f : 0.5f, 0);
            dailyQuestUIInfo.clearRewardValue.DOFade(curQuest.isClear() ? 1f : 0.5f, 0);
        }
    }
    public void QuestRewardReceive(int index)
    {
        if (achiveMents.baseAchivements[index].isClear())
        {
            int clearCount = achiveMents.baseAchivements[index].beforeClearCount;
            int processingValue = achiveMents.baseAchivements[index]._index;
            achiveMents.baseAchivements[index] = new BaseAchivement() { type = (EAchivementsType)index };
            achiveMents.baseAchivements[index].beforeClearCount = clearCount + 1;
            achiveMents.baseAchivements[index]._index = processingValue;
        }
        else
        {
            SoundManager.Instance.PlaySoundClip("SFX_Error", SoundType.SFX);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "DailyQuestInfo", menuName = "DailyQuestInfo", order = int.MinValue)]
public class DailyQuestInfo : ScriptableObject
{
    public QuestType questType;
    public int needValueCount;
    public EResourcesType rewardType;
    public double rewardValue;
}
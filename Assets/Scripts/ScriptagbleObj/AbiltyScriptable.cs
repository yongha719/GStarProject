using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbiltyScriptable", menuName = "AbiltyScriptable", order = int.MinValue)]
[System.Serializable]
public class AbiltyScriptable : ScriptableObject
{
    public GoldAbilityType type;
    public Sprite icon;
    [TextArea]
    public string abiltyDesc;
}

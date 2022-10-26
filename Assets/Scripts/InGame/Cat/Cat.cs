using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ���� �ǹ� ����� �ɷ�
/// </summary>

public enum GoldAbilityType
{
    Fishing,                // ����
    Mining,                 // ����
    Axing,                  // ������
    Farming,                // ���
    Kiln,                   // ������
    Knitting,               // �߰��� 
    Boiling,                // ���̱�
    GeneratorOperating,     // ������
    End
}

public class Cat : MonoBehaviour
{
    public string Name;
    public GoldAbilityType GoldAbilityType;
    public int AbilityRating;

    public Dictionary<GoldAbilityType, Dictionary<int, int>> CatAbilityInfo = new Dictionary<GoldAbilityType, Dictionary<int, int>>();

    void Start()
    {
        var reductiontimebygrade = new Dictionary<int, int>()
        {
            { 1, 10 },
            { 2, 10 },
            { 3, 15 }
        };

        for (int abilityType = 0; abilityType < (int)GoldAbilityType.End; abilityType++)
        {
            CatAbilityInfo.Add((GoldAbilityType)abilityType, reductiontimebygrade);
        }
    }

    void Update()
    {

    }
}

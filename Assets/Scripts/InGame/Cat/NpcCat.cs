using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ���� �ǹ� ����� �ɷ�
/// </summary>

public enum GoldAbilityType
{
    Fishing,               // ����
    Mining,                // ����
    Axing,                 // ������
    Farming,               // ���
    Kiln,                  // ������
    Knitting,              // �߰��� 
    Boiling,               // ���̱�
    GeneratorOperating     // ������
}

public class Cat : MonoBehaviour
{
    public string Name;
    public GoldAbilityType GoldAbilityType;
    int AbilityRating;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class BigUnit
{
    public char Unit = '\'';
    public ushort Value = 0;


    public Dictionary<int, int> _values = new Dictionary<int, int>();


    public string GetString()
    {
        string returnStr = $"{Value}.{_values[Unit - 1] / 10}" + Unit;
        return returnStr;
    }
    public static BigUnit operator -(BigUnit A, BigUnit B)
    {
        if (A.Unit - B.Unit < 0 || (A.Value - B.Value < 0 && A.Unit == B.Unit))
        {
            Debug.Log("���� �߻� ū�� ������ -�� �� �� ����");
            return new BigUnit();
        }



        return new BigUnit();
    }

}
public class CalculatorManager : MonoBehaviour
{

    // ��ġ�� ���ӿ� ū �������� ���� ���ϰ� ����ϱ� ���Ͽ� ���� ����
    // 1000 = 1a �� �̻� ������ ���������� b,c,d... ������ �Ѿ  ���� z���ķδ� ���� ���� �Ѿ�� �����ּ���Ф�
    // ���ɿ� �����ϰ� �´ٰ� ������ ���� ȣ��

    /// <summary>
    /// ������ �⺻ ������ string������ ����Ǹ� "123.45 a" �̷������� ����
    /// </summary>
    /// <param name="valueStr">�⺻ ū �������� �̷���� </param>
    void ToBigUnit(string valueStr)
    {

    }
}

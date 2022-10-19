using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
// ��ġ�� ���ӿ� ū �������� ���� ���ϰ� ����ϱ� ���Ͽ� ���� ����
// 1000 = 1a �� �̻� ������ ���������� b,c,d... ������ �Ѿ  ���� z���ķδ� ���� ���� �Ѿ�� �����ּ���Ф�
// ���ɿ� �����ϰ� �´ٰ� ������ ���� ȣ��

public class BigUnit
{
    /// ������ �⺻ ������ char������ ����Ǹ� "123.45 a" �̷������� ����
    public char Unit = '\'';
    public ushort index = 0;

    public Dictionary<ushort, ushort> _values = new Dictionary<ushort, ushort>();

    public string GetString()
    {
        string returnStr = $"{index}.{_values[(ushort)(Unit - 1)] / 10}" + Unit;
        return returnStr;
    }
    public static BigUnit operator +(BigUnit A, BigUnit B)
    {
        BigUnit returnValue = new BigUnit();
        returnValue.index = (ushort)(A.index + B.index);
        while (returnValue.index >= 1000)
        {
            returnValue.index /= 1000;
            returnValue.Unit++;
            returnValue._values[A.Unit] = (ushort)(A.index % 1000);
        }


        return new BigUnit();
    }
    public static BigUnit operator -(BigUnit A, BigUnit B)
    {
        if (A.Unit - B.Unit < 0 || (A.index - B.index < 0 && A.Unit == B.Unit))
        {
            Debug.Log("���� �߻� ū�� ������ -�� �� �� ����");
            return new BigUnit();
        }

        return new BigUnit();
    }
    public static BigUnit operator *(BigUnit A, BigUnit B)
    {
        return new BigUnit();
    }
    public static BigUnit operator *(BigUnit A, int B)
    {
        return new BigUnit();
    }
    public static BigUnit operator /(BigUnit A, int B)
    {
        return new BigUnit();
    }

}

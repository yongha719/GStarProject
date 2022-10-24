using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using System.Linq;
public static class CalculatorManager
{
    //1000�� �ڸ��� ���� �ø��Ѵ�
    //123.45a <- �⺻ ����
    const int roundUnit = 10000;
    /// <summary>
    /// ���� double���� ���� 1000�� �ڸ������� ���ĺ����� �ٲ۴�
    /// </summary>
    public static string returnStr(this double value)
    {
        char unit = '`';

        //�Ҽ��� �ڿ� ���ڸ��� ��������� ���� ���� �������
        while (value >= roundUnit)
        {
            if (unit > 'z') break;
            value /= roundUnit;
            unit++;
        }
        value = System.Math.Round(value, 2);

        string returnStr;
        //�Ҽ��� �ڿ� ���ڸ� ó���ϴ� ����

        if (unit > '`') returnStr = $"{value}{unit}";
        else
        {
            System.Math.Truncate(value);
            returnStr = $"{value}";
        }
        return returnStr;
    }
    /// <summary>
    /// 123.45a ��(string) ���� double�� �ٲ��ش�
    /// </summary>
    public static double returnValue(this string str)
    {
        char unit = '`';
        double value;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] > '`')
            {
                unit = str[i];
                str.Remove(i);
            }
        }

        value = double.Parse(str);

        for (; unit > '`'; unit--) value *= roundUnit;

        return value;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using System.Linq;
class CalculatorManager : Singleton<CalculatorManager>
{
    //1000�� �ڸ��� ���� �ø��Ѵ�
    //123.45a <- �⺻ ����
    readonly int roundUnit = 1000;
    /// <summary>
    /// ���� double���� ���� 1000�� �ڸ������� ���ĺ����� �ٲ۴�
    /// </summary>
    public string returnStr(double value)
    {
        char unit = '`';
        int remainValue = 0;

        //�Ҽ��� �ڿ� ���ڸ��� ��������� ���� ���� �������
        while (value >= roundUnit)
        {
            if (unit > 'z') break;
            if (value < roundUnit * 100) remainValue = ((int)value % roundUnit) / 10;
            else remainValue = 0;
            value /= roundUnit;
            unit++;
        }
        value = System.Math.Ceiling(value);

        string returnStr, remainStr;
        //�Ҽ��� �ڿ� ���ڸ� ó���ϴ� ����
        if (remainValue < 10) remainStr = $"0{remainValue}";
        else remainStr = remainValue.ToString();

        if (unit > '`') returnStr = $"{value}.{remainStr}{unit}";
        else returnStr = $"{value}";

        return returnStr;
    }
    /// <summary>
    /// 123.45a ��(string) ���� double�� �ٲ��ش�
    /// </summary>
    public double returnValue(string str)
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
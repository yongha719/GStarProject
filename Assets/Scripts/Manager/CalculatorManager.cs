using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Audio;

class CalculatorManager : Singleton<CalculatorManager>
{
    //1000�� �ڸ��� ���� �ø��Ѵ�
    //123.45a <- �⺻ ����
    readonly int roundUnit = 1000;
    public string returnStr(double value)
    {
        char unit = '\'';
        int remainValue = 0;

        //�Ҽ��� �ڿ� ���ڸ��� ��������� ���� ���� �������
        while (value >= 1000)
        {
            if (unit >= 'z') break;
            if (value < 100000)
            {
                // �����ο��ٰ� �� �־��
            }
            remainValue = (int)value;
            value /= 1000;
            unit++;
        }

        string returnStr = $"value unit";
        return returnStr;
    }

}
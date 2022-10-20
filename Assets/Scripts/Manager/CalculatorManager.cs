using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Audio;

class CalculatorManager : Singleton<CalculatorManager>
{
    //1000에 자리수 마다 올림한다
    //123.45a <- 기본 형태
    readonly int roundUnit = 1000;
    public string returnStr(double value)
    {
        char unit = '\'';
        int remainValue = 0;

        //소수점 뒤에 두자리를 남기기위해 지난 수를 집어넣음
        while (value >= 1000)
        {
            if (unit >= 'z') break;
            if (value < 100000)
            {
                // 리메인에다가 값 넣어라
            }
            remainValue = (int)value;
            value /= 1000;
            unit++;
        }

        string returnStr = $"value unit";
        return returnStr;
    }

}
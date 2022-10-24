using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using System.Linq;
class CalculatorManager : Singleton<CalculatorManager>
{
    //1000에 자리수 마다 올림한다
    //123.45a <- 기본 형태
    readonly int roundUnit = 1000;
    /// <summary>
    /// 높은 double형에 수를 1000의 자리수마다 알파벳으로 바꾼다
    /// </summary>
    public string returnStr(double value)
    {
        char unit = '`';
        int remainValue = 0;

        //소수점 뒤에 두자리를 남기기위해 지난 수를 집어넣음
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
        //소수점 뒤에 숫자를 처리하는 과정
        if (remainValue < 10) remainStr = $"0{remainValue}";
        else remainStr = remainValue.ToString();

        if (unit > '`') returnStr = $"{value}.{remainStr}{unit}";
        else returnStr = $"{value}";

        return returnStr;
    }
    /// <summary>
    /// 123.45a 형(string) 값을 double로 바꿔준다
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
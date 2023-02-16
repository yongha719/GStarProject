using System.Runtime.InteropServices;
using UnityEngine;

public static class CalculatorExetension
{
    //1000에 자리수 마다 올림한다
    //123.45a <- 기본 형태
    const int roundUnit = 10000;
    /// <summary>
    /// 높은 double형에 수를 1000의 자리수마다 알파벳으로 바꾼다
    /// </summary>
    public static string returnStr(this double value)
    {
        char unit = '`';

        //소수점 뒤에 두자리를 남기기위해 지난 수를 집어넣음
        while (value >= roundUnit)
        {
            if (unit > 'z')
            {
                Debug.Log("Func: \"returnstr\" error - 단위 초과");
                break;
            }
            value /= roundUnit;
            unit++;
        }
        value = System.Math.Round(value, 2);

        string returnStr;
        //소수점 뒤에 숫자를 처리하는 과정

        if (unit > '`') returnStr = $"{value}{unit}";
        else
        {
            System.Math.Truncate(value);
            returnStr = $"{value}";
        }
        return returnStr;
    }
    /// <summary>
    /// 123.45a 형(string) 값을 double로 바꿔준다
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
                str = str.Remove(i);
            }
        }

        value = double.Parse(str);

        for (; unit > '`'; unit--) value *= roundUnit;

        return value;
    }
}
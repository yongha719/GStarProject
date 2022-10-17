using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
// 방치형 게임에 큰 단위수를 좀더 편하게 계산하기 위하여 만든 계산기
// 1000 = 1a 그 이상 단위가 오를때마다 b,c,d... 순으로 넘어감  아직 z이후로는 개발 안함 넘어가지 말아주세요ㅠㅠ
// 성능에 과부하가 온다고 생각시 깜펭 호출

public class BigUnit
{
    /// 단위는 기본 적으로 char형으로 저장되며 "123.45 a" 이런식으로 계산됨
    public char Unit = '\'';
    public int index = 0;


    public Dictionary<int, int> _values = new Dictionary<int, int>();


    public string GetString()
    {
        string returnStr = $"{index}.{_values[Unit - 1] / 10}" + Unit;
        return returnStr;
    }
    public static BigUnit operator +(BigUnit A, BigUnit B)
    {
        A.index += B.index;
        while (A.index >= 100)
        {
            A.Unit++;
            A.index /= 100;
            
        }


        return new BigUnit();
    }
    public static BigUnit operator -(BigUnit A, BigUnit B)
    {
        if (A.Unit - B.Unit < 0 || (A.index - B.index < 0 && A.Unit == B.Unit))
        {
            Debug.Log("오류 발생 큰수 연산은 -가 될 수 없음");
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

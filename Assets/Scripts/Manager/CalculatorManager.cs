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
            Debug.Log("오류 발생 큰수 연산은 -가 될 수 없음");
            return new BigUnit();
        }



        return new BigUnit();
    }

}
public class CalculatorManager : MonoBehaviour
{

    // 방치형 게임에 큰 단위수를 좀더 편하게 계산하기 위하여 만든 계산기
    // 1000 = 1a 그 이상 단위가 오를때마다 b,c,d... 순으로 넘어감  아직 z이후로는 개발 안함 넘어가지 말아주세요ㅠㅠ
    // 성능에 과부하가 온다고 생각시 깜펭 호출

    /// <summary>
    /// 단위는 기본 적으로 string형으로 저장되며 "123.45 a" 이런식으로 계산됨
    /// </summary>
    /// <param name="valueStr">기본 큰 수형으로 이루어진 </param>
    void ToBigUnit(string valueStr)
    {

    }
}

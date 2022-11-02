using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// FindObjectType을 사용하여 쓸 것
/// 
/// </summary>
public class CatManager : MonoBehaviour
{
    public List<CatData> CatList = new List<CatData>();


    /// <summary>
    /// 마을 내보내기
    /// </summary>
    public void RemoveCat(CatData cat) => CatList.Remove(cat);

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceProductionBuilding
{
    public void OnCatMemberChange(CatData catData,int index, Action action);
    IEnumerator ResourceProduction();
    public IEnumerator WaitGetResource();

}

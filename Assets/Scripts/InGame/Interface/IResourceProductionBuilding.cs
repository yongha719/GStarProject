using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceProductionBuilding 
{
    public void OnCatMemberChange(Cat cat,Action action);
    public IEnumerator ResourceProduction { get; }

    public IEnumerator WaitGetResource();

}

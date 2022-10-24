using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceProductionBuilding 
{
    public IEnumerator ResourceProduction();
    public IEnumerator CWaitClick();
}

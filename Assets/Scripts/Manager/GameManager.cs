using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ResourceClass
{
    public double coin;
    public double energy;
    public double ice;
}
[System.Serializable]
public enum EResourcesType
{
    Coin,
    Ice,
    Stamina,
    End
}
public class GameManager : Singleton<GameManager>
{

    public ResourceClass resource = new ResourceClass();

    public double _coin
    {
        get { return resource.coin; }
        set
        {
            resource.coin = value;
            if (UIManager.Instance != null) UIManager.Instance.ResourcesApply();
        }
    }
    public double _energy
    {
        get { return resource.energy; }
        set
        {
            resource.energy = value;
            if (UIManager.Instance != null) UIManager.Instance.ResourcesApply();
        }
    }
    public double _ice
    {
        get { return resource.ice; }
        set
        {
            resource.ice = value;
            if (UIManager.Instance != null) UIManager.Instance.ResourcesApply();
        }
    }



    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }
    void Start()
    {

    }

}

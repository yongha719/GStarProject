using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ResourceClass
{
    public double catCount;
    public double coin;
    public double energy;
    public double ice;
}
public class GameManager : Singleton<GameManager>
{

    public ResourceClass resource = new ResourceClass();

    public double _catCount
    {
        get { return resource.catCount; }
        set
        {
            //만약에 ui매니저가 씬내에서 사라졌다면 어떻게 처리되지? 그건 좀 알아봐야 할듯
            resource.catCount = value;
            if (UIManager.Instance != null) UIManager.Instance.ResourcesApply();
        }
    }
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

        _coin = double.MaxValue;
    }
    void Start()
    {

    }

    void Update()
    {

    }
}

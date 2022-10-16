using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ResourceClass
{
    public int catCount;
    public int coin;
    public int electric;
    public int ice;
}
public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public ResourceClass resource = new ResourceClass();

    public int _catCount
    {
        get { return resource.catCount; }
        set { resource.catCount = value; }
    }
    public int _coin
    {
        get { return resource.coin; }
        set { resource.coin = value; }
    }
    public int _electric
    {
        get { return resource.electric; }
        set { resource.electric = value; }
    }
    public int _ice
    {
        get { return resource.ice; }
        set { resource.ice = value; }
    }
    void Start()
    {

    }

    void Update()
    {

    }
}

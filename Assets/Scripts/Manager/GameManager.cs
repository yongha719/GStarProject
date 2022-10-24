using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ResourceClass
{
    public double catCount;
    public double coin;
    public double electric;
    public double ice;
}
public class GameManager : Singleton<GameManager>
{

    public ResourceClass resource = new ResourceClass();

    public double _catCount
    {
        get { return resource.catCount; }
        set { resource.catCount = value; }
    }
    public double _coin
    {
        get { return resource.coin; }
        set { resource.coin = value; }
    }
    public double _electric
    {
        get { return resource.electric; }
        set { resource.electric = value; }
    }
    public double _ice
    {
        get { return resource.ice; }
        set { resource.ice = value; }
    }
    private void Awake()
    {
        if (Instance != null)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }

    void Update()
    {

    }
}

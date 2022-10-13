using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    [System.Serializable]
    public class ResourceClass
    {
        public int catCount;
        public int coin;
        public int electric;
        public int ice;
    }
    private ResourceClass m_Resource;
    int _catCount
    {
        get { return m_Resource.catCount; }
        set { m_Resource.catCount = value; }
    }
    public int _coin
    {
        get { return m_Resource.coin; }
        set { m_Resource.coin = value; }
    }
    public int _electric
    {
        get { return m_Resource.electric; }
        set { m_Resource.electric = value; }
    }
    public int _ice
    {
        get { return m_Resource.ice; }
        set { m_Resource.ice = value; }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

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

    public ResourceClass resourceClass;
    public ResourceClass _resourceClass
    {
        get { return resourceClass; }
        set
        {
            resourceClass = value; 
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

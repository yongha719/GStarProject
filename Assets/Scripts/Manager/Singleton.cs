using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    Debug.Assert(false, "야야 아무리 찾아도 없는데 뭔가 잘못됨");
                    return null;
                }
            }
            return instance;
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}

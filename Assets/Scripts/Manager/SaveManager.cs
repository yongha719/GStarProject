using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SaveManager : Singleton<SaveManager>
{
    [System.Serializable]
    public class SaveData
    {
        ResourceClass ResourceClass = GameManager.Instance.resource;

    }
    public SaveData saveData;

    private string savePath = "DataSavePath";
    private void Awake()
    {
        DontDestroyOnLoad(this);
        DataLoad();
    }

    private void DataLoad()
    {
        string dataString = PlayerPrefs.GetString(savePath, "null");
        if (dataString == "null")
        {
            saveData = new SaveData();
        }
        else
        {
            saveData = JsonUtility.FromJson<SaveData>(dataString);
        }
    }
    private void DataSave()
    {
        string dataString = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(savePath, dataString);
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            DataSave();
        }
    }

    private void OnApplicationQuit()
    {
        DataSave();
    }
}

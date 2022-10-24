using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SaveManager : Singleton<SaveManager>
{
    [System.Serializable]
    public class SaveData
    {
        public ResourceClass ResourceClass;

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
        if (dataString == "null") saveData = new SaveData();
        else saveData = JsonUtility.FromJson<SaveData>(dataString);

        OutputSaveData();
    }
    private void DataSave()
    {
        InputSaveData();

        string dataString = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(savePath, dataString);
    }

    private void InputSaveData()
    {
        saveData.ResourceClass = GameManager.Instance.resource;
    }
    private void OutputSaveData()
    {
        GameManager.Instance.resource = saveData.ResourceClass;

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

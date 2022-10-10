using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SaveManager : Singleton<SaveManager>
{
    [System.Serializable]
    public class SaveData
    {
        //TODO: 단순 재화들은 나중에 한 클래스로 묶고 싶다만 딱히 재화 시스템을 만들라는 기획은 없으니 기각
        public int catCount;
        public int gold;

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

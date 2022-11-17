using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SaveManager : Singleton<SaveManager>
{
    [System.Serializable]
    public class SaveData
    {
        public ResourceClass ResourceClass = new ResourceClass();
        public DailyQuests dailyQuests = new DailyQuests();
        public float[] audioVolumes = new float[(int)SoundType.END];
        public List<Cat> cats = new List<Cat>();
    }
    public SaveData saveData;

    [Tooltip("세이브 날리기")]
    [SerializeField] private bool DEBUG;
    private string savePath = "DataSavePath";
    private void Awake()
    {
        DontDestroyOnLoad(this);
        DataLoad();
    }

    private void DataLoad()
    {
        if (DEBUG) return;

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
        saveData.audioVolumes = SoundManager.Instance.audioVolumes;
        saveData.dailyQuests = DailyQuestManager.dailyQuests;
        saveData.cats = CatManager.Instance.CatList;
        saveData.dailyQuests.nowTimeStr = System.DateTime.Now.ToString();
    }

    private void OutputSaveData()
    {
        GameManager.Instance.resource = saveData.ResourceClass;
        SoundManager.Instance.audioVolumes = saveData.audioVolumes;
        DailyQuestManager.dailyQuests = saveData.dailyQuests;
        CatManager.Instance.CatList = saveData.cats;
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

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

public class MissionData : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] bool isPersistent;
    [Header("File Path")]
    [SerializeField] string missionFileName = "Mission Table";
    [SerializeField] string rewardFileName = "Reward Table";
    [SerializeField] string titleFileName = "Tittle Table";
    [SerializeField] string itemFileName = "Item Table";
    [SerializeField] string jsonPath = GLOBAL_PATH+"/JSON";
    [SerializeField] string csvPath = GLOBAL_PATH + "/CSV";
    [SerializeField] MissionTable[] missionTable;
    [SerializeField] RewardTable[] rewardTable;
    [SerializeField] TitleTable[] titleTable;
    [SerializeField] ItemTable[] itemTable;

    const string GLOBAL_PATH = "Mission Data";
    
    private void Start()
    {
        MissionSystem.Instance.StoredData
            (
                missionTable,
                rewardTable,
                titleTable,
                itemTable
            );
    }
    #region JSON Region
    [ContextMenu("Save Data To JSON")]
    void SaveFromJSON()
    {
        /*SaveMissionTable();
        SaveRewardTable();
        SaveTittleTable();
        SaveItemTable();*/

        SaveJSONTable(missionTable);
        SaveJSONTable(rewardTable);
        SaveJSONTable(titleTable);
        SaveJSONTable(itemTable);
    }
    [ContextMenu("Load Data From JSON")]
    void LoadFromJSON()
    {
        //LoadMissionTable();
        LoadJSONTable(out missionTable);
        LoadJSONTable(out rewardTable);
        LoadJSONTable(out titleTable);
        LoadJSONTable(out itemTable);
    }
    void SaveJSONTable<T>(T table)
    {
        string filename = GetFileName(typeof(T));
        string json = JsonConvert.SerializeObject(table, Formatting.Indented);

        string persistent = isPersistent == true ? Application.persistentDataPath : Application.dataPath;

        string path = Path.Combine(persistent, jsonPath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Create Directory");
        }

        string filePath = Path.Combine(path, filename + ".json");

        File.WriteAllText(filePath, json);

        Debug.Log("Data saved to " + filePath);
    }
    void LoadJSONTable<T>(out T table)
    {
        string fileName = GetFileName(typeof(T));

        string persisten = isPersistent == true ? Application.persistentDataPath : Application.dataPath;

        string path = Path.Combine(persisten, jsonPath);
        string filePath = Path.Combine(path, fileName + ".json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            table = JsonConvert.DeserializeObject<T>(json);

            Debug.LogWarning("Update Data Mission Table Load from " + path);
            Debug.LogWarning("File Name = " + fileName);

        }
        else
        {
            Debug.LogWarning("No data file found at " + path);
            table = default(T);
        }
    }
    private string GetFileName(Type type)
    {
        if (type == typeof(MissionTable[]) || type == typeof(MissionTable))
        {
            return missionFileName;
        }
        else if (type == typeof(RewardTable[]) || type == typeof(RewardTable))
        {
            return rewardFileName;
        }
        else if (type == typeof(TitleTable[]) || type == typeof(TitleTable))
        {
            return titleFileName;
        }
        else if (type == typeof(ItemTable[]) || type == typeof(ItemTable))
        {
            return itemFileName;
        }
        else
        {
            return null;
        }
    }
    #region Save JSON
    void SaveMissionTable()
    {
        string json = JsonConvert.SerializeObject(missionTable, Formatting.Indented);

        string persistent = isPersistent == true ? Application.persistentDataPath : Application.dataPath;

        string path = Path.Combine(persistent, jsonPath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Create Directory");
        }

        string filePath = Path.Combine(path, missionFileName+".json");

        File.WriteAllText(filePath, json);

        Debug.Log("Data saved to " + filePath);
    }
    void SaveRewardTable()
    {
        string json = JsonConvert.SerializeObject(rewardTable, Formatting.Indented);

        string persistent = isPersistent == true ? Application.persistentDataPath : Application.dataPath;

        string path = Path.Combine(persistent, jsonPath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Create Directory");
        }

        string filePath = Path.Combine(path, rewardFileName + ".json");

        File.WriteAllText(filePath, json);

        Debug.Log("Data saved to " + filePath);
    }
    void SaveTittleTable()
    {
        string json = JsonConvert.SerializeObject(titleTable, Formatting.Indented);

        string persistent = isPersistent == true ? Application.persistentDataPath : Application.dataPath;

        string path = Path.Combine(persistent, jsonPath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Create Directory");
        }

        string filePath = Path.Combine(path, titleFileName + ".json");

        File.WriteAllText(filePath, json);

        Debug.Log("Data saved to " + filePath);
    }
    void SaveItemTable()
    {
        string json = JsonConvert.SerializeObject(itemTable, Formatting.Indented);

        string persistent = isPersistent == true ? Application.persistentDataPath : Application.dataPath;

        string path = Path.Combine(persistent, jsonPath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Create Directory");
        }

        string filePath = Path.Combine(path, itemFileName + ".json");

        File.WriteAllText(filePath, json);

        Debug.Log("Data saved to " + filePath);
    }
    #endregion

    #region Load JSON
    void LoadMissionTable()
    {
        string persisten = isPersistent == true ? Application.persistentDataPath : Application.dataPath;

        string path = Path.Combine(persisten, jsonPath);
        string filePath = Path.Combine(path, missionFileName + ".json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            missionTable = JsonConvert.DeserializeObject<MissionTable[]>(json);

            Debug.LogWarning("Update Data Mission Table Load from " + path);

        }
        else
        {
            Debug.LogWarning("No data file found at " + path);
        }
    }
    #endregion
    #endregion
    #region CSV Region
    [ContextMenu("Save To CSV")]
    void SaveCsvFile()
    {
        /*SaveCsvMissionTable();
        SaveCsvRewardTable();
        SaveCsvTittleTable();
        SaveCsvItemTable();*/

        SaveCsvTable(missionTable.ToList());
        SaveCsvTable(rewardTable.ToList());
        SaveCsvTable(titleTable.ToList());
        SaveCsvTable(itemTable.ToList());
    }
    [ContextMenu("Load Csv File")]
    void LoadCsvFile()
    {
        List<MissionTable> temp = new();
        LoadCsvTable(out temp);
        //Debug.Log("Temp Value = "+temp[0].MissionName);
        //missionTable = temp.ToArray();
    }
    void LoadCsvTable<T>(out List<T> table)
    {
        string fileName = GetFileName(typeof(T));
        string persistent = isPersistent ? Application.persistentDataPath : Application.dataPath;
        string path = Path.Combine(persistent, csvPath);
        string filePath = Path.Combine(path, fileName + ".csv");

        if (File.Exists(filePath))
        {
            //table;
            var temp= CsvLoader.LoadCsv<MissionTable>(filePath);
            Debug.LogWarning(temp[1].MissionName);
            //table = temp;
            Debug.LogWarning("Update Data Mission Table Load from " + filePath);
            Debug.LogWarning("File Name = " + fileName);
            table = default(List<T>);
        }
        else
        {
            Debug.LogWarning("No data file found at " + path);
            table = default(List<T>);
        }
    }
 

    void SaveCsvTable<T>(List<T> table)
    {
        string fileName = GetFileName(typeof(T));
        string persisten = isPersistent == true ? Application.persistentDataPath : Application.dataPath;

/*        string path = Path.Combine(persisten, jsonPath);
        string filePath = Path.Combine(path, fileName + ".json");

        string json = File.ReadAllText(filePath);

        List<T> missionData = JsonConvert.DeserializeObject<List<T>>(json);*/

        string csv = GenerateCsv(table, typeof(T));
        string pathCsv = Path.Combine(persisten, csvPath);
        if (!Directory.Exists(pathCsv))
        {
            Directory.CreateDirectory(pathCsv);
        }
        string filePathCsv = Path.Combine(pathCsv, fileName + ".csv");

        File.WriteAllText(filePathCsv, csv);

        Debug.Log("Data saved to " + filePathCsv);
    }
    void SaveCsvMissionTable()
    {
        string persisten = isPersistent == true ? Application.persistentDataPath : Application.dataPath;

        string path = Path.Combine(persisten, jsonPath);
        string filePath = Path.Combine(path, missionFileName+".json");

        string json = File.ReadAllText(filePath);

        List<MissionTable> missionData = JsonConvert.DeserializeObject<List<MissionTable>>(json);

        //string csv = GenerateCsv(missionData);
        string pathCsv = Path.Combine(persisten, csvPath);
        if (!Directory.Exists(pathCsv))
        {
            Directory.CreateDirectory(pathCsv);
        }
        string filePathCsv = Path.Combine(pathCsv, missionFileName + ".csv");

        //File.WriteAllText(filePathCsv, csv);

        Debug.Log("Data saved to " + filePathCsv);
    }
    private string GenerateCsv<T>(List<T> data, Type type)
    {
        StringWriter csvString = new StringWriter();

        string headerCSV = GetHeaderData(type);
        // Write header
        csvString.WriteLine(headerCSV);
        
        if (type == typeof(MissionTable[]) || type == typeof(MissionTable))
        {
            var tempData = data as List<MissionTable>;
            Debug.Log("Temp Data Lenght "+tempData.Count);
            foreach (var item in tempData)
            {
                csvString.WriteLine($"{item.MissionID},{item.MissionName},{item.MissionDescription},{item.MissionAction},{item.CountAction},{item.RewardID}");
            }
        }
        else if (type == typeof(RewardTable[]) || type == typeof(RewardTable))
        {
            var tempData = data as List<RewardTable>;
            Debug.Log("Temp Data Lenght " + tempData.Count);
            foreach (var item in tempData)
            {
                csvString.WriteLine($"{item.RewardID},{item.RewardType},{item.Value}");
            }
        }
        else if (type == typeof(TitleTable[]) || type == typeof(TitleTable))
        {
            var tempData = data as List<TitleTable>;
            Debug.Log("Temp Data Lenght " + tempData.Count);
            foreach (var item in tempData)
            {
                csvString.WriteLine($"{item.TitleID},{item.TitleName},{item.TitleDescription}");
            }
        }
        else if (type == typeof(ItemTable[]) || type == typeof(ItemTable))
        {
            var tempData = data as List<ItemTable>;
            Debug.Log("Temp Data Lenght " + tempData.Count);
            foreach (var item in tempData)
            {
                csvString.WriteLine($"{item.ItemID},{item.ItemName}");
            }
        }

        return csvString.ToString();
    }
    string GetHeaderData(Type type)
    {
        if (type == typeof(MissionTable[]) || type == typeof(MissionTable))
        {
            return "Mission ID,Mission Name,Mission Decscription,Mission Action,Count Action,Reward ID";
        }
        else if (type == typeof(RewardTable[]) || type == typeof(RewardTable))
        {
            return "Reward ID,Reward Type,Value";
        }
        else if (type == typeof(TitleTable[]) || type == typeof(TitleTable))
        {
            return "Title ID,Tittle Name,Tittle Description";
        }
        else if (type == typeof(ItemTable[]) || type == typeof(ItemTable))
        {
            return "Item ID,Item Name";
        }
        else
        {
            return null;
        }
    }
    #endregion
}
[System.Serializable]
public class MissionTable
{
    public int MissionID;
    public string MissionName;
    public string MissionDescription;
    public MissionAction MissionAction;
    public int CountAction;
    public int RewardID;
    public MissionStatus Status;
}
[System.Serializable]
public class RewardTable
{
    public int RewardID;
    public RewardType RewardType;
    public int Value;
}
[System.Serializable]
public class TitleTable
{
    public int TitleID;
    public string TitleName;
    public string TitleDescription;
}
[System.Serializable]
public class ItemTable
{
    public int ItemID;
    public string ItemName;
}
public enum MissionAction
{
    Sleep,
    Eat,
    Bathe
}
public enum RewardType
{
    Exp,
    Title,
    Item
}
public enum MissionStatus
{
    Unassign,
    Active,
    Succes,
    Fail
}
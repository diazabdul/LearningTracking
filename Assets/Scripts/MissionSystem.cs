using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionSystem : MonoBehaviour
{
    public static MissionSystem Instance;

    [SerializeField] MissionTable[] missionTable;
    [SerializeField] RewardTable[] rewardTable;
    [SerializeField] TitleTable[] titleTable;
    [SerializeField] ItemTable[] itemTable;

    public MissionTable MissionData(int index) => missionTable[index];
    public RewardTable RewardData(int index) => rewardTable[index];
    public TitleTable TitleData(int index) => titleTable[index];
    public ItemTable ItemData(int index) => itemTable[index];

    public delegate void UpdateMissionData(MissionTable[] mission);
    public static event UpdateMissionData OnUpdateMission;
    public void StoredData
        (
        MissionTable[] missionTable,
        RewardTable[] rewardTable,
        TitleTable[] titleTable,
        ItemTable[] itemTable
        )
    {
        this.missionTable = missionTable;
        this.rewardTable = rewardTable;
        this.titleTable = titleTable;
        this.itemTable = itemTable;

        Debug.Log("Data Stored");
        OnUpdateMission?.Invoke(this.missionTable);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
}

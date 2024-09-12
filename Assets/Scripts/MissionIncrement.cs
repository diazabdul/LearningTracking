using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissionIncrement : MonoBehaviour
{
    [Header("Setup Mission")]
    [SerializeField] MissionAction currentAction;

    [Header("Detail Quest")]
    [SerializeField] MissionTable missionTable;

    [Header("Debugging")]
    [SerializeField] int currentCounting;
    [SerializeField] int targetCounting;
    MissionSystem MissionSystem => MissionSystem.Instance;
    private void OnEnable()
    {
        MissionSystem.OnUpdateMission += UpdateMission;
    }

    private void UpdateMission(MissionTable[] mission)
    {
        for (int i = 0; i < mission.Length; i++)
        {
            if(mission[i].MissionAction == currentAction && mission[i].Status == MissionStatus.Unassign)
            {
                missionTable = MissionSystem.MissionData(i);
                targetCounting = missionTable.CountAction;
                missionTable.Status = MissionStatus.Active;
                break;
            }
        }
    }

    private void OnDisable()
    {
        MissionSystem.OnUpdateMission -= UpdateMission;
    }
    [ContextMenu("Increment Mission")]
    public void IncrementAction()
    {
        if (missionTable.Status != MissionStatus.Active)
            return;

        currentCounting++;
        if(currentCounting == targetCounting)
        {
            missionTable.Status = MissionStatus.Succes;
            var rewardType = MissionSystem.RewardData(missionTable.RewardID).RewardType;
            var rewardValue = MissionSystem.RewardData(missionTable.RewardID).Value;
            string rewardDetails;
            switch (rewardType)
            {
                case RewardType.Exp:
                    rewardDetails = rewardValue.ToString();
                    break;
                case RewardType.Title:
                    rewardDetails = MissionSystem.TitleData(rewardValue).TitleName;
                    break;
                case RewardType.Item:
                    rewardDetails = MissionSystem.ItemData(rewardValue).ItemName;
                    break;
                default:
                    rewardDetails = "Null";
                    break;
            }
            Debug.Log("Mission Succes, You Got = " +rewardType+" Value = "+rewardDetails);
        }
    }
}

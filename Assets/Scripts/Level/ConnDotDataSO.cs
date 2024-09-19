using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Connection Dot Data", menuName = "Minigame/Connection Dot/Data")]
public class ConnDotDataSO : ScriptableObject
{
    [SerializeField] ConnDotPlayerSO playerData;
    [SerializeField] ConnDotLevelSO levelData;


    public ConnDotPlayerSO GetPlayer => playerData;
    public ConnDotLevelDetailSO GetLevel(int level) => levelData.LevelDetail(level);
    public int MaxLevel => levelData.LenghtDetailLevel;
}


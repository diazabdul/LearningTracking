using UnityEngine;

[CreateAssetMenu(fileName = "Connection Dot Level", menuName = "Minigame/Connection Dot/Level")]
public class ConnDotLevelSO : ScriptableObject
{
    [SerializeField] ConnDotLevelDetailSO[] detailLevels;

    public ConnDotLevelDetailSO LevelDetail(int level) => detailLevels[level];
    public int LenghtDetailLevel => detailLevels.Length;
}

using UnityEngine;

[CreateAssetMenu(fileName = "Connection Dot Player Data", menuName = "Minigame/Connection Dot/Player")]
public class ConnDotPlayerSO : ScriptableObject
{
    [SerializeField] int currentLevel;

    public int CurrentLevel => currentLevel;
    public void LevelUp() => currentLevel++;
    public void ResetLevel() => currentLevel = 0;
}

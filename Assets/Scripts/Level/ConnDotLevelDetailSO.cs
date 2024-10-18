using UnityEngine;

[CreateAssetMenu(fileName = "Level 0", menuName = "Minigame/Connection Dot/Level Details")]
public class ConnDotLevelDetailSO : ScriptableObject
{
    [SerializeField] int level;
    [SerializeField] int tileSize;
    [SerializeField] int maxMove;
    [SerializeField] DotPlacement[] dotPositionVariation;


    public int Level => level;
    public int TileSize => tileSize;
    public int MaxMove => maxMove;

    public DotPosition[] GetDotPlacement => dotPositionVariation[Random.Range(0, dotPositionVariation.Length)].GetDotPosition;

}
[System.Serializable]
public class DotPlacement
{
    [SerializeField] DotPosition[] dotPosition;

    public DotPosition[] GetDotPosition => dotPosition;
}
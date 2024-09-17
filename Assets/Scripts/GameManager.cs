using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Main Components")]
    [SerializeField] Tile tile;
    [SerializeField] Transform lookPos;
    [Header("Game Setting")]
    [Range(3,10)]
    [SerializeField] int tileSize = 4;

    [SerializeField] bool isInteger;
    [SerializeField] List<DotPosition> dotPosition = new List<DotPosition>();
    [SerializeField] List<Tile> listTile = new List<Tile>();
    [SerializeField] List<Tile> chainTile = new List<Tile>();


    public System.Action<DirectionLine[]> ActivateNextTiles;
    public System.Action ResetChain;

    int GetLastIndexChain => chainTile.Count - 1;

    //Helper
    [Header("Debugging")]
    [SerializeField] Tile currentTile;
    [SerializeField] Tile lastExitTile;
    [SerializeField] Tile firstNodeTile;
    [SerializeField] List<CompletedNode> completedNodes = new List<CompletedNode>();

    public void OnTileFirstClick(Tile tile)
    {
        AddSChainTile(tile);
        currentTile = tile;
        firstNodeTile = tile;
    }
    public void AddSChainTile(Tile tile)
    {
        if (chainTile.Contains(tile)) return;

        chainTile.Add(tile);
        ActivateNextTile(tile.GetPos);
    }
    public void ResetChainTile()
    {
        chainTile.Clear();
        ResetChain?.Invoke();

        currentTile = null;
        lastExitTile = null;

        firstNodeTile = null;
    }
    public bool CheckWin(Tile tile)
    {
        if((firstNodeTile != tile) && firstNodeTile.GetTileId == tile.GetTileId)
        {
            Debug.Log("Chain Completed");

            chainTile.Add(tile);

            foreach (var item in chainTile)
            {
                item.SetChainComplete();
            }
            completedNodes.Add(new CompletedNode(firstNodeTile.GetTileId, chainTile));

            ResetChain?.Invoke();

            chainTile.Clear();
            /*            currentTile = null;
                        lastExitTile = null;

                        firstNodeTile = null;*/

            return true;
        }
        return false;
    }
    void ActivateNextTile(Vector2 vec)
    {
        Vector2 right = new(vec.x+1, vec.y);
        Vector2 top = new(vec.x, vec.y+1);
        Vector2 left = new(vec.x-1, vec.y);
        Vector2 bottom = new(vec.x, vec.y-1);

        DirectionLine[] data = { new(right, PipePos.Right), new(top, PipePos.Top), new (left, PipePos.Left), new(bottom, PipePos.Down)};
        ActivateNextTiles?.Invoke(data);

        Debug.Log($"Right = {right}, Top = {top}, Left = {left}, Botton = {bottom}");
    }
    void DeactiveTiles()
    {

    }
    private void Awake()
    {
        Vector2 camPos;
        if (tileSize % 2 == 0)
        {
            float tileMid = (tileSize / 2)-0.5f;
            camPos = new Vector2(tileMid,tileMid);
        }
        else
        {
            int tileMid = (tileSize-1)/2;
            camPos = new Vector2(tileMid, tileMid);
        }
        lookPos.position = camPos;
            
    }
    private void Start()
    {
        for (int i = 0; i < tileSize; i++)
        {
            for (int j = 0; j < tileSize; j++)
            {              

                var tempObj = Instantiate(tile, transform);
                tempObj.name = "Tile {"+i+"} {"+j+"}";
                Vector2 pos = new(i, j);
                tempObj.transform.position = pos;
                tempObj.Initialize(new Vector2(i,j),this);

                listTile.Add(tempObj);
            }
        }

        foreach (var item in listTile)
        {
            for (int i = 0; i < dotPosition.Count; i++)
            {
                if(item.GetPos == dotPosition[i].FirstNode || item.GetPos == dotPosition[i].SecondNode)
                {
                    item.SetNode(i, dotPosition[i].Color);
                }
            }
        }
    }

    public void OnTileEnter(Tile obj)
    {
        if (!chainTile.Contains(obj))
        {
            //chainTile[GetLastIndexChain].SetPipe(true);
            AddSChainTile(obj);
        }
        else
        {
            if (obj == chainTile[0]) return;
            //chainTile.RemoveAt(chainTile.Count-1);
            chainTile.Remove(obj);
        }
        currentTile = obj;

        if(lastExitTile != null)
        {
            lastExitTile.SetPipe(currentTile.GetPipeDirection);
        }

    }

    public void OnTileExit(Tile obj)
    {
        lastExitTile = obj;
    }

}
[System.Serializable]
public class DotPosition
{
    public Vector2 FirstNode;
    public Vector2 SecondNode;
    public Color Color = Color.red;
}

public class DirectionLine
{
    public Vector2 Pos;
    public PipePos Direction;

    public DirectionLine(Vector2 pos, PipePos direction)
    {
        this.Pos = pos;
        this.Direction = direction;
    }
}
[System.Serializable]
public class CompletedNode
{
    public int NodeId;
    public List<Tile> ChainDot = new List<Tile>();

    public CompletedNode(int Id, List<Tile> Chain)
    {
        NodeId = Id;
        ChainDot = new List<Tile>(Chain);
    }
}

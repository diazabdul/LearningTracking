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

    [SerializeField] int maxMove = 9;
    [SerializeField] int currentMove;
    [SerializeField] List<DotPosition> dotPosition = new List<DotPosition>();
    [SerializeField] List<Tile> listTile = new List<Tile>();
    [SerializeField] List<Tile> chainTile = new List<Tile>();

    int GetLastIndexChain => chainTile.Count - 1;

    //Helper
    [Header("Debugging")]
    [SerializeField] Tile currentTile;
    [SerializeField] Tile lastExitTile;
    [SerializeField] Tile firstNodeTile;
    [SerializeField] List<CompletedNode> completedNodes = new List<CompletedNode>();

    [SerializeField] bool onMouseHold;

    public bool MouseHold => onMouseHold;



    public System.Action<DirectionLine[]> ActivateNextTiles;
    public System.Action ResetChain;
    public System.Action<int> CancelCompletedChain;


    //Event For UI

    public delegate void InitializeUI(int maxMove, int targetComplete);
    public static event InitializeUI OnInitializeUI;

    public delegate void UpdateMove(int move);
    public static event UpdateMove OnUpdateMove;

    public delegate void UpdateCompleteChain(int complete);
    public static event UpdateCompleteChain OnUpdateCompleteChain;

    public void OnTileFirstClick(Tile tile)
    {
        AddSChainTile(tile);
        currentTile = tile;
        firstNodeTile = tile;
        onMouseHold = true;
    }
    public void AddSChainTile(Tile tile)
    {
        if (currentMove + 1 > maxMove) return;
        if (chainTile.Contains(tile)) return;

        chainTile.Add(tile);
        
        currentMove++;

        OnUpdateMove?.Invoke(currentMove);
        
        if(currentMove+1 <= maxMove)
            ActivateNextTile(tile.GetPos);
    }
    public void ResetChainTile()
    {
        chainTile.Clear();
        ResetChain?.Invoke();

        currentTile = null;
        lastExitTile = null;

        firstNodeTile = null;

        onMouseHold = false;

        currentMove = 0;

        OnUpdateMove?.Invoke(currentMove);
    }
    public void ResetCompleteChain(Tile tile)
    {
        foreach (var item in completedNodes)
        {
            if(item.NodeId == tile.GetTileId)
            {
                CancelCompletedChain.Invoke(item.NodeId);

                completedNodes.Remove(item);

                OnUpdateCompleteChain?.Invoke(completedNodes.Count);
                return;
            }
        }

        
    }
    public bool CheckBackwardMove(Tile tile)
    {
        if (chainTile[Mathf.Abs(GetLastIndexChain-1)] == tile)
        {
            currentTile = tile;
            lastExitTile = chainTile[GetLastIndexChain-1];
            tile.SetPipe(false);
            Debug.LogWarning("Backward Move !! Remove Tile " + chainTile[GetLastIndexChain].name);
            chainTile[GetLastIndexChain].SetSelected(false);
            chainTile.RemoveAt(GetLastIndexChain);

            currentMove--;

            OnUpdateMove?.Invoke(currentMove);
            return true;
        }
        return false;
    }
    public bool CheckWin(Tile tile)
    {
        if (firstNodeTile == null) return false;

        if((firstNodeTile != tile) && firstNodeTile.GetTileId == tile.GetTileId)
        {
            Debug.Log("Chain Completed");

            currentMove = 0;

            OnUpdateMove?.Invoke(currentMove);

            chainTile.Add(tile);

            foreach (var item in chainTile)
            {
                item.SetChainComplete();
                item.SetTileId(firstNodeTile.GetTileId);
            }
            completedNodes.Add(new CompletedNode(firstNodeTile.GetTileId, chainTile));

            OnUpdateCompleteChain?.Invoke(completedNodes.Count);

            ResetChain?.Invoke();

            chainTile.Clear();
            /*            currentTile = null;
                        lastExitTile = null;

                        firstNodeTile = null;*/
            
            onMouseHold = false;

            return true;
        }
        return false;
    }
    public void ActivateNextTile(Vector2 vec)
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


        OnInitializeUI?.Invoke(maxMove, dotPosition.Count);
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
        lastExitTile = currentTile;
        currentTile = obj;

        if(lastExitTile != null)
        {
            lastExitTile.SetPipe(currentTile.GetPipeDirection, GetNodeLineColor(firstNodeTile.GetTileId));
        }

    }

    public void OnTileExit(Tile obj)
    {
        lastExitTile = obj;
    }

    public Color GetNodeLineColor(int index)
    {
        return dotPosition[index].Color;
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

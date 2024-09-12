using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, System.IDisposable
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


    public System.Action<Vector2> OnHoverActivated;
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
                tempObj.OnSelected += OnTileClick;
                listTile.Add(tempObj);
            }
        }

        foreach (var item in listTile)
        {
            for (int i = 0; i < dotPosition.Count; i++)
            {
                if(item.GetPos == dotPosition[i].FirstNode || item.GetPos == dotPosition[i].SecondNode)
                {
                    item.SetNode(i);
                }
            }
        }
    }
    void OnTileClick(Vector2 vec)
    {
        Debug.Log("Clicked at Pos " + vec);

        OnHoverActivated(new Vector2(vec.x-1,vec.y));
        OnHoverActivated(new Vector2(vec.x,vec.y-1));
        
        OnHoverActivated(new Vector2(vec.x+1,vec.y));
        OnHoverActivated(new Vector2(vec.x,vec.y+1));
    }

    public void Dispose()
    {
        foreach (var item in listTile)
        {
            item.OnSelected -= OnTileClick;
        }
    }
}
[System.Serializable]
public class DotPosition
{
    public Vector2 FirstNode;
    public Vector2 SecondNode;
}

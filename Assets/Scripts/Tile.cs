using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    [SerializeField] int id = -1;
    [SerializeField] bool firstNode;
    [SerializeField] bool allowHighlight;
    [SerializeField] bool isSelected;
    [SerializeField] bool isDone;
    [SerializeField] SpriteRenderer tile;
    [SerializeField] SpriteRenderer circle;
    [SerializeField] Vector2 tilePos;
    [SerializeField] PipePos pipePosition;
    [SerializeField] PipeOnTile pipe;
    public Vector2 GetPos => tilePos;
    public PipePos GetPipeDirection => pipePosition;
    public int GetTileId => id;
    public int SetTileId(int id) => this.id = id; 
    public bool SetSelected(bool b) => isSelected = b;
    GameManager gameManager;

    public void Initialize(Vector2 pos, GameManager gameManager)
    {
        tilePos = pos;
        this.gameManager = gameManager;

        circle.enabled = false;
        pipe.SetActive(false);

        this.gameManager.ActivateNextTiles += OnActivateNextTile;
        this.gameManager.ResetChain += OnResetedChain;
        this.gameManager.CancelCompletedChain += OnCancelCompletedChain;
    }

    private void OnCancelCompletedChain(int obj)
    {
        if(id == obj)
        {
            if (!firstNode)
            {
                id = -1;
            }
            SetPipe(false);
            isDone = false;
        }
    }

    public void SetChainComplete()
    {
        isDone = true;
        allowHighlight = false;
        //firstNode = false;
        isSelected = false;
    }
    private void OnResetedChain()
    {
        if (!isDone)
        {
            //firstNode = false;
            allowHighlight = false;

            isSelected = false;
            SetPipe(false);
        }
    }

    private void OnActivateNextTile(DirectionLine[] obj)
    {
        if (isSelected) return;
        foreach (var item in obj)
        {
            if(item.Pos == tilePos)
            {
                allowHighlight = true;

                pipePosition = item.Direction;
                return;
            }
        }
        allowHighlight = false;
    }

    public void SetNode(int id, Color col)
    {
        this.id = id;

        circle.color = col;
        circle.enabled = true;

        firstNode = true;
    }
    public void SetPipe(bool b)
    {
        if (b)
        {
            pipe.transform.localRotation = new Quaternion(0, 0, (int)pipePosition, 0);
        }
        pipe.SetActive(b);
    }
    public void SetPipe(PipePos pipePos, Color col)
    {
        if (isDone && !firstNode) return;
        Debug.Log($"Set Pipe Tile {gameObject.name} Value = " + (int)pipePos);
        pipePosition = pipePos;
        Vector3 rotateTarget = new Vector3(0, 0, (int)pipePos);
        pipe.transform.DORotate(rotateTarget, 0);

        pipe.GetLine.color = col;
        pipe.SetActive(true);
    }
    private void OnMouseEnter()
    {
        if (isSelected)
        {
            if (gameManager.CheckBackwardMove(this))
            {
                gameManager.ActivateNextTile(tilePos);
            }
            return;
        }
        if (isDone && gameManager.MouseHold)
        {
            gameManager.ResetCompleteChain(this);
            gameManager.OnTileEnter(this);
            Debug.Log("Reset Complete Chain");
            return;
        }
        if ((allowHighlight && !isSelected))
        {
            //gameManager.OnTileExit(this);
            Debug.Log("Mouse Enter");
            isSelected = true;

            if (id != -1)
            {
                if (gameManager.CheckWin(this))
                {
                    SetPipe(GetReversePos(pipePosition), gameManager.GetNodeLineColor(id));
                }
            }
            else
                gameManager.OnTileEnter(this);

        }
    }
    private void OnMouseExit()
    {
        /*if (!isSelected && !firstNode && isDone) return;

        gameManager.OnTileExit(this);*/
    }
    private void OnMouseDown()
    {
        if (id < 0 || isDone) return;
        //firstNode = true;        
        gameManager.OnTileFirstClick(this);
        isSelected = true;
        Debug.Log("Mouse Down Tile = "+gameObject.name);
    }
    private void OnMouseUp()
    {
        if(firstNode && !isDone)
        {
            gameManager.ResetChainTile();
            Debug.Log("Reset Chain");
        }
    }

    private void OnDisable()
    {
        gameManager.ActivateNextTiles -= OnActivateNextTile;
        gameManager.ResetChain -= OnResetedChain;
    }
    PipePos GetReversePos(PipePos pos)
    {
        Debug.Log($"Reverse Pipe From {gameObject.name}");
        switch (pos)
        {
            case PipePos.Right:
                return PipePos.Left;

            case PipePos.Top:
                return PipePos.Down;

            case PipePos.Left:
                return PipePos.Right;

            case PipePos.Down:
                return PipePos.Top;

            default:
                return PipePos.Null;

        }
    }
}
public enum PipePos
{
    Null = -1,
    Right = 0,
    Top = 90,
    Left = 180,
    Down = 270
}

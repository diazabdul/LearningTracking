using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, System.IDisposable
{
    [SerializeField] int id = -1;
    [SerializeField] bool allowHighlight;
    [SerializeField] SpriteRenderer tile;
    [SerializeField] SpriteRenderer circle;
    [SerializeField] SpriteRenderer pipe;
    [SerializeField] Vector2 tilePos;

    public Vector2 GetPos => tilePos;

    public System.Action<Vector2> OnSelected;

    GameManager gameManager;
    public void Initialize(Vector2 pos, GameManager gameManager)
    {
        tilePos = pos;
        this.gameManager = gameManager;

        circle.enabled = false;
        pipe.enabled = false;

        this.gameManager.OnHoverActivated += HoverListener;
    }
    void HoverListener(Vector2 vec)
    {
        if(vec == tilePos)
        {
            allowHighlight = true;
        }
    }
    public void SetNode(int id)
    {
        this.id = id;

        circle.enabled = true;
    }
    private void OnMouseEnter()
    {
        if (allowHighlight)
        {
            pipe.enabled = true;
            Debug.Log("Mouse Enter");
        }
    }
    private void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
    }
    private void OnMouseDown()
    {
        if (id < 0) return;
        OnSelected?.Invoke(tilePos);
        Debug.Log("Mouse Down");
    }
    private void OnMouseUp()
    {
        Debug.Log("Mouse Up");
    }

    public void Dispose()
    {
        gameManager.OnHoverActivated -= HoverListener;
    }
}

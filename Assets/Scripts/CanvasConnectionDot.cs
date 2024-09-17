using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CanvasConnectionDot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentMove;
    [SerializeField] TextMeshProUGUI maxMove;
    [SerializeField] TextMeshProUGUI currentComplete;
    [SerializeField] TextMeshProUGUI targetComplete;

    private void OnEnable()
    {
        GameManager.OnInitializeUI += InitializeUI;
        GameManager.OnUpdateMove += UpdateMove;
        GameManager.OnUpdateCompleteChain += UpdateCompleteChain;
    }

    private void UpdateCompleteChain(int complete)
    {
        this.currentComplete.text = $"Chain {complete}";
    }

    private void UpdateMove(int move)
    {
        this.currentMove.text = $"Move {move}";
    }

    private void InitializeUI(int maxMove, int targetComplete)
    {
        this.maxMove.text = $"/ {maxMove}";
        this.targetComplete.text = $"/ {targetComplete}";

        this.currentComplete.text = $"Chain {0}";
        this.currentMove.text = $"Move {0}";

    }
}

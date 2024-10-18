
using UnityEngine;
using DG.Tweening;

public class PipeOnTile : MonoBehaviour
{
    [SerializeField] SpriteRenderer innerLine;

    public SpriteRenderer GetLine => innerLine;

    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
        if (b)
        {
            DoScale(1);
        }
        else
        {
            DoScale(0);
        }
        
    }
    public void DoScale(float target)
    {
        transform.DOScaleX(target, .25f);
    }
}

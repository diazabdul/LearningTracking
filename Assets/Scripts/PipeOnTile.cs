
using UnityEngine;

public class PipeOnTile : MonoBehaviour
{
    [SerializeField] SpriteRenderer innerLine;

    public SpriteRenderer GetLine => innerLine;

    public void SetActive(bool b) => gameObject.SetActive(b);
}

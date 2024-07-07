

using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class SideJokes : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject[] jokesList;

    [SerializeField] int clickCounter;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick(false);
        if(clickCounter+1 < jokesList.Length)
        {
            clickCounter++;
        }
        else
        {
            clickCounter = 0;
        }
        OnClick(true);
        transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f),.2f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.1f, .35f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, .2f);
    }

    private void Start()
    {
        jokesList[0].SetActive(true);
        for (int i = 1; i < jokesList.Length; i++)
        {
            jokesList[i].SetActive(false);
        }
    }
    void OnClick(bool t)
    {
        jokesList[clickCounter].SetActive(t);
    }
}

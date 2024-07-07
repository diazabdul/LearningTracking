using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics;
using System;
using DG.Tweening;


public class ButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image image;
    [SerializeField] string path;


    Transform tf;

    private void Start()
    {
        tf = gameObject.transform;
    }
    public void Initialize(string title, string path)
    {
        text.text = title;
        this.path = path;
    }
    public void Initialize(string title, string path, Sprite sprite)
    {
        text.text = title;
        this.path = path;
        image.sprite = sprite;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Color temp = new();
        temp = button.colors.pressedColor;
        text.color = temp;

        //Process.Start(Environment.CurrentDirectory + "/miscgame1folder/thegame1.exe");

        Process.Start(Environment.CurrentDirectory +"/Load/"+path);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color temp = new();
        temp = button.colors.highlightedColor;
        text.color = temp;


        tf.DOScale(1f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color temp = new();
        temp = button.colors.normalColor;
        text.color = temp;

        tf.DOScale(.9f, 1f);
    }

}

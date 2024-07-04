using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class Zenject_UI 
{
    Text text;

    public Zenject_UI(Text txt)
    {
        text = txt;
    }
    public void ChangeText(string str)
    {
        text.text = str;
    }
}

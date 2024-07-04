using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;


public class Zenject_Controller : IInitializable, System.IDisposable
{
    Zenject_UI ui;
    Zenject_Tickable tickable;
    public Zenject_Controller(Zenject_UI zUi, Zenject_Tickable tick)
    {
        ui = zUi;
        tickable = tick;
    }

    public void Dispose()
    {
        tickable.OnKeyClick -= TickListener;
    }

    public void Initialize()
    {
        tickable.OnKeyClick += TickListener;

    }

    private void TickListener(KeyCode obj)
    {
        ui.ChangeText(obj.ToString());
    }
}

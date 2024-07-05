using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Zenject_Tickable : ITickable
{
    public event System.Action<KeyCode> OnKeyClick;

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnKeyClick?.Invoke(KeyCode.Space);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnKeyClick?.Invoke(KeyCode.LeftShift);
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            OnKeyClick?.Invoke(KeyCode.Delete);
        }
    }
}

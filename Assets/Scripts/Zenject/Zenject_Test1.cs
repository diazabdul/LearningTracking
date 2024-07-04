using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Zenject_Test1 : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<int>().FromInstance(99);
        Container.Bind<Greeter>().AsSingle().NonLazy();
    }
}

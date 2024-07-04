using UnityEngine;
using Zenject;

public class Zenject_Test : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<string>().FromInstance("Hello Dickhead!");
        Container.Bind<Greeter>().AsSingle().NonLazy();

        Container.Bind<string>().FromNew().AsSingle().NonLazy();
        Container.Bind<Greeter>().AsSingle().NonLazy();
    }
}
public class Greeter
{
    public void IntegerGreeter(int i)
    {
        Debug.Log("Number = " + i);
    }
    public Greeter(string message)
    {
        Debug.Log(message);
    }
    
}

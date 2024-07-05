using UnityEngine;
using Zenject;

public class Zenject_Test : MonoInstaller
{
    public override void InstallBindings()
    {
        var debug = new Greeter("Helo World");
        Container.BindInstance(debug).NonLazy();
        
        var debug1 = new Greeter("Helo Dickhead");
        Container.BindInstance(debug);


        /*Container.Bind<string>().FromInstance("Hello Dickhead!");
        Container.Bind<Greeter>().AsSingle().NonLazy();

        Container.Bind<string>().FromNew().AsSingle().NonLazy();
        Container.Bind<Greeter>().AsSingle().NonLazy();*/
    }
}
public class Greeter
{
    public Greeter(string message)
    {
        Debug.Log(message);
    }
    
}

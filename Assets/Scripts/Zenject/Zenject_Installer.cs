using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class Zenject_Installer : MonoInstaller
{
    [SerializeField] Text textPressedKey;
    public override void InstallBindings()
    {

        // Tickable
        var tickable = new Zenject_Tickable();
        Container.BindInstance(tickable);
        Container.BindInterfacesTo(tickable.GetType()).FromInstance(tickable);

        // UI
        var ui = new Zenject_UI(textPressedKey);
        Container.BindInstance(ui);
        //Container.BindInterfacesTo(ui.GetType()).FromInstance(ui);

/*        // Controller
        var controller = new Zenject_Controller(textPressedKey*//*, tickable*//*);
        Container.BindInstance(controller);
        Container.BindInterfacesTo(controller.GetType()).FromInstance(controller);*/

        Container.BindInterfacesAndSelfTo<Zenject_Controller>().AsSingle().NonLazy();
        //Container.BindInterfacesAndSelfTo<Zenject_Tickable>().AsSingle().NonLazy();

    }
}



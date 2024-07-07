using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Launcher : MonoBehaviour
{
    
    //here were creating a text input box in the editor to type up the desired URLs we want to link to, you can
    //create as many as you want depending on what you want to link to.
    public string HomepageURL;
    public string TwitterURL;
    public string YoutubeURL;
    
    // Each one of these public voids is the function for each button referencing a game, im calling them launch_gamex but it can be whatever
    //helps you locate the specific function faster when in the unity editor on click event drop down menu
    public void Launch_Game1()
    {
        //this is opening the app, using the launcher location as root at the specified directory here
        //in the orange/red directory line type your directory for your game
        Process.Start(Environment.CurrentDirectory + "/miscgame1folder/thegame1.exe");
    }

    public void Launch_Game2()
    {
        Process.Start(Environment.CurrentDirectory + "/miscgame2folder/thegame2.exe");
    }

    public void Launch_Game3()
    {
        Process.Start(Environment.CurrentDirectory + "/miscgame3folder/thegame3.exe");
    }

    public void Launch_Game4()
    {
        Process.Start(Environment.CurrentDirectory + "/miscgame4folder/thegame4.exe");
    }

    public void Launch_Game5()
    {
        Process.Start(Environment.CurrentDirectory + "/miscgame5folder/thegame5.exe");
    }

    public void Launch_Game6()
    {
        Process.Start(Environment.CurrentDirectory + "/miscgame6folder/thegame6.exe");
    }
    
    public void Website()
    {
        Application.OpenURL(HomepageURL);
    }

    public void Twitter()
    {
        Application.OpenURL(TwitterURL);
    }

    public void Youtube()
    {
        Application.OpenURL(YoutubeURL);
    }


    //this one quits the launcher
    public void QuitLauncher()
    {
        Application.Quit();
    }

   
}

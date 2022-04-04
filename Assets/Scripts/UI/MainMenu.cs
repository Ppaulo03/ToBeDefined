using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : ChangeScene
{ 
    public void Quit() => Application.Quit();
    
    public void StartGame() => ChooseLevel("Level1");

}

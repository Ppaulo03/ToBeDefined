using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pause_UI = null;
    [SerializeField] private GameObject inGame_UI = null;
    [System.NonSerialized] public static bool isPaused = false;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pause_UI.SetActive(false);
        inGame_UI.SetActive(true);
        if(!DialogController.isEnabled) Time.timeScale = 1f;
        isPaused = false;
    }
    
    public void Quit()
    {
        Time.timeScale = 1f;
        GetComponent<ChangeScene>().ChooseLevel("MainMenu");
    }

    private void Pause()
    {
        pause_UI.SetActive(true);
        inGame_UI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }
}

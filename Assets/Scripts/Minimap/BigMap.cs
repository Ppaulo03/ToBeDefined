using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMap : MonoBehaviour
{
    [SerializeField] private Signal close = null;
    
    private void OnEnable() 
    {
        Time.timeScale = 0f;
        PauseMenu.isPaused = true;
    }

    private void Update() 
    {
        if(Input.GetKeyDown("t")) PauseMenu.isPaused = false;
        if(!PauseMenu.isPaused)
        {
            PauseMenu.isPaused = false;
            Time.timeScale = 1f;
            close.Raise();
            gameObject.SetActive(false);
        }
    }

}


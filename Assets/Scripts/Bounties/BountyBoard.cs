using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BountyBoard : MonoBehaviour
{

    [SerializeField] private List<BountyPoster> posters = new List<BountyPoster>();
    private void OnEnable()
    {
        Time.timeScale = 0f;
        PauseMenu.isPaused = true;
        SetBounties();
    }
    
    private void Update() 
    {
        if(!PauseMenu.isPaused)
        {   
            foreach(BountyPoster poster in posters) poster.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        
    }
    
    private void SetBounties()
    {
        List<Bounty> bounties = DataManager.manager.bounties_list;
        int cont = 0;
        foreach(Bounty bt in bounties)
        {
            if(cont >= posters.Count) break;
            if(bt.status != BountyStatus.Completa)
            {
                posters[cont].SetBounty(bt);
                cont ++;
            }
        }
    }

}

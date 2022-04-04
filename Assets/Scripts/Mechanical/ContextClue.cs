using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContextClue : SignalListener
{
    [SerializeField] private GameObject context = null;
    private GameObject clone = null;
    private Transform holder = null;
    protected bool inArea = false;

    private void Update() 
    {
        if(inArea && context != null)
        {
            if(holder.childCount == 0)
            {
                clone = Instantiate(context, holder.position, Quaternion.identity);
                clone.transform.parent = holder;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!inArea)
        {
            if (other.gameObject.tag == "Player" && !other.isTrigger) 
            {
                holder = other.gameObject.transform.GetChild(0);
                inArea = true;
            }      
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(inArea)
        {
            if (other.gameObject.tag == "Player" && !other.isTrigger)
            {
                if(clone != null) Destroy(clone);
                inArea = false;
            }       
        }
    }
    
    public override void onSignalRaised()
    {
        if(inArea && clone != null) signalEvent.Invoke();
    }

    
}

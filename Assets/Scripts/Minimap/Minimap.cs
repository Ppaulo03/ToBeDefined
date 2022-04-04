using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameObject player = null;

    private void Start() => StartCoroutine(LateStart(1));
    
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        player = GameObject.FindGameObjectWithTag("Player");
    }
 
    private void Update() 
    {
        if(player != null)
            transform.position = new Vector2 (player.transform.position.x/20f,  player.transform.position.y/12f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        other.GetComponent<SpriteRenderer>().enabled = true;
        other.GetComponent<Collider2D>().enabled = false;
    }
}

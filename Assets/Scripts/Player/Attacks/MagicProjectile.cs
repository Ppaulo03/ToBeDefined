using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : Projectile
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(!other.isTrigger)
        {
            if (other.gameObject.tag == "Enemy") other.GetComponent<Enemy>().GetHurt(damage);
            if(destroy_on_contact && other.gameObject.tag != "Player") Destroy(gameObject);
        }
    }
}
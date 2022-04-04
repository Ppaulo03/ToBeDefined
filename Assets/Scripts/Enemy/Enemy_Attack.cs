using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : Projectile
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.isTrigger)
        {
            if (other.gameObject.tag == "Player" ) other.GetComponent<Player>().GetHurt(damage, direction);
            if(destroy_on_contact && other.gameObject.tag != "Enemy") Destroy(gameObject);
        }
    }
}

using System.Collections;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    [SerializeField] private Inventory inventory = null;
    public Weaponry weapon = null;
    private float drop_life = 300;

    private void Start() => StartCoroutine(despawnCo());

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player" && !other.isTrigger)
        {
            if(inventory.addWeapon(weapon)) Destroy(gameObject);
        } 
    }

    private IEnumerator despawnCo()
    {
       
        yield return new WaitForSeconds(drop_life);
        Destroy(gameObject);

    }
}

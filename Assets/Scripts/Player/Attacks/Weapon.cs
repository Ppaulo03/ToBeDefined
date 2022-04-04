using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private GameObject destroy_effect = null;

    public float damage_Base = 5f;
    public float time = 1f;

    [System.NonSerialized] public float damage = 0f;
    private bool effect = false;
    
    private void Start() => Destroy(gameObject, time);

    private void OnDestroy() 
    {
       if(effect) Instantiate(destroy_effect, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            StartCoroutine(hitCo());
            other.gameObject.GetComponent<Enemy_Base>().GetHurt(damage_Base + damage);
            
        }    
    }

    private IEnumerator hitCo()
    {
        yield return new WaitForSeconds(time - 0.2f);
        if(inventory.WeaponHit() && destroy_effect != null) effect = true;
            
        
    }
}

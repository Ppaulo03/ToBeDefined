using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Base : MonoBehaviour
{
    protected Dictionary<string,float> effects = new Dictionary<string,float>
    {
        {"Damage Resistance", 1},
        {"Speed", 1},
    };

    [Header("Defense")]
    [SerializeField] protected float resistence = 20f;
    [SerializeField] protected float max_health = 10f;

    protected float knock_back_force = 0f, health = 10f;
    protected bool invunerable = false, noticing = false;


    protected Transform target = null;
    [SerializeField] protected GameObject[] drops = null;

    
    protected Rigidbody2D rb2d;
    protected SpriteRenderer sprite_renderer;
    protected Animator animator;
    protected bool enable_cooldown = false;
    
    protected virtual void OnEnable() => StartCoroutine(enableCo());

    protected virtual IEnumerator enableCo()
    {
        enable_cooldown = true;
        yield return new WaitForSeconds(1f);
        enable_cooldown = false;
    }

    protected virtual void Start()
    {
        health = max_health;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void FixedUpdate() 
    {
        if(!enable_cooldown)
        {
            if(invunerable) KnockBack();
            else if(noticing) Notice();
        }
        
    }

    public virtual void GetHurt(float damage)
    {
        if(!invunerable)
        {
            Hurted();
            health -= (damage * effects["Damage Resistance"]);
            if (health < 0) 
            {
                int rand = Random.Range(0, drops.Length);
                if(drops[rand] != null) Instantiate(drops[rand], transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                knock_back_force = damage/resistence;
                StartCoroutine(flickerCo());
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player") 
        {
            target = other.gameObject.transform;
            noticing = true;
        }
    }
    
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") target = other.gameObject.transform;
    }
    
    protected virtual void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            StopAnimation();
            noticing = false;
        }
    }


    protected abstract void StopAnimation();
    protected abstract void Hurted();
    protected abstract void KnockBack();
    protected abstract void Notice();
    protected abstract IEnumerator flickerCo();


   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private DataManager mg = null;
    [SerializeField] private Player_Stats stats = null;
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private GameObject punch_obj = null;

    [Header("Signals")]
    [SerializeField] private Signal dead = null;
    [SerializeField] private Signal open_book = null;
    [SerializeField] private Signal open_inventory = null;
    [SerializeField] private Signal context_clue = null;
    [SerializeField] private Signal bigMap = null;

   
    private float knock_force = 0f;
    private bool invunerable = false, attacking = false, dashing = false;
    private Vector2 knock_direction = Vector2.zero, direction = Vector2.down;
    
    
    //Components
    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer sprite_renderer;
    private ScriptableTimer scriptable_timer;

    private void Awake() => mg.InstantiateManager();

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        
        try
        {
            scriptable_timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<ScriptableTimer>();
        } catch(NullReferenceException ex)
        {
            Debug.Log(ex);
        }

        foreach (Magic mgc in inventory.magics) 
        {
            mgc.ready = true;
            mgc.rest_time = 0f;
        }
    }

    private void Update() 
    {
        if(!attacking && !invunerable && !PauseMenu.isPaused) GetInput();
        stats.health.current += stats.health.recovery * Time.deltaTime;
        stats.energy.current += stats.energy.recovery * Time.deltaTime;
        stats.stamina.current += stats.stamina.recovery * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(invunerable) KnockBack();
        else if(dashing) DashMove();
        else if(!attacking) Move();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown("space")) Attack();
        else if(Input.GetKeyDown("left ctrl")) Magic();
        else if(Input.GetKeyDown("left shift" )) Dash();
        else if(Input.GetKeyDown("m")) inventory.UseItem();
        else if(Input.GetKeyDown("o")) inventory.ChangeItem();
        else if(Input.GetKeyDown("p")) inventory.ChangeWeapon();
        else if(Input.GetKeyDown("t")) bigMap.Raise();
        //else if(Input.GetKeyDown("b")) open_book.Raise();
        else if(Input.GetKeyDown("i")) open_inventory.Raise();
        else if(Input.GetKeyDown("f")) context_clue.Raise();
    }

    public Vector2 getDirection()
    {
        return direction;
    }
    
    //=================================== Attack Functions ===================================================//

    private void Attack()
    {
        
        StartCoroutine(attackCo());

        GameObject clone;
        Vector2 offset;
        Vector3 rotation;

        if(direction.y > 0)
        {
            offset = new Vector2(-0.2f, 1.25f); rotation = new Vector3(0,0,180);
        }
        else if(direction.y < 0) 
        {
            offset = new Vector2(-0.2f, -0.25f); rotation = new Vector3(0,0,0);
        }
        else if(direction.x > 0)
        {
            offset = new Vector2(0.75f, 0.25f); rotation = new Vector3(0,0,90);
        }
        else
        {
            offset = new Vector2(-0.75f, 0.25f); rotation = new Vector3(0,0,-90);
        } 
        if(inventory.weapon.name == "")clone = Instantiate(punch_obj, rb2d.position + offset, Quaternion.Euler (rotation));
        else clone = Instantiate(inventory.weapon.obj, rb2d.position + offset, Quaternion.Euler (rotation));
        clone.GetComponent<Weapon>().damage = stats.strength;
        clone.transform.parent = gameObject.transform;
     
    }

    private void Magic()
    {
        if(inventory.magic != null)
        {
            StartCoroutine(magicCo());
            inventory.magic.Cast(stats, rb2d.position, direction, scriptable_timer);   
        }     
    }
    
    //=================================== Movement Functions ===================================================//
    private void Move()
    {
        Vector2 new_direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        new_direction = new_direction.normalized;

        if (new_direction == Vector2.zero) animator.SetBool("Walking", false);
        else
        {
            direction = new_direction;
            animator.SetBool("Walking", true);
            animator.SetFloat("Direction X", direction.x);
            animator.SetFloat("Direction Y", direction.y);
            rb2d.MovePosition(rb2d.position + direction*stats.speed*Time.fixedDeltaTime);
        }
    }

    private void Dash()
    {
        if(stats.stamina.current >= stats.dash.cost)
        {
            stats.stamina.current -= stats.dash.cost;
            Instantiate(stats.dash.effect, rb2d.position, Quaternion.identity);
            StartCoroutine(dashCo());
        }
    }
    private void DashMove()
    {
        rb2d.MovePosition(rb2d.position + direction*stats.dash.speed*Time.fixedDeltaTime);
    }

    
    //=================================== Damage Functions ===================================================//
    public void GetHurt(float damage, Vector2 direction)
    {
        if(!invunerable)
        {
            stats.health.current -= damage;
            if (stats.health.current <= 0)
            {
                stats.health.current = stats.health.max;
                TimelineManager.animaion_flags["Dead"] = true;
                dead.Raise();
            }
            else
            {
                knock_direction = direction;
                knock_force = damage/stats.resistence;
                StartCoroutine(flickerCo());
            }
        }
    }

    private void KnockBack() => rb2d.MovePosition(rb2d.position + knock_direction*knock_force*Time.fixedDeltaTime);
    

    //=================================== Coroutines ===================================================//
    private IEnumerator attackCo()
    {
        attacking = true;
        animator.SetBool("Attack", true);

        float time;
        if(inventory.weapon.name == "") time = 0.25f;
        else time = inventory.weapon.info.time;
        yield return new WaitForSeconds(time);
        attacking = false;
        animator.SetBool("Attack", false);
    }

    private IEnumerator magicCo()
    {
        attacking = true;
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(inventory.magic.cast_time);
        attacking = false;
        animator.SetBool("Attack", false);
    }

    private IEnumerator dashCo()
    {
        dashing = true;
        animator.SetBool("Walking", true);
        yield return new WaitForSeconds(stats.dash.time);
        dashing = false;
    }

    private IEnumerator flickerCo()
    {
        invunerable = true;
        for(int i = 0; i <= 25; i++)
        {
            sprite_renderer.enabled = !sprite_renderer.enabled;
            yield return new WaitForSeconds(0.01f);
        }
        invunerable = false;
    }
}

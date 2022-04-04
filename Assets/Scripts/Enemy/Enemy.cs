using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Enemy_Base
{

    [Header("Movement")]
    [SerializeField] private float move_speed = 12f;


    [Header("Attack")]
    [SerializeField] private GameObject attack = null;
    [SerializeField] private bool rotate = false;
    
    [SerializeField] private float attack_strength = 5f;
    [SerializeField] private float attack_radius = 12f;
    [SerializeField] private float attack_time = 2f;
    [SerializeField] private float impulse = 0f;

    private bool can_attack = true;

    //=================================== Movement ===================================================//
    
    private void Attack(Vector2 direction)
    {
        StartCoroutine(attackCo());
        
        float angle;
        if(!rotate) angle = 0;
        else angle = (Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg) - 90;      
        
        GameObject clone = Instantiate(attack, rb2d.position + direction,  Quaternion.Euler (new Vector3(0,0,angle)));
        clone.GetComponent<Enemy_Attack>().SetStatus(attack_strength, impulse, direction);
        clone.transform.parent = gameObject.transform;
        
    }
    
    private void Move(Vector2 direction)
    {
        animator.SetBool("Walking",true);
        rb2d.MovePosition(rb2d.position + direction*move_speed*effects["Speed"]*Time.fixedDeltaTime);
    }
    
    protected override void Notice()
    {
        Vector2 direction = target.position - transform.position;
        float distance = direction.magnitude;
        direction = direction.normalized;
        animator.SetFloat("Direction X", direction.x);
        animator.SetFloat("Direction Y", direction.y);
       
        if(can_attack)
        {  
            if(distance <= attack_radius) Attack(direction);
            else Move(direction);
        }
    }
    
    protected override void StopAnimation() => animator.SetBool("Walking", false);

    //=================================== Damage Functions ===================================================//
    protected override void Hurted(){}
    protected override void KnockBack()
    {
        if(target == null) target = GameObject.FindGameObjectWithTag("Player").transform;
        Vector2 direction = target.position - transform.position;
        direction = direction.normalized;
        rb2d.MovePosition(rb2d.position - direction*knock_back_force*Time.fixedDeltaTime);
    }

    //=================================== Coroutines ===================================================//
    private IEnumerator attackCo()
    {
        animator.SetBool("Walking", false);
        can_attack = false;
        yield return new WaitForSeconds(attack_time);
        can_attack = true;
    }
    
    protected override IEnumerator flickerCo()
    {
        animator.SetBool("Walking", false);
        invunerable = true;
        for(int i = 0; i <= 25; i++)
        {
            sprite_renderer.enabled = !sprite_renderer.enabled;
            yield return new WaitForSeconds(0.01f);
        }
        invunerable = false;
    }

}

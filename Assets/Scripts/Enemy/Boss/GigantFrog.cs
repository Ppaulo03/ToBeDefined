using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GigantFrog : Enemy_Base
{
    [Header("Movement")]
    [SerializeField] private float jump_speed = 12f;
    

    [Header("Attack")]
    [SerializeField] private GameObject attack = null;
    [SerializeField] private float attack_strength = 5f;
    [SerializeField] private float attack_radius = 12f;
    [SerializeField] private float charge_time = 4f;
    [SerializeField] private float attack_time = 2f;

    [SerializeField] private GameObject jump = null;
    [SerializeField] private float jump_strength = 5f;
    [SerializeField] private float jump_radius = 12f;
    [SerializeField] private float jump_time = 2f;


    [SerializeField] private float impulse = 0f;

    private bool attacking = false, jumping = false;
   
    protected override void FixedUpdate() 
    {
        if(!enable_cooldown)
        {
            if(invunerable) KnockBack();
            else if(jumping) Jump();
            else if(noticing) Notice();
        }
        
    }

    //=================================== Movement ===================================================//
    protected override void StopAnimation(){}
    protected override void Notice()
    {
        Vector2 direction = target.position - transform.position;
        float distance = direction.magnitude;
        direction = direction.normalized;
        
        if(target.position.x > transform.position.x) sprite_renderer.flipX = true;
        else sprite_renderer.flipX = false;
        
        if(!attacking)
        {  
            if(distance <= jump_radius) StartCoroutine(jumpCo(direction));
            else if(distance >= attack_radius) StartCoroutine(attackCo());
            else
            {
                if(Random.value < 0.5f) StartCoroutine(jumpCo(direction));
                else StartCoroutine(attackCo());
            }
        }
    }
    private void Jump()
    {
        Vector2 direction = target.position - transform.position;
        direction = direction.normalized;
        rb2d.MovePosition(rb2d.position + direction*jump_speed*Time.fixedDeltaTime);
    }


    //=================================== Damage Functions ===================================================//
    protected override void Hurted()
    {
        StopAllCoroutines();
        attacking = false;
        jumping = false;
    }
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
        animator.SetTrigger("Attack");
        attacking = true;
        yield return new WaitForSeconds(charge_time);
        int times = (int)((1 - ((health/max_health)))*10);
        for(int i = 0; i <= times; i++)
        {
            Vector2 direction = target.position - transform.position;
            direction = direction.normalized;
            float angle;
            angle = (Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg) - 90;      

            GameObject clone = Instantiate(attack, new Vector2(rb2d.position.x,rb2d.position.y + 1.3f),  Quaternion.Euler (new Vector3(0,0,angle)));
            clone.GetComponent<Enemy_Attack>().SetStatus(attack_strength, impulse, direction);
            clone.transform.parent = gameObject.transform;

            yield return new WaitForSeconds(attack_time);
        }
        attacking = false;
    }

    private IEnumerator jumpCo(Vector2 direction)
    {
        animator.SetTrigger("Jump");
        attacking = true;
        jumping = true;
        yield return new WaitForSeconds(jump_time);  

        GameObject clone = Instantiate(jump, rb2d.position,  Quaternion.identity);
        clone.GetComponent<ParticleAttack>().SetStatus(jump_strength, direction);
        clone.transform.parent = gameObject.transform;
        
        jumping = false;
        yield return new WaitForSeconds(0.75f);
        attacking = false;
    }

    protected override IEnumerator flickerCo()
    {
        animator.SetTrigger("Hit");
        invunerable = true;
        yield return new WaitForSeconds(1f);

        Vector2 direction = target.position - transform.position;
        direction = direction.normalized;

        GameObject clone = Instantiate(jump, rb2d.position,  Quaternion.identity);
        clone.GetComponent<ParticleAttack>().SetStatus(jump_strength, direction);
        clone.transform.parent = gameObject.transform;

        invunerable = false;
    }


}

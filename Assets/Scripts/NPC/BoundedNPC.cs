using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedNPC : ContextClue
{ 
    [SerializeField] private float speed  = 2f;

    [SerializeField] private float min_move_time = 1f;
    [SerializeField] private float max_move_time = 1.5f;

    [SerializeField] private float min_wait_time  = 0.1f;
    [SerializeField] private float max_wait_time  = 0.5f;

    [SerializeField] private Collider2D bound = null;
     
    private Vector2 directionVector;
    private Rigidbody2D rb2d;
    private Animator animator;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        ChangeDirection();
    }

    void Update()
    {
        if(!inArea) Move();
        else animator.SetBool("Walking", false);
    }

    private void Move()
    {
        if (directionVector == Vector2.zero) animator.SetBool("Walking", false);
        else
        {
            animator.SetFloat("Direction X", directionVector.x);
            animator.SetFloat("Direction Y", directionVector.y);
            animator.SetBool("Walking", true);
            Vector3 tmp = rb2d.position + directionVector*speed*Time.deltaTime;
            if (bound.bounds.Contains(tmp)) rb2d.MovePosition(tmp);
            else 
            {
                StopAllCoroutines();
                ChooseDiferentDirection(directionVector);
            }
        }
    }

    private void ChooseDiferentDirection(Vector2 actualDirection)
    {
        ChangeDirection();
        while(actualDirection == directionVector) ChangeDirection();
    }

    private void ChangeDirection()
    {
        int direction = Random.Range(0, 4);
        switch(direction)
        {
            case 0:
                directionVector = Vector2.right;            
                break;
            case 1:
                directionVector = Vector2.up;
                break;
            case 2:
                directionVector = Vector2.left;
                break;
            case 3:
                directionVector = Vector2.down;
                break;
        } 
        StartCoroutine(changeCo());
        
    }

    private IEnumerator changeCo()
    {
        float time = Random.Range(min_move_time, max_move_time);

        for(int i = 0; i <= time*10; i ++)
        {
            while(inArea) yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        
        StartCoroutine(waitCo());
    }

    private IEnumerator waitCo()
    {
        Vector2 tmp = directionVector;
        directionVector = Vector2.zero;            
        float time = Random.Range(min_wait_time, max_wait_time);
        
        for(int i = 0; i <= time*10; i ++)
        {
            while(inArea) yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        ChooseDiferentDirection(directionVector);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        StopAllCoroutines();
        ChooseDiferentDirection(directionVector);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        StopAllCoroutines();
        ChooseDiferentDirection(directionVector);   
    }
    
}

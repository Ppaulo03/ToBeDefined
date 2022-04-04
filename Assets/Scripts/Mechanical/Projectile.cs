using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5;
    [SerializeField] private float rotateOffset = 0f;
    [SerializeField] protected bool destroy_on_contact = true;

    [System.NonSerialized] public float damage = 0f, impulse = 0f;
    [System.NonSerialized] public Vector2 direction = Vector2.zero;
    private Rigidbody2D rb2d;

    private void Start() 
    {
        rb2d = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);

        transform.rotation = Quaternion.Euler(0,0,transform.localRotation.eulerAngles.z - rotateOffset);
    }

    private void FixedUpdate() => rb2d.MovePosition(rb2d.position + direction*impulse*Time.fixedDeltaTime);
    
    public void SetStatus(float damage, float impulse, Vector2 direction)
    {
        this.damage = damage;
        this.impulse = impulse;
        this.direction = direction;
    }

}

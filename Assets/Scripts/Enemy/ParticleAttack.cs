using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttack : MonoBehaviour
{
    [System.NonSerialized] public float damage = 0f;
    [System.NonSerialized] public Vector2 direction = Vector2.zero;

    public void SetStatus(float damage, Vector2 direction)
    {
        this.damage = damage;
        this.direction = direction;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Player" ) other.GetComponent<Player>().GetHurt(damage, direction);
    }

}

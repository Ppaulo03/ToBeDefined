using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] private Sprite open_img = null;
    [SerializeField] private GameObject[] objects = null;
    public void Open()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = open_img;
        int rand = Random.Range(0, objects.Length);
        if(objects[rand] != null)
        {
            GameObject clone = Instantiate(objects[rand], transform.position - new Vector3(0,0.5f,0), Quaternion.identity);
            clone.transform.parent = gameObject.transform;
        }
    }
}

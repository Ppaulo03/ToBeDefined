using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] objects = null;

    void Start()
    {
        int rand = Random.Range(0, objects.Length);
        if(objects[rand] != null)
        {
            GameObject clone = Instantiate(objects[rand], transform.position, Quaternion.identity);
            clone.transform.parent = gameObject.transform;
        }
    }

}


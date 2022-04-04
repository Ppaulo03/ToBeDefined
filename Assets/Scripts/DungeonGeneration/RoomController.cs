using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject virtualCamera = null;
    [SerializeField] private List<ListGameObjects> list_objects = null;
    [SerializeField] private List<BountyList> bounties;
    [SerializeField] private int maxSpawnedBounties = 3;

    [System.NonSerialized] public int type = 0;
    private bool initiated = false;

    private void Start() 
    {
        virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            if(!initiated)
            {
                initiated = true;
                if(type >= 0 && type < list_objects.Count)
                {
                    if(DataManager.manager.spawned_bounties < maxSpawnedBounties)
                    {
                        if(type < bounties.Count)
                        {
                            List<Bounty> bt_list = bounties[type].bounties;
                            int rand = Random.Range(0, bt_list.Count + 5);
                            if(rand < bt_list.Count)
                            {
                                if(bt_list[rand].status == BountyStatus.Ativa) GenerateRandom(list_objects[type].objects); // Generate Bountie
                                else GenerateRandom(list_objects[type].objects);
                            }else GenerateRandom(list_objects[type].objects);
                        }else GenerateRandom(list_objects[type].objects);
                    }else GenerateRandom(list_objects[type].objects);
                }       
            }
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void GenerateRandom(GameObject[] objects)
    {
        int rand = Random.Range(0, objects.Length);
        if(objects[rand] != null)
        {
            GameObject clone = Instantiate(objects[rand], transform.position, Quaternion.identity);
            clone.transform.parent = gameObject.transform;
        }
    }

}

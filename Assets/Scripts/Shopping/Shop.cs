using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Shop : MonoBehaviour
{
    
    [Serializable] protected struct Merchandise
    {
        public GameObject slot;
        public float price;
        public Item item;
    }
    [SerializeField] protected Inventory player_inventory = null;
    [SerializeField] protected List<Merchandise> merchandises = null;

}

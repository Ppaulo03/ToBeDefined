using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponShop : BookBase
{
    [SerializeField] private Transform slots = null;
    [SerializeField] private Transform Buying_Selling = null;
    private List<Weaponry> weapons_list = null;
    private List<int> indexes = new List<int>();
    
    
    public float percentage = 1;
    public bool buying = true;

    protected override void SetMaxIndex() => max_index = 0;

    protected override void Start()
    {
        weapons_list = DataManager.manager.weapons_list;
        SetToBuy();
    }

    protected override void SetInfo()
    {
        List<Weaponry> list;
        if(buying) list = weapons_list;
        else list = inventory.weapons;

        int i = index*6;
        foreach(Transform item in slots)
        {
            if(i >= indexes.Count) item.gameObject.SetActive(false);
            else
            {
                item.gameObject.SetActive(true);
                Weaponry weapon = list[indexes[i]];
                if(weapon.name == "") 
                {
                    item.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = "Sold";
                    item.GetChild(2).GetComponent<Button>().enabled = false;
                }
                else
                {   
                    item.GetChild(0).gameObject.GetComponent<Image>().sprite = weapon.image;

                    item.GetChild(1).gameObject.GetComponent<Text>().text = $"{weapon.name}\n"+
                                                                            $"Damage: {weapon.info.damage_Base}\n" +
                                                                            $"Attack Time: {weapon.info.time}\n"+
                                                                            $"Durability: {weapon.durability}";
                    
                    float condicion = weapon.durability/weapon.max_durability;
                    string price = "$" + ((int) Mathf.Ceil(weapon.price*percentage*condicion)).ToString();
                    item.GetChild(2).GetComponent<Button>().enabled = true;
                    item.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = price;

                    if(buying)  item.GetChild(2).GetComponent<Image>().color = new Color(0f, 255f, 0f, 255f);
                    else item.GetChild(2).GetComponent<Image>().color = new Color(255f, 0f, 0f, 255f);

                }
            }
            i++;
        }
         
        if (index >= max_index - 1) nxtBtn.SetActive(false);
        else nxtBtn.SetActive(true);
        if (index <= 0) bkBtn.SetActive(false);
        else bkBtn.SetActive(true);
    }

    public void SetToSell()
    {
        indexes = new List<int>();
        for(int i = 0; i < inventory.weapons.Count; i++)
            if(inventory.weapons[i].name != "") indexes.Add(i);
        
        buying = false;

        max_index = (int) Mathf.Ceil(indexes.Count/6f);

        Buying_Selling.GetComponent<Image>().color = new Color(255f, 0f, 0f, 255f);
        Buying_Selling.GetChild(0).GetComponent<Text>().text = "Selling";
        
        percentage = 0.75f;
        index = 0;
        SetInfo();
    }

    public void SetToBuy()
    {    
        indexes = new List<int>();
        for(int i = 0; i < weapons_list.Count; i++)
            if(weapons_list[i].name != "") indexes.Add(i);

        buying = true;
        max_index = (int) Mathf.Ceil(indexes.Count/6f);

        Buying_Selling.GetComponent<Image>().color = new Color(0f, 255f, 0f, 255f);
        Buying_Selling.GetChild(0).GetComponent<Text>().text = "Buying";

        percentage = 1;
        index = 0;
        SetInfo();
    }

    public void BuyOrSell(int n)
    {
        if(buying) Buy(n);
        else Sell(n);
    }

    public void Buy(int n)
    {
        Weaponry item = weapons_list[indexes[index*6 + n]];
        if (item.price <= inventory.money )
        {
            Weaponry new_item = new Weaponry(item); 
            if(inventory.addWeapon(new_item)) inventory.money -= item.price;
        }
    }

    public void Sell(int n)
    {
        Weaponry item = inventory.weapons[index*6 + n];
        float condicion = item.durability/item.max_durability;
        inventory.money += (int) Mathf.Ceil(item.price*percentage*condicion);
        inventory.weapons[indexes[index*6 + n]] = new Weaponry();
        SetInfo();
    }

}

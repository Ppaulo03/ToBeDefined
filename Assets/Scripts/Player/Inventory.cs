using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObject/Inventory", order = 0)]
public class Inventory : ScriptableObject, ISerializationCallbackReceiver
{

    public List<Item> items = new List<Item>();
    public List<Weaponry> weapons = new List<Weaponry>();
    public List<Magic> magics = new List<Magic>();
    [SerializeField] private Player_Stats stats = null;

    public int[] item_index = new int[3] {-1,-1,-1}, weapon_index = new int[3] {-1,-1,-1};
    public int magic_index = 0, weapon_chosen = 0, item_chosen = 0, max_capacity = 16;
    
    public int _money = 0;
    public int money
    {
        get{return _money;}
        set{
            if (value <= 0) _money = 0;
            else if (value <= 999999999) _money = value;
            else _money = 999999999;
            }
    }
   
    public void OnBeforeSerialize()
    {
        for(int i = items.Count; i < max_capacity; i++) items.Add(new Item());
        for(int i = weapons.Count; i < max_capacity; i++) weapons.Add(new Weaponry());
    }

    public void OnAfterDeserialize()
    {
        magic_index = 0; 
        weapon_chosen = 0;
    }

    public Magic magic { get{ if(magics.Count > 0)return magics[magic_index];
                                else return null;}}

    public Weaponry weapon { get{ if(weapon_index[weapon_chosen] < 0) return new Weaponry();
                                else return weapons[weapon_index[weapon_chosen]]; }}

    public Item item_obj { get{ if(item_index[item_chosen] < 0) return new Item();
                                else return items[item_index[item_chosen]]; }}

    public bool WeaponHit()
    {
        weapons[weapon_index[weapon_chosen]].durability -= 1;
        if(weapons[weapon_index[weapon_chosen]].durability <= 0) 
        {
            weapons[weapon_index[weapon_chosen]] = new Weaponry();
            return true;
        }
        return false;
    }

    public (bool, int) addItem(Item new_item)
    {   

        List<Item> item = items.FindAll(t => t.name == new_item.name);
        foreach (Item it in item)
        {
            it.qtd += new_item.qtd;

            if(it.qtd > it.max_qtd) 
            {
                new_item.qtd = it.qtd - it.max_qtd;
                it.qtd = it.max_qtd;
            }
            else return (true, 0);
        }
        
        int item_index = items.FindIndex(t => t.name == "");
        
        if(item_index == -1) return (false, new_item.qtd);
        else items[item_index] = new_item;
        items[item_index].stats = stats;
        
        return (true, 0);   
    }

    public bool addWeapon(Weaponry wp)
    {   
        int new_weapon_index = weapons.FindIndex(t => t.name == "");
        if(new_weapon_index == -1) return false;
        else weapons[new_weapon_index] = wp;
        return true;
    }
    
    public void removeItem(string item_name, int value)
    {

        int item_index = items.FindIndex(t => t.name == item_name);
        if(item_index != -1) 
        {
            items[item_index].qtd -= value;
            if(items[item_index].qtd <= 0)
            {
                value = -items[item_index].qtd;
                items[item_index] = new Item();
                if(value > 0) removeItem(item_name, value);
            }
        }
    
    }

    public void UseItem()
    {
        if(item_index[item_chosen] == -1) return;
        items[item_index[item_chosen]].Use();
        if(items[item_index[item_chosen]].qtd <= 0) 
        {
            items[item_index[item_chosen]] = new Item();
            item_index[item_chosen] = -1;
        }
    }

    public void ChangeItem()
    {
        item_chosen += 1;
        if(item_chosen >= 3) item_chosen = 0;
    }

    public void ChangeWeapon()
    {
        weapon_chosen += 1;
        if(weapon_chosen >= 3) weapon_chosen = 0;
    }
}



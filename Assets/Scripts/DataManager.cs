using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataManager", menuName = "ScriptableObject/DataManager", order = 0)]
public class DataManager : ScriptableObject
{
    public static DataManager manager = null;
    public void InstantiateManager()
    {  
        if(DataManager.manager == null) DataManager.manager = this;
    }
    
    public int level = 0;
    [SerializeField] private List<WeaponsList> _weapons;
    public List<Weaponry> weapons_list
    {
        get
        {
            List<Weaponry> new_list = new List<Weaponry>();
            for(int i = 0; i <= level; i++)
            {
                if(i >= _weapons.Count) break;
                foreach(Weaponry w in _weapons[i].weapons) new_list.Add(w);
            }
            return new_list;
        }
    }

    [SerializeField] private List<ItemsList> _items;
    public List<Item> items_list
    {
        get
        {
            List<Item> new_list = new List<Item>();
            for(int i = 0; i <= level; i++)
            {
                if(i >= _weapons.Count) break;
                foreach(Item it in _items[i].items) new_list.Add(it);
            }
            return new_list;
        }
    }

    [System.NonSerialized] public int spawned_bounties = 0;
    [SerializeField] private List<BountyList> _bounties;
    public List<Bounty> bounties_list
    {
        get
        {
            List<Bounty> new_list = new List<Bounty>();
            for(int i = 0; i <= level; i++)
            {
                if(i >= _bounties.Count) break;
                foreach(Bounty bt in _bounties[i].bounties) new_list.Add(bt);
            }
            return new_list;
        }
    }

    public void resetBounties()
    {
        spawned_bounties = 0;
        for(int i = 0; i <= level; i++)
        {
            if(i >= _bounties.Count) break;
            foreach(Bounty bt in _bounties[i].bounties)
            {
                if(bt.status == BountyStatus.Spawned)
                    bt.status = BountyStatus.Ativa;
            }
        }       
    }

}

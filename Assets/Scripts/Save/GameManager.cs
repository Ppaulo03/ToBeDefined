using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ChangeScene
{
    [SerializeField] private string scene = "";
    [SerializeField] private Player_Stats stats = null;
    [SerializeField] private Inventory inventory = null;

    private void Start() 
    {
        if(scene != "") Save();
    }

    public void Save()
    {
        PlayerData data = new PlayerData(stats, inventory, scene);
        SaveSystem.Save(data);
    }

    public void Load()
    {
        PlayerData data = SaveSystem.Load();
        data = null;
        if(data != null)
        {
            stats.health = data.health;
            stats.energy = data.energy;

            inventory.money = data.money;

            ChooseLevel(data.scene);
        }
        else 
        {
            stats.health.current = stats.health.current;
            stats.energy.current = stats.energy.current;
            inventory.money = 0;
            ChooseLevel("Village1");
        }

    }
}

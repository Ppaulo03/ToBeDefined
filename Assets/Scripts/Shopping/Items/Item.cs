using UnityEngine;

[System.Serializable] public class Item
{
    public enum ItemType
    {
        None = 0,
        HealthPotion,
        ManaPotion
    }

    public Player_Stats stats = null;
    public string name = "";
    public string description = "";
    public int qtd = 0;
    public int price = 0;
    public int max_qtd = 24;
    public float value = 0;
    public Sprite image = null;
    public GameObject drop_obj = null;
    public ItemType type = ItemType.None;

    public Item(Item clone)
    {
        this.stats = clone.stats;
        this.name = clone.name;
        this.description = clone.description;
        this.qtd = clone.qtd;
        this.price = clone.price;
        this.max_qtd = clone.max_qtd;
        this.value = clone.value;
        this.image = clone.image;
        this.drop_obj = clone.drop_obj;
        this.type = clone.type;
    }

    public Item()
    {
        this.name = "";
    }

    public void Use()
    {
        this.qtd -= 1;
        switch(type)
        {
            case ItemType.HealthPotion:
                stats.health.current += this.value;
            break;

            case ItemType.ManaPotion:
                stats.energy.current += this.value;
            break;
        }
    }
    
}
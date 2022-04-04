using UnityEngine;

[System.Serializable] public class Weaponry
{
    public string name = "";
    public Weapon info = null;
    public int price = 0;
    public int max_durability = 0;
    public int durability = 0;
    public Sprite image = null;
    public GameObject obj;
    public GameObject drop_obj = null;

    public Weaponry(Weaponry clone)
    {
        this.name = clone.name;
        this.info = clone.info;
        this.price = clone.price;
        this.max_durability = clone.max_durability;
        this.durability = clone.durability;
        this.image = clone.image;
        this.obj = clone.obj;
        this.drop_obj = clone.drop_obj;
    }

    public Weaponry()
    {
        this.name = "";
    }


}

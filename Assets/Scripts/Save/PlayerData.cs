[System.Serializable] public class PlayerData
{
    public int level;

    public stat health;
    public stat energy;

    public int money;

    public string scene;

    public PlayerData(Player_Stats stats, Inventory inventory, string _scene)
    {
        scene = _scene;

        level = 0;

        health = stats.health;

        energy = stats.energy;

        money = inventory.money;
    }

}

using UnityEngine;

[System.Serializable] public struct stat
{
    public float max;
    public float recovery;
    private float _current;
    public float current
    {
        get{ return _current; }
        set
        { 
            if(value >= max) _current = max; 
            else if(value <= 0) _current = 0; 
            else _current = value;
        }
    }
}

[System.Serializable] public struct ability
{
    public float time;
    public float cost;
    public float speed;
    public GameObject effect;
}

[CreateAssetMenu(fileName = "Player_Stats", menuName = "ScriptableObject/Player_Stats", order = 0)]
public class Player_Stats : ScriptableObject
{
    public static Player_Stats stats = null;
    public ability dash;
    public stat health;
    public stat stamina;
    public stat energy;

    public float strength;
    public float magic;
    public float resistence;
    public float speed;
}
    



   

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magic", menuName = "ScriptableObject/Magic", order = 0)]
public class Magic : ScriptableObject, ISerializationCallbackReceiver
{
    public string spell_name = "", descricption = "";
    public float cost = 0f, cooldown = 0f,  strength = 0f, cast_time = 0f;
    public Sprite image = null;
    
    [SerializeField] private bool self_effect = false;
    [SerializeField] private GameObject magic = null;

    [System.NonSerialized] public float rest_time = 0f;
    [System.NonSerialized] public bool ready;
    
    public void OnBeforeSerialize(){}
    public void OnAfterDeserialize()
    {
        ready = true;
        rest_time = 0f;
    }

    public void Cast(Player_Stats stats, Vector2 position, Vector2 direction, ScriptableTimer timer)
    {
        if(ready && cost <= stats.energy.current)
        {
            stats.energy.current -= cost;
            timer.DoStartCoroutine(this.cooldownCo());
            if(self_effect)
            {
                Instantiate(magic, position ,  Quaternion.identity);
                stats.health.current += stats.magic + strength;
            }
            else
            {
                GameObject clone;
                Vector2 offset;
                Vector3 rotation;

                if(direction.y > 0)
                {
                    offset =  new Vector2(-0.20f, 0.8f); rotation = new Vector3(0,0,0);
                }
                else if(direction.y < 0)
                {
                    offset = new Vector2(-0.20f,-0.8f); rotation = new Vector3(0,0,-180); 
                }
                else if(direction.x > 0)
                {
                    offset =  new Vector2(0.8f,0.20f); rotation = new Vector3(0,0,-90); 
                }   
                else 
                {
                    offset = new Vector2(-0.8f,0.20f); rotation = new Vector3(0,0,90); 
                }
                
                clone = Instantiate(magic, position + offset, Quaternion.Euler (rotation));
                clone.GetComponent<MagicProjectile>().SetStatus(stats.magic +strength, 6f, direction);
            }
        }
    }
   
    private IEnumerator cooldownCo()
    {
        ready = false;
        float time = cooldown;
        rest_time = 1f;
        
        for(int i = 0; i<=100; i++)
        {
            yield return new WaitForSeconds(cooldown/100);
            time -= cooldown/100;
            rest_time = time/cooldown;
        }
    
        ready = true;
    }
    
}

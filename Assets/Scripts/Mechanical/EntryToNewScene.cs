using UnityEngine;

public class EntryToNewScene : ChangeScene
{
    [SerializeField] private string new_scene = "";
    [SerializeField] private string entry_name = "";
    [SerializeField] private GameObject player = null;
    [SerializeField] private Vector3 entry_offset = Vector2.zero;
    

    private static string destination = "";
    private static Vector2 entry_direction = Vector2.down;

    
    private void Start() 
    {
        if(destination == entry_name)
        {
            DataManager.manager.resetBounties();
            Animator player_anim = player.GetComponent<Animator>();
            player_anim.SetFloat("Direction X", entry_direction.x);
            player_anim.SetFloat("Direction Y", entry_direction.y);
            player.transform.position = transform.position + entry_offset;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(!other.isTrigger && other.gameObject.tag == "Player") 
        {
            destination = entry_name;
            entry_direction = player.GetComponent<Player>().getDirection();
            ChooseLevel(new_scene);
        }
    }
}

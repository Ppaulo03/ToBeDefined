using System.Collections;
using UnityEngine;

public class Droppable : MonoBehaviour
{
    [SerializeField] private Inventory inventory = null;
    public Item item = null;
    private float drop_life = 300;
    
    private void Start()
    {
        StartCoroutine(despawnCo());
        Transform value_text = transform.GetChild(0);
        if(item.qtd > 1)
        {
            value_text.gameObject.SetActive(true);
            value_text.GetComponent<TextMesh>().text = item.qtd.ToString();
        }else value_text.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player" && !other.isTrigger)
        {
            Item it = new Item(item);
            (bool got, int resto) = inventory.addItem(it);
            if(resto <= 0) Destroy(gameObject);
        } 
    }

    private IEnumerator despawnCo()
    {
       
        yield return new WaitForSeconds(drop_life);
        Destroy(gameObject);

    }

}

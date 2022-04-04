using UnityEngine;

public class SpriteText : MonoBehaviour
{
    void Start()
    {
 
        Renderer parentRenderer = transform.parent.GetComponent<Renderer>();
        Renderer renderer = GetComponent<Renderer>();

        renderer.sortingLayerID = parentRenderer.sortingLayerID;
        renderer.sortingOrder = parentRenderer.sortingOrder + 1;
    }
}

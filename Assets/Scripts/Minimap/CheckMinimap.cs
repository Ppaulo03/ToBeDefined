using UnityEngine;

public class CheckMinimap : MonoBehaviour
{

    private void Awake()
    {
       if(GameObject.FindGameObjectsWithTag("MiniMap").Length == 0)
       {
           gameObject.SetActive(false);
       }
   }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private GameObject fade_in_panel = null;
    [SerializeField] private GameObject fade_out_panel = null;

    private void Awake()
    {
        if(fade_in_panel != null) 
        {
            if(GameObject.FindGameObjectsWithTag("Transition").Length == 0)
            {
                GameObject clone = Instantiate(fade_in_panel, Vector3.zero, Quaternion.identity);
                Destroy(clone, 1);
            }
        }
    }
    public void ChooseLevel(string level) => StartCoroutine(ChooseLevelCo(level));

    private IEnumerator ChooseLevelCo(string level)
    {
        if(fade_out_panel != null)
        {
            Instantiate(fade_out_panel, Vector3.zero, Quaternion.identity);
            yield return new WaitForSeconds(0.33f);
        }
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(level);
        while(!asyncOperation.isDone) yield return null;
    }
    
}

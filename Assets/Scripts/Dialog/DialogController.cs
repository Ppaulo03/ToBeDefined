using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class DialogController : MonoBehaviour
{
    [System.NonSerialized] static public bool isEnabled = false;
    [SerializeField] private Text text = null;
    [SerializeField] private Image image = null;
    [SerializeField] private Sprite player = null;
    [SerializeField] private GameObject next = null;
    [SerializeField] private List<GameObject> choices = null;
    [SerializeField] private DialogValue dialog = null;
    private Story myStory = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            gameObject.SetActive(false);
           
    }

    private void OnEnable()
    {
        isEnabled = true;
        PauseMenu.isPaused = true;
        Time.timeScale = 0f;
        SetStory();
    } 

    private void OnDisable() 
    {
        isEnabled = false;
        PauseMenu.isPaused = false;
        Time.timeScale = 1f;
    }

    public void SetStory()
    {
        if(dialog.text_Asset)
        {
            myStory = new Story(dialog.text_Asset.text);
            RefreshView();
        }
        else Debug.Log("Error with story");
    }

    public void MakeChoice(int choiceValue)
    {
        myStory.ChooseChoiceIndex(choiceValue);
        image.sprite = player;
        RefreshView();
    }

    public void RefreshView()
    {
        for(int i = 0; i < choices.Count; i++) choices[i].SetActive(false);
        if(myStory.canContinue) 
        {
            text.text = myStory.Continue();
            List<string> tags = myStory.currentTags;
            if(tags.Count  > 0) image.sprite = dialog.image;
            
        }
        else gameObject.SetActive(false);

        if(myStory.currentChoices.Count > 0)
        {
            next.SetActive(false);
            for(int i = 0; i < myStory.currentChoices.Count; i++)
            {
                choices[i].SetActive(true);
                choices[i].transform.GetChild(0).GetComponent<Text>().text = myStory.currentChoices[i].text;
            }
        }
        else next.SetActive(true);
    }

}

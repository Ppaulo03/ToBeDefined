using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BookBase : MonoBehaviour
{
    [SerializeField] protected Inventory inventory = null;
    protected GameObject page, nxtBtn, bkBtn;
    protected Animator anim;
    public int index = 0;
    public int max_index;

    protected abstract void SetInfo();
    protected abstract void SetMaxIndex();

    private void Awake() 
    {
        SetMaxIndex();

        anim = GetComponent<Animator>();

        page = transform.GetChild(0).gameObject;
        Transform buttons = page.transform.GetChild(0);

        nxtBtn = buttons.GetChild(0).gameObject;
        bkBtn = buttons.GetChild(1).gameObject;
    }

    protected virtual void Start()
    {
        SetInfo();
    }

    private void Update() 
    {
        if(!PauseMenu.isPaused) StartCoroutine(closeCo());
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        PauseMenu.isPaused = true;
        StartCoroutine(changeCo());
    }
    
    public void Next()
    {
        index += 1;
        anim.SetTrigger("Next");
        if(index >= max_index)
        {
            index = max_index - 1;
            return;
        }
        bkBtn.SetActive(true);
        StartCoroutine(changeCo());
        SetInfo();
    }

    public void Back()
    {
        index -= 1;
        anim.SetTrigger("Back");
        if(index < 0)
        {
            index = 0;
            return;
        }
        nxtBtn.SetActive(true);
        StartCoroutine(changeCo());
        SetInfo();
    } 

    public void Close() => PauseMenu.isPaused = false;

    private IEnumerator closeCo()
    {
        page.SetActive(false);
        anim.SetTrigger("Exit");
        yield return new WaitForSecondsRT(0.5f);
        PauseMenu.isPaused = false;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private IEnumerator changeCo()
    {
        page.SetActive(false);   
        yield return new WaitForSecondsRT(0.5f);
        page.SetActive(true);
    }


}

using UnityEngine;
using UnityEngine.UI;

public class BountyPoster : MonoBehaviour
{
    private Bounty bounty;
    [SerializeField] private Color accept_color;
    [SerializeField] private Color waiting_color;

    public void SetBounty(Bounty _bounty)
    {
    
        if(_bounty == null) return;
        this.bounty = _bounty;
        
        gameObject.SetActive(true);
        
        transform.GetChild(0).GetComponent<Text>().text = bounty.title;
        transform.GetChild(1).GetComponent<Image>().sprite = bounty.image;
        transform.GetChild(2).GetComponent<Text>().text = bounty.descricao;
        Transform btn = transform.GetChild(3);

        if(this.bounty.status == BountyStatus.Inativa)
        {
            btn.GetComponent<Button>().enabled = true;
            btn.GetComponent<Image>().color = accept_color;
            btn.GetChild(0).GetComponent<Text>().text = "Accept";
        }else
        {
            btn.GetComponent<Button>().enabled = false;
            btn.GetComponent<Image>().color = waiting_color;
            btn.GetChild(0).GetComponent<Text>().text = "Waiting";
        }
    }

    public void Accept()
    {
        if(this.bounty != null || this.bounty.status == BountyStatus.Inativa)
        {
            Transform btn = transform.GetChild(3);
            btn.GetComponent<Button>().enabled = false;
            btn.GetComponent<Image>().color = waiting_color;
            btn.GetChild(0).GetComponent<Text>().text = "Waiting";
            
            bounty.status = BountyStatus.Ativa;
        }
    }

}
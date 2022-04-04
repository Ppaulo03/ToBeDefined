using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemShop : BookBase
{
    [SerializeField] private Transform slots = null;
    [SerializeField] private Transform Buying_Selling = null;
    private List<Item> item_list = null;
    private List<int> indexes = new List<int>();
    

    public float percentage = 1;
    public bool buying = true;
    
    protected override void SetMaxIndex() => max_index = 0;

    protected override void Start()
    {
        item_list = DataManager.manager.items_list;
        SetToBuy();
    } 
    
    
    protected override void SetInfo()
    {   
        List<Item> list;
        if(buying) list = item_list;
        else list = inventory.items;

        int i = index*6;
        foreach(Transform item in slots)
        {
            if(i >= indexes.Count) item.gameObject.SetActive(false);
            else
            {
                Item itm = list[indexes[i]];
                if(itm.name == "") 
                {      
                    item.GetChild(2).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Sold";
                    item.GetChild(2).GetChild(0).GetComponent<Button>().enabled = false;
                    item.GetChild(2).GetChild(1).GetComponent<Slider>().enabled = false;
                    item.GetChild(2).GetChild(2).GetComponent<InputField>().enabled = false;
                    
                }
                else
                {
                    item.gameObject.SetActive(true);
                
                    item.GetChild(0).gameObject.GetComponent<Image>().sprite = itm.image;

                    item.GetChild(1).gameObject.GetComponent<Text>().text = $"{itm.name}\n" + 
                                                                            itm.description;
                    
                    item.GetChild(2).GetChild(0).GetComponent<Button>().enabled = true;
                    item.GetChild(2).GetChild(1).GetComponent<Slider>().enabled = true;
                    item.GetChild(2).GetChild(2).GetComponent<InputField>().enabled = true;
                    
                    if(buying) 
                    {
                        item.GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(0f, 255f, 0f, 255f);
                        item.GetChild(2).GetComponent<ItemShopSetValue>().SetMax(24, itm, percentage);
                    }
                    else
                    {
                        item.GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(255f, 0f, 0f, 255f);
                        item.GetChild(2).GetComponent<ItemShopSetValue>().SetMax(itm.qtd, itm, percentage);
                    } 
                }
                
            }
            i++;
        } 
         
        if (index >= max_index - 1) nxtBtn.SetActive(false);
        else nxtBtn.SetActive(true);
        if (index <= 0) bkBtn.SetActive(false);
        else bkBtn.SetActive(true);
    }

    public void SetToSell()
    {
        indexes = new List<int>();
        for(int i = 0; i < inventory.items.Count; i++)
            if(inventory.items[i].name != "") indexes.Add(i);

        buying = false;

        max_index = (int) Mathf.Ceil(indexes.Count/6f);

        Buying_Selling.GetComponent<Image>().color = new Color(255f, 0f, 0f, 255f);
        Buying_Selling.GetChild(0).GetComponent<Text>().text = "Selling";

        percentage = 0.75f;
        index = 0;

        SetInfo();
    }

    public void SetToBuy()
    {   
        indexes = new List<int>();
        for(int i = 0; i < item_list.Count; i++)
            if(item_list[i].name != "") indexes.Add(i);

        buying = true;
        
        max_index = (int) Mathf.Ceil(indexes.Count/6f);

        Buying_Selling.GetComponent<Image>().color = new Color(0f, 255f, 0f, 255f);
        Buying_Selling.GetChild(0).GetComponent<Text>().text = "Buying";
        
        percentage = 1f;
        index = 0;

        SetInfo();
    }

    public void BuyOrSell(int n, int qtd)
    {
        if(buying) Buy( n, qtd);
        else Sell( n, qtd);
    }
    public void Buy(int n, int qtd)
    {
        Item item =  item_list[indexes[index*6 + n]];

        if (item.price * qtd <= inventory.money )
        {
            Item it = new Item(item);
            it.qtd = qtd;
            (bool sold, int resto) = inventory.addItem(it);
            if(sold) inventory.money -= it.price * qtd;
            else
            {
                int bougth = it.qtd - resto;
                inventory.money -= it.price * bougth;
            }
        }
    }

    public void Sell(int n, int qtd)
    {
        Item item =  inventory.items[indexes[index*6 + n]];
        Item it = new Item(item);
        inventory.money += (int) Mathf.Ceil(item.price * qtd * percentage);
        inventory.removeItem(it.name, qtd);
        SetInfo();
    }

}

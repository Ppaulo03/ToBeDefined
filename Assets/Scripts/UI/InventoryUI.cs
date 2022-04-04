using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventorySection
{
    Consumables = 0,
    Weapons = 1, 
    Armor = 2,
    Accessories = 3
}

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inGame_UI = null;
    [SerializeField] private List<GameObject> slots = null;
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private Transform info = null;

    [SerializeField] private GameObject drop_btm = null;
    [SerializeField] private GameObject equip_btm = null;
    [SerializeField] private GameObject use_btm = null;
    [SerializeField] private GameObject slots_btm = null;
    [SerializeField] private GameObject qtd_btm = null;

    [SerializeField] private Transform onUse = null;

    [SerializeField] private Material black_white = null;
    [SerializeField] private Sprite armor_default = null;
    [SerializeField] private Sprite accessory_default = null;
    [SerializeField] private Sprite weapom_default = null;
    [SerializeField] private Sprite consumable_default = null;

    [SerializeField] private List<Image> btn_sect = null;
    [SerializeField] private Color selected_btn_sect = new Color(255f, 98f, 13f, 186f);
    [SerializeField] private Color normal_btn_sect = new Color(3f, 1f, 24f, 186f);

    [SerializeField] private Material outline = null;

    private Transform player = null;

    public int selected = -1;
    private Image info_image;
    private Text info_text;
    
    public InventorySection section = InventorySection.Consumables;
    public InventorySection info_section = InventorySection.Consumables;

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        info_image = info.GetChild(0).GetComponent<Image>();
        info_text = info.GetChild(1).GetComponent<Text>();
    }

    private void Update() 
    {
        if(Input.GetKeyDown("i")) PauseMenu.isPaused = false;
        if(!PauseMenu.isPaused) 
        {
            inGame_UI.SetActive(true);
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        PauseMenu.isPaused = true;
        SetItems();
        SetOnUse();
    }
    
    public void SetSection(int newSection)
    {
        if((InventorySection) newSection != section)
        {
            section = (InventorySection) newSection;
            SetItems();
        }
    }

    public void SetItems()
    {
        int cont = 0;
        foreach(Image img in btn_sect) img.color = normal_btn_sect;
        switch(section)
        {
            case InventorySection.Consumables:

                btn_sect[0].color = selected_btn_sect;
                foreach (Item it in inventory.items)
                {
                    Image slot_img = slots[cont].transform.GetChild(0).GetComponent<Image>();
                    Text slot_text =  slots[cont].transform.GetChild(1).GetComponent<Text>();

                    slot_img.sprite = it.image;
                    if(it.qtd > 1)
                    {
                        slot_text.enabled = true;
                        slot_text.text = it.qtd.ToString();

                        slot_img.rectTransform.sizeDelta = new Vector2(56, 28);
                        slot_img.rectTransform.anchoredPosition = new Vector3(0,14,0);
                    }
                    else 
                    {
                        slot_text.enabled = false;

                        slot_img.rectTransform.sizeDelta = new Vector2(56, 56);
                        slot_img.rectTransform.anchoredPosition = Vector3.zero;
                    }
                    

                    if(it.image == null) 
                    {
                        slot_img.color = new Color(255f,255f,255f,0);
                        slot_text.enabled = false;
                        slots[cont].GetComponent<DragItem>().enabled = false;
                    }
                    else 
                    {
                        slot_img.color = new Color(255f,255f,255f,255f);
                        slots[cont].GetComponent<DragItem>().enabled = true;
                        slot_img.material = null;
                        foreach(int index in inventory.item_index)
                        {
                            if(index == cont)
                            {
                                slot_img.material = outline;
                                break;
                            }
                        }
        
                    }
                
                    cont ++;
                }
            break;

            case InventorySection.Weapons:

                btn_sect[1].color = selected_btn_sect;
            
                foreach (Weaponry wp in inventory.weapons)
                {
                    Image slot_img = slots[cont].transform.GetChild(0).GetComponent<Image>();
                    Text slot_text =  slots[cont].transform.GetChild(1).GetComponent<Text>();

                    slot_img.sprite = wp.image;
                    slot_img.rectTransform.sizeDelta = new Vector2(56, 56);
                    slot_img.rectTransform.anchoredPosition = Vector3.zero;

                    slot_text.enabled = false;
                    

                    if(wp.image == null) 
                    {
                        slot_img.color = new Color(255f,255f,255f,0);
                        slots[cont].GetComponent<DragItem>().enabled = false;
                    }
                    else 
                    {
                        slot_img.color = new Color(255f,255f,255f,255f);
                        slots[cont].GetComponent<DragItem>().enabled = true;

                        slot_img.material = null;
                        foreach(int index in inventory.weapon_index)
                        {
                            if(index == cont)
                            {
                                slot_img.material = outline;
                                break;
                            }
                        }
                    }
                    cont ++;
                }
            break;

            case InventorySection.Armor:
                btn_sect[2].color = selected_btn_sect;
            break;

            case InventorySection.Accessories:
                btn_sect[3].color = selected_btn_sect;
            break;
        }
        
    }

    public void SetInfo(int slot_num) => SetInfo(slot_num, section);
    
    public void SetInfo(int slot_num, InventorySection _section)
    {
        info_section = _section;
        selected = slot_num;

        slots_btm.SetActive(false);
        qtd_btm.SetActive(false);
        
        switch(_section)
        {
            case InventorySection.Consumables:
                if(inventory.items[selected].image != null)
                {
                    info_image.color = new Color(255f,255f,255f,255f);
                    info_image.sprite = inventory.items[selected].image;
                    info_text.enabled = true;
                    info_text.text = $"{inventory.items[selected].name}\n"+
                                     $"{inventory.items[selected].description}\n\n"+
                                     $"Quantity: {inventory.items[selected].qtd}";

                    drop_btm.SetActive(true);
                    equip_btm.SetActive(true);
                    use_btm.SetActive(true);
                }
                else
                {
                    info_image.color = new Color(255f,255f,255f,0);
                    info_text.enabled = false;
                    drop_btm.SetActive(false);
                    equip_btm.SetActive(false);
                    use_btm.SetActive(false);
                }
            break;

            case InventorySection.Weapons:
                if(inventory.weapons[selected].image != null)
                {
                    info_image.color = new Color(255f,255f,255f,255f);
                    info_image.sprite = inventory.weapons[selected].image;
                    info_text.enabled = true;
                    Weapon wp_info = inventory.weapons[selected].obj.GetComponent<Weapon>();
                    info_text.text = $"Name: {inventory.weapons[selected].name}\n" +
                                     $"Damage: {wp_info.damage_Base}\n" +
                                     $"Attack Time: {wp_info.time} s\n"+
                                     $"Durability: {inventory.weapons[selected].durability}";

                    drop_btm.SetActive(true);
                    equip_btm.SetActive(true);
                    use_btm.SetActive(false);
                }
                else
                {
                    info_image.color = new Color(255f,255f,255f,0);
                    info_text.enabled = false;

                    drop_btm.SetActive(false);
                    equip_btm.SetActive(false);
                    use_btm.SetActive(false);
                }
            break;
        }
    }
    
    public void SetOnUse()
    {
        Image tmp;

        tmp = onUse.GetChild(0).GetChild(0).GetComponent<Image>();
        tmp.sprite = armor_default;
        tmp.material = black_white;
        
        for(int i = 0; i<2; i++)
        {
            tmp =  onUse.GetChild(i + 1).GetChild(0).GetComponent<Image>();
            tmp.sprite = accessory_default;
            tmp.material = black_white;

        }
        for(int i = 0; i<3; i++)
        {
            if(inventory.weapon_index[i] == -1 || inventory.weapons[inventory.weapon_index[i]].image == null)
            {
                tmp =  onUse.GetChild(i + 3).GetChild(0).GetComponent<Image>();
                tmp.sprite = weapom_default;
                tmp.material = black_white;
            }else
            {
                onUse.GetChild(i + 3).GetChild(0).gameObject.SetActive(true);
                tmp = onUse.GetChild(i + 3).GetChild(0).GetComponent<Image>();
                tmp.sprite = inventory.weapons[inventory.weapon_index[i]].image;
                tmp.material = null;
            }
        }

        for(int i = 0; i<3; i++)
        {
            if(inventory.item_index[i] == -1 || inventory.items[inventory.item_index[i]].image == null)
            {
                tmp = onUse.GetChild(i + 6).GetChild(0).GetComponent<Image>();
                tmp.sprite = consumable_default;
                tmp.material = black_white;
            }else
            {
                onUse.GetChild(i + 6).GetChild(0).gameObject.SetActive(true);
                tmp = onUse.GetChild(i + 6).GetChild(0).GetComponent<Image>();
                tmp.sprite = inventory.items[inventory.item_index[i]].image;
                tmp.material = null;
            }
        }
    }
    
    public void Equip(int i = -1)
    {
        switch(info_section)
        {
            case InventorySection.Consumables:
                if(i == -1)
                {
                    slots_btm.SetActive(true);
                    drop_btm.SetActive(false);
                    equip_btm.SetActive(false);
                    use_btm.SetActive(false);
                }
                else
                {
                    for(int j=0; j<3; j++)
                    {
                        if(inventory.item_index[j] == selected) 
                            inventory.item_index[j] = inventory.item_index[i];
                    }
                    inventory.item_index[i] = selected;
                    slots_btm.SetActive(false);
                    drop_btm.SetActive(true);
                    equip_btm.SetActive(true);
                    use_btm.SetActive(true);
                } 
            break;

            case InventorySection.Weapons:
                if(i == -1)
                {
                    slots_btm.SetActive(true);
                    drop_btm.SetActive(false);
                    equip_btm.SetActive(false);
                    use_btm.SetActive(false);
                }
                else
                {
                    for(int j=0; j<3; j++)
                    {
                        if(inventory.weapon_index[j] == selected) 
                            inventory.weapon_index[j] = inventory.weapon_index[i];
                    }
                    inventory.weapon_index[i] = selected;
                    slots_btm.SetActive(false);
                    drop_btm.SetActive(true);
                    equip_btm.SetActive(true);
                    use_btm.SetActive(true);
                } 
            break;

            case InventorySection.Armor:
            break;

            case InventorySection.Accessories:
            break;
        }
        SetOnUse();
        SetItems();
    }

    public void Drop(int i = -1)
    {
        float x = Random.Range(0.5f,1f)*(Random.Range(0,2)*2-1), y = Random.Range(0.5f,1f)*(Random.Range(0,2)*2-1);
        Vector3 offset = new Vector3(x,y, 0); 
        switch(info_section)
        {
            case InventorySection.Consumables:
                if(i == -1)
                {
                    if(inventory.items[selected].qtd == 1) Drop(1);
                    else
                    {
                        drop_btm.SetActive(false);
                        equip_btm.SetActive(false);
                        use_btm.SetActive(false);
                        qtd_btm.SetActive(true);
                    }
                }
                else if(i > 0)
                {
                    GameObject clone = Instantiate(inventory.items[selected].drop_obj, player.position + offset, Quaternion.identity);
                    clone.GetComponent<Droppable>().item.qtd = i;

                    inventory.items[selected].qtd -= i;
                    if(inventory.items[selected].qtd <= 0) inventory.items[selected] = new Item();

                    SetInfo(selected);
                }             
            break;

            case InventorySection.Weapons:
                Instantiate(inventory.weapons[selected].drop_obj, player.position + offset, Quaternion.identity);
                inventory.weapons[selected] = new Weaponry();
                SetInfo(selected);
            break;

            case InventorySection.Armor:
            break;

            case InventorySection.Accessories:
            break;
        }
        SetItems();
        SetOnUse();
    }

    public void Use()
    {
        if(info_section == InventorySection.Consumables)
        {
            inventory.items[selected].Use();
            if(inventory.items[selected].qtd <= 0) 
            {
                inventory.items[selected] = new Item();
                for(int i=0; i < 3; i++) if(inventory.item_index[i] == selected) inventory.item_index[i] = -1;
            }
            SetItems();
            SetOnUse();
        }
    }


}

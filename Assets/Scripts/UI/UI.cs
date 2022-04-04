using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Slider health_bar = null;
    [SerializeField] private Slider mana_bar = null;
    [SerializeField] private Slider stamina_bar = null;
    [SerializeField] private Player_Stats stats = null;
    [SerializeField] private Text money = null;
    [SerializeField] private Transform item = null;
    private Image item_img; private Text item_qtd;
    [SerializeField] private Transform weapon = null;
    private Image weapon_img;
    [SerializeField] private Inventory inventory = null;

    [SerializeField] private Material black_white = null;
    [SerializeField] private Sprite weapom_default = null;
    [SerializeField] private Sprite consumable_default = null;

    private void Start() 
    {
            item_img = item.GetChild(0).GetComponent<Image>();
            item_qtd = item.GetChild(1).GetComponent<Text>();

            weapon_img = weapon.GetChild(0).GetComponent<Image>();
    }

    private void Update() 
    {
            health_bar.value = stats.health.current/stats.health.max;
            mana_bar.value = stats.energy.current/stats.energy.max;
            stamina_bar.value = stats.stamina.current/stats.stamina.max;

            money.text = inventory.money.ToString();
            
            if(inventory.item_obj.image == null)
            {
                item_img.sprite = consumable_default;
                item_img.material = black_white;
                
                item_img.rectTransform.sizeDelta = new Vector2(58, 58);
                item_img.rectTransform.anchoredPosition = Vector3.zero;

                item_qtd.text = "";
            }
            else
            {
                item_img.sprite = inventory.item_obj.image;
                item_img.material = null;
                if(inventory.item_obj.qtd > 1)
                {
                    item_qtd.text = inventory.item_obj.qtd.ToString();
                    item_img.rectTransform.sizeDelta = new Vector2(58, 29);
                    item_img.rectTransform.anchoredPosition = new Vector3(0, 14.5f);
                }
                else
                {
                    item_qtd.text = "";
                    item_img.rectTransform.sizeDelta = new Vector2(58, 58);
                    item_img.rectTransform.anchoredPosition = Vector3.zero;
                }
            }
            
            if(inventory.weapon.image == null)
            {
                weapon_img.sprite = weapom_default;
                weapon_img.material = black_white;
            }
            else
            {
                weapon_img.sprite = inventory.weapon.image;
                weapon_img.material = null;
            }
            


    }

    
}

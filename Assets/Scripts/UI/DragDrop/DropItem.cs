using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DropItem : MonoBehaviour, IDropHandler
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private Inventory inventory;
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            int drop_slot = System.Int32.Parse(gameObject.name), drag_slot =  System.Int32.Parse(eventData.pointerDrag.name);
            switch(inventoryUI.section)
            {
                case InventorySection.Consumables:
                    if(inventory.items[drop_slot].name == inventory.items[drag_slot].name)
                    {
                        inventory.items[drag_slot].qtd += inventory.items[drop_slot].qtd;
                        if(inventory.items[drag_slot].qtd > inventory.items[drag_slot].max_qtd)
                        {
                            inventory.items[drop_slot].qtd = inventory.items[drag_slot].qtd - inventory.items[drag_slot].max_qtd;
                            inventory.items[drag_slot].qtd = inventory.items[drag_slot].max_qtd;
                        }
                        else inventory.items[drop_slot] = new Item();
                    }

                    Item item_tmp = inventory.items[drop_slot];
                    inventory.items[drop_slot] = inventory.items[drag_slot];
                    inventory.items[drag_slot] = item_tmp;

                    for(int i = 0; i < 3; i ++)
                    {
                        if(inventory.item_index[i] == drop_slot) inventory.item_index[i] = drag_slot;
                        else if(inventory.item_index[i] == drag_slot) inventory.item_index[i] = drop_slot;
                    }
                break;

                case InventorySection.Weapons:
                    Weaponry wp_tmp = inventory.weapons[drop_slot];
                    inventory.weapons[drop_slot] = inventory.weapons[drag_slot];
                    inventory.weapons[drag_slot] = wp_tmp;

                    for(int i = 0; i < 3; i ++)
                    {
                        if(inventory.weapon_index[i] == drop_slot) inventory.weapon_index[i] = drag_slot;
                        else if(inventory.weapon_index[i] == drag_slot) inventory.weapon_index[i] = drop_slot;
                    }
                    
                break;

                case InventorySection.Armor:
                break;

                case InventorySection.Accessories:
                break;
            }

            inventoryUI.SetItems();
            eventData.pointerDrag.GetComponent<DragItem>().endDrag();
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DropEquip : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventorySection section;
    [SerializeField] private int slot;


    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            int drag_slot =  System.Int32.Parse(eventData.pointerDrag.name);
            if(inventoryUI.section == section)
            {
                switch(section)
                {
                    case InventorySection.Consumables:
                        for(int j=0; j<3; j++)
                        {
                            if(inventory.item_index[j] == drag_slot) 
                                inventory.item_index[j] = inventory.item_index[slot];
                        }
                        inventory.item_index[slot] = drag_slot;
                    break;
                    case InventorySection.Weapons:
                        for(int j=0; j<3; j++)
                        {
                            if(inventory.weapon_index[j] == drag_slot) 
                                inventory.weapon_index[j] = inventory.weapon_index[slot];
                        }
                        inventory.weapon_index[slot] = drag_slot;
                    break;
                    case InventorySection.Armor:
                    break;
                    case InventorySection.Accessories:
                    break;
                }
                inventoryUI.SetOnUse();
                inventoryUI.SetItems();
            }

            
            eventData.pointerDrag.GetComponent<DragItem>().endDrag();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        switch(section)
        {
            case InventorySection.Consumables:
                if(inventory.item_index[slot] != -1)
                    if(inventory.items[inventory.item_index[slot]].name != "")
                        inventoryUI.SetInfo(inventory.item_index[slot], section);
            break;
            case InventorySection.Weapons:
                if(inventory.weapon_index[slot] != -1)
                    if(inventory.weapons[inventory.weapon_index[slot]].name != "")
                        inventoryUI.SetInfo(inventory.weapon_index[slot], section);
            break;
            case InventorySection.Armor:
            break;
            case InventorySection.Accessories:
            break;
        }   
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DragItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private InventoryUI inventoryUI;

    private RectTransform rectTransform;
    private Vector2 originalPos;
    private CanvasGroup canvasGroup;
    private int slot_num;
    

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        slot_num =  System.Int32.Parse(gameObject.name);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        originalPos = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
        => rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    
    
    public void OnEndDrag(PointerEventData eventData)
    {
        endDrag();
    }
    
    public void endDrag()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        rectTransform.anchoredPosition = originalPos;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        inventoryUI.SetInfo(slot_num);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvSlotElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int slotNumber;
    public InvItemElement itemElement;

    private static DraggedItem draggedItem;
    private static GameObject hoverElement;
    private static Image hoverElementImage;

    private void Start()
    {
        draggedItem = new DraggedItem();
        
        hoverElement = new GameObject();
        hoverElement.AddComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        hoverElement.transform.SetParent(transform.parent);

        hoverElementImage = hoverElement.AddComponent<Image>();
        hoverElementImage.raycastTarget = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemElement == null) return;

        UIController.Instance.GetInventoryMenu().SetSelectedItem(itemElement);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        draggedItem.oldSlot = this.gameObject;

        if (itemElement != null) draggedItem.oldItem = itemElement;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        draggedItem.oldSlot = null;
        draggedItem.oldItem = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemElement == null) return;

        hoverElement.SetActive(true);

        if (itemElement != null)
        {
            hoverElementImage.sprite = itemElement.item.GetIcon();
            itemElement.gameObject.SetActive(false);
        }

        draggedItem.newSlot = hoverElement;
        draggedItem.newItem = itemElement;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemElement == null) return;
        hoverElement.SetActive(false);

        if (draggedItem.oldSlot != null) GameController.Instance.GetInventory().MoveItem(this.slotNumber, draggedItem.oldSlot.GetComponent<InvSlotElement>().slotNumber);
        if (itemElement != null ) itemElement.gameObject.SetActive(true);

        draggedItem.newItem = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemElement == null) return;
        if (draggedItem.newSlot != null) draggedItem.newSlot.GetComponent<RectTransform>().position = Input.mousePosition;
    }
}

public class DraggedItem
{
    public GameObject newSlot;
    public GameObject oldSlot;

    public InvItemElement newItem;
    public InvItemElement oldItem;
}
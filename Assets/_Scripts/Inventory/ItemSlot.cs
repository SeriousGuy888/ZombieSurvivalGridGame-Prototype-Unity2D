using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot :
    MonoBehaviour,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler {
  [SerializeField] private Image icon;
  [SerializeField] private TextMeshProUGUI label;
  [SerializeField] private TextMeshProUGUI amountLabel;
  [SerializeField] private GameObject stackObj;
  [SerializeField] private Image selectedIndicator;

  public int slotIndex;

  public void Set(InventoryItem item) {
    if(item == null) {
      icon.color = Color.clear;
      label.text = "";
      amountLabel.text = "";
      stackObj.SetActive(false);
      return;
    }

    icon.color = Color.white; // make icon visible
    icon.sprite = item.data.icon; // set icon to item icon
    label.text = item.data.displayName; // set label to item name
    amountLabel.text = item.stackSize.ToString(); // set amount label to item stack size
    stackObj.SetActive(item.stackSize > 1); // only show stack object if item stack size is greater than 1
    
    // display selected indicator if item slot is selected
    selectedIndicator.gameObject.SetActive(InventorySystem.Instance.selectedIndex == slotIndex);
  }

  public void OnPointerClick(PointerEventData eventData) {
    if(eventData.button == PointerEventData.InputButton.Left) {
      InventorySystem.Instance.SelectSlot(InventorySystem.Instance.selectedIndex == slotIndex
        ? -1
        : slotIndex
      );
    }
  } 

  public void OnDrag(PointerEventData eventData) {
    print("I'm being dragged!");
  }

  public void OnBeginDrag(PointerEventData eventData) {
    throw new System.NotImplementedException();
  }

  public void OnEndDrag(PointerEventData eventData) {
    throw new System.NotImplementedException();
  }
}

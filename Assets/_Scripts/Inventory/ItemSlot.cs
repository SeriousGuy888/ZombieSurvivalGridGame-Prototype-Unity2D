using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour {
  [SerializeField] private Image icon;
  [SerializeField] private TextMeshProUGUI label;
  [SerializeField] private TextMeshProUGUI amountLabel;
  [SerializeField] private GameObject stackObj;

  public int slotIndex;

  public void Set(InventoryItem item) {
    icon.sprite = item.data.icon; // set icon to item icon
    label.text = item.data.displayName; // set label to item name
    amountLabel.text = item.stackSize.ToString(); // set amount label to item stack size
    stackObj.SetActive(item.stackSize > 1); // only show stack object if item stack size is greater than 1
  }
}

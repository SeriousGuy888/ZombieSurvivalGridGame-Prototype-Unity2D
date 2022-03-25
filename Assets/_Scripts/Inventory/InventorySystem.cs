using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour {
  public static InventorySystem Instance;
  private Dictionary<InventoryItemData, InventoryItem> itemDictionary;
  public List<InventoryItem> inventory { get; private set; }
  public int maxInventorySize = 8;

  public delegate void InventoryUpdate();
  public event InventoryUpdate OnInventoryChangedEvent;

  private void Awake() {
    Instance = this;
    itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    inventory = new List<InventoryItem>();
  }

  public InventoryItem Get(InventoryItemData itemData) {
    if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
      return item;
    return null;
  }

  // Adds an item to the inventory.
  // Returns true if the item was added, and false if the item was not added.
  public bool Add(InventoryItemData itemData) {
    if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
      item.AddToStack();
    else {
      if(inventory.Count >= maxInventorySize)
        return false;

      InventoryItem newItem = new InventoryItem(itemData);
      itemDictionary.Add(itemData, newItem);
      inventory.Add(newItem);
    }

    if(OnInventoryChangedEvent != null)
      OnInventoryChangedEvent();
    return true;
  }

  public void Remove(InventoryItemData itemData) {
    if(itemDictionary.TryGetValue(itemData, out InventoryItem item)) {
      item.RemoveFromStack();
      if(item.stackSize <= 0) {
        itemDictionary.Remove(itemData);
        inventory.Remove(item);
      }
    }

    if(OnInventoryChangedEvent != null)
      OnInventoryChangedEvent();
  }
}

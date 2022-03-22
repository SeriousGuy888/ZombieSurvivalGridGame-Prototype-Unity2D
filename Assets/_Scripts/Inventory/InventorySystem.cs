using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour {
  public static InventorySystem Instance;
  private Dictionary<InventoryItemData, InventoryItem> _itemDictionary;
  public List<InventoryItem> inventory { get; private set; }

  public delegate void InventoryUpdate();
  public event InventoryUpdate OnInventoryChangedEvent;

  private void Awake() {
    Instance = this;
    _itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    inventory = new List<InventoryItem>();
  }

  public InventoryItem Get(InventoryItemData itemData) {
    if(_itemDictionary.TryGetValue(itemData, out InventoryItem item))
      return item;
    return null;
  }

  public void Add(InventoryItemData itemData) {
    if(_itemDictionary.TryGetValue(itemData, out InventoryItem item)) {
      item.AddToStack();
    } else {
      InventoryItem newItem = new InventoryItem(itemData);
      _itemDictionary.Add(itemData, newItem);
      inventory.Add(newItem);
    }

    if(OnInventoryChangedEvent != null)
      OnInventoryChangedEvent();
  }

  public void Remove(InventoryItemData itemData) {
    if(_itemDictionary.TryGetValue(itemData, out InventoryItem item)) {
      item.RemoveFromStack();
      if(item.stackSize <= 0) {
        _itemDictionary.Remove(itemData);
        inventory.Remove(item);
      }
    }

    if(OnInventoryChangedEvent != null)
      OnInventoryChangedEvent();
  }
}

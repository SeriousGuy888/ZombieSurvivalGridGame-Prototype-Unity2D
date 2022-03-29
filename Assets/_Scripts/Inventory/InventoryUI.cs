using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
  public static InventoryUI Instance;
  private void Awake() {
    Instance = this;
  }

  [SerializeField] private GameObject _slotPrefab;
  private List<ItemSlot> itemSlots;


  private void Start() {
    InventorySystem.Instance.OnInventoryChangedEvent += OnUpdateInventory;

    itemSlots = new List<ItemSlot>();
    for(int i = 0; i < InventorySystem.Instance.maxInventorySize; i++) {
      GameObject obj = Instantiate(_slotPrefab);
      obj.transform.SetParent(transform);
      obj.name = "Inventory Slot " + i;

      var slot = obj.GetComponent<ItemSlot>();
      slot.slotIndex = i;
      slot.Set(null);

      itemSlots.Add(slot);
    }
  }

  private void OnUpdateInventory() {
    foreach(var slot in itemSlots) {
      if(slot.slotIndex >= InventorySystem.Instance.inventory.Count) {
        slot.Set(null);
        continue;
      }
      
      InventoryItem item = InventorySystem.Instance.inventory[slot.slotIndex];
      slot.Set(item);
    }
  }
}

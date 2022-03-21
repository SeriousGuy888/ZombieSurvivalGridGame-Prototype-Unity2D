using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : BaseEntity {
  public InventoryItemData itemData;

  public void PickUp() {
    GameManager.Instance.player.inventorySystem.Add(itemData);
    Destroy(gameObject);
  }
}

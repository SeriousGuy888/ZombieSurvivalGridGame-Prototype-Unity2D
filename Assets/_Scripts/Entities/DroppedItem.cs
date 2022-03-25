using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : BaseEntity {
  public InventoryItemData itemData;

  public void PickUp() {
    if(InventorySystem.Instance.Add(itemData)) {
      Destroy(gameObject);
    }
  }
}

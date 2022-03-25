using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemsManager : MonoBehaviour {
  public static DroppedItemsManager Instance;
  private void Awake() {
    Instance = this;
  }

  public ItemRegistry itemRegistry;
  [SerializeField] private DroppedItem droppedItemPrefab;

  public void SpawnDroppedItem(Vector2 position, InventoryItemData itemData) {
    DroppedItem droppedItem = Instantiate(droppedItemPrefab, position, Quaternion.identity);
    droppedItem.itemData = itemData;
    droppedItem.name = itemData.displayName;
    droppedItem.GetComponent<SpriteRenderer>().sprite = itemData.icon;
  }


  [Serializable]
  public class ItemRegistry {
    public InventoryItemData apple;
  }
}
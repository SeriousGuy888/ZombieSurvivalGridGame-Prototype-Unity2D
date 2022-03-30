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

  public DroppedItem SpawnItem(Vector2 position, InventoryItemData itemData) {
    DroppedItem droppedItem = Instantiate(droppedItemPrefab, position, Quaternion.identity);
    droppedItem.itemData = itemData;
    droppedItem.name = itemData.displayName;
    droppedItem.GetComponent<SpriteRenderer>().sprite = itemData.icon;
    return droppedItem;
  }

  public DroppedItem SpawnItemWithRandVel(Vector2 position, InventoryItemData itemData) {
    Vector2 randVel = Random.insideUnitCircle;
    var droppedItem = SpawnItem(position + new Vector2(0.5f, 0.5f), itemData);
    droppedItem.SetVelocity(randVel);
    return droppedItem;
  }


  [System.Serializable]
  public class ItemRegistry {
    public InventoryItemData apple, log;
  }
}
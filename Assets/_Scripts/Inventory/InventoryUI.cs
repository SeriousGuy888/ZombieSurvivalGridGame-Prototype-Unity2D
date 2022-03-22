using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
  public static InventoryUI Instance;
  private void Awake() {
    Instance = this;
  }

  [SerializeField] private GameObject _slotPrefab;


  private void Start() {
    InventorySystem.Instance.OnInventoryChangedEvent += OnUpdateInventory;
  }

  private void OnUpdateInventory() {
    foreach(Transform t in transform) {
      Destroy(t.gameObject);
    }
    
    DrawInventory();
  }

  public void DrawInventory() {
    foreach(InventoryItem item in InventorySystem.Instance.inventory) {
      AddInventorySlot(item);
    }
  }

  public void AddInventorySlot(InventoryItem item) {
    GameObject obj = Instantiate(_slotPrefab);
    obj.transform.SetParent(transform);

    ItemSlot slot = obj.GetComponent<ItemSlot>();
    slot.Set(item);
  }
}

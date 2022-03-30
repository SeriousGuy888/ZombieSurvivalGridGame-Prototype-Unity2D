using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : BaseEntity {
  public InventoryItemData itemData;

  public void PickUp() {
    if(InventorySystem.Instance.Add(itemData))
      StartCoroutine(ExecutePickup());
  }

  private IEnumerator ExecutePickup() {
    Vector2 playerPos = GameManager.Instance.player.transform.position;
    Vector2 dir = (playerPos - (Vector2) transform.position).normalized;

    float dist = -1f;
    while(dist < 0 || dist > 0.1f) {
      dist = Vector2.Distance(playerPos, transform.position);
      if(dist > 2f)
        yield return null;

      transform.position += (Vector3) dir * 0.1f;
      yield return new WaitForSeconds(0.01f);
    }
    Destroy(gameObject);
  }
}

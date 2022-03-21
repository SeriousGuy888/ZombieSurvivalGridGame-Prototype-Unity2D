using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapClickReceiver : MonoBehaviour {
  [SerializeField] private BoxCollider2D boxCollider;

  private void Start() {
    int mapSize = MapManager.Instance.mapSize;
    boxCollider.size = new Vector2(mapSize, mapSize);
    boxCollider.offset = new Vector2(mapSize / 2, mapSize / 2);
  }

  // tests if the player is hovering over the tilemap but not an entity
  private void Update() {
    RaycastHit2D hit = Physics2D.GetRayIntersection(
      Camera.main.ScreenPointToRay(Input.mousePosition),
      distance: Mathf.Infinity,
      layerMask: LayerMask.GetMask(new string[] { "UI", "Entity", "Player" }));

    TileSelection.Instance.isHovering = (
      hit.collider != null &&
      hit.collider.gameObject.layer == LayerMask.NameToLayer("UI"));
  }

  private void OnMouseDown() => TileSelection.Instance.Click();
}

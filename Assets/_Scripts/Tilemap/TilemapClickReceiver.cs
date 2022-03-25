using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TilemapClickReceiver : MonoBehaviour {
  [SerializeField] private BoxCollider2D boxCollider;

  private void Start() {
    int mapSize = MapManager.Instance.mapSize;
    boxCollider.size = new Vector2(mapSize, mapSize);
    boxCollider.offset = new Vector2(mapSize / 2, mapSize / 2);
  }

  // tests if the player is hovering over the tilemap but not an entity
  private void Update() {
    // get object that the mouse is over
    RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

    // set the tilemap's isHovering bool, based on if the mouse is not over the ui
    TileSelection.Instance.isHovering =
      hit.collider != null &&
      !EventSystem.current.IsPointerOverGameObject();
  }

  private void OnMouseDown() => TileSelection.Instance.Click();
}

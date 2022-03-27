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
    RaycastHit2D hit = Physics2D.GetRayIntersection(
      ray: Camera.main.ScreenPointToRay(Input.mousePosition),
      distance: Mathf.Infinity,
      layerMask: LayerMask.GetMask(new string[] { "HoverOverlay", "Entity", "Player" })
    );

    // set the tilemap's isHovering bool, based on if the mouse is not over the ui
    TileSelection.Instance.isHovering =
      hit.collider != null &&
      hit.collider.gameObject.layer == LayerMask.NameToLayer("HoverOverlay") &&
      !EventSystem.current.IsPointerOverGameObject();
    
    if(Input.GetMouseButtonDown(0))
      TileSelection.Instance.Click(PointerEventData.InputButton.Left);
    if(Input.GetMouseButtonDown(1))
      TileSelection.Instance.Click(PointerEventData.InputButton.Right);
  }
}

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

  private void OnMouseEnter() => TileSelection.Instance.SetHovered(true);
  private void OnMouseExit() => TileSelection.Instance.SetHovered(false);
  private void OnMouseDown() => TileSelection.Instance.Click();
}

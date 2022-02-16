using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelection : MonoBehaviour {
  public static TileSelection Instance;

  [SerializeField] private Camera cam;
  [SerializeField] private Tilemap selectionOverlayTilemap;
  [SerializeField] private TileBase hoverTileAsset;
  private Vector3 prevMousePos;
  private Vector2Int hoveredTilePos;

  private void Awake() {
    Instance = this;
    cam = Camera.main;
  }

  private void Update() {
    // if(Input.mousePosition == prevMousePos)
    //   return;

    if(hoveredTilePos != null)
      SetOverlayTile(hoveredTilePos, null);
    
    prevMousePos = Input.mousePosition;
    hoveredTilePos = GetHoveredCellPos();
    SetOverlayTile(hoveredTilePos, hoverTileAsset);
  }



  // public void Click() {
  //   GameTile clickedTile = MapManager.Instance.GetTile(GetHoveredCellPos());
  // }

  // private void SelectTile(GameTile tile) {
  //   selectedTile = tile;
  // }

  // private void DeselectTile() {
  //   selectedTile = null;
  // }


  private void SetOverlayTile(Vector2Int position, TileBase overlayTileType) {
    if(MapManager.Instance.terrainTilemap.HasTile((Vector3Int) position))
      selectionOverlayTilemap.SetTile((Vector3Int) position, overlayTileType);
  }

  private Vector2Int GetHoveredCellPos() {
    Vector3 currentHoveredWorldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
    Vector2Int tilemapPos = (Vector2Int) MapManager.Instance.terrainTilemap.WorldToCell(currentHoveredWorldPoint);
    return tilemapPos;
  }
}

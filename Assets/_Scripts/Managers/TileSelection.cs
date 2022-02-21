using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelection : MonoBehaviour {
  public static TileSelection Instance;

  public bool isHovering; // is the cursor directly over the tilemap's boxcollider right now
  [SerializeField] private Camera cam;
  [SerializeField] private Tilemap selectionOverlayTilemap;
  [SerializeField] private TileBase hoverTileAsset;
  private Vector3 prevMousePos;
  private Vector2Int hoveredTilePos;

  [SerializeField] private Zombie zombie;

  private void Awake() {
    Instance = this;
    cam = Camera.main;
  }

  private void Update() {
    // if(Input.mousePosition == prevMousePos)
    //   return;

    if(hoveredTilePos != null)
      SetOverlayTile(hoveredTilePos, null);
    if(!isHovering)
      return;

    prevMousePos = Input.mousePosition;
    hoveredTilePos = GetHoveredCellPos();
    SetOverlayTile(hoveredTilePos, hoverTileAsset);
  }



  public void Click() {
    if(!isHovering)
      return;
    GameTile clickedTile = MapManager.Instance.GetTile(GetHoveredCellPos());

    if(clickedTile.structure.type == StructureType.None) {
      MapManager.Instance.SetStructureAt(clickedTile.coords, StructureType.Crate);
    } else {
      clickedTile.structure.ApplyDamage(10);
    }


    // Instantiate(zombie,
    //   MapManager.Instance.terrainTilemap.CellToWorld((Vector3Int) clickedTile.coords),
    //   Quaternion.identity);
  }


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

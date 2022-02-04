using UnityEngine;

public class GrassTile : BaseTile {
  [SerializeField] private Color checkerCol1, checkerCol2;
  
  protected override void InitTile() {
    _renderer.color = (coords.x + coords.y) % 2 == 0 ? checkerCol1 : checkerCol2;
  }
}
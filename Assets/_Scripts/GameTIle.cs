using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile {
  public Vector2Int coords;
  public TerrainType terrainType;
  public StructureType structureType;

  public GameTile(Vector2Int coords, TerrainType terrainType) {
    this.coords = coords;
    this.terrainType = terrainType;
  }

  public bool GetWalkable() => terrainType != TerrainType.Water;

  public override string ToString() {
    return $"Coords: {coords} | Terrain: {terrainType}";
  }
}

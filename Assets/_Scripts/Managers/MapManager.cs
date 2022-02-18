using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {
  public static MapManager Instance;
  private void Awake() {
    Instance = this;
  }

  public int mapSize = 32;
  public Tilemap terrainTilemap, structureTilemap;
  public Dictionary<Vector2Int, GameTile> gameTiles;
  [SerializeField] private NavMesh navMesh;
  [SerializeField] private Tile barrierTile, grassTile, waterTile;
  [SerializeField] private Tile treeTile, crateTile;

  public void BuildMap() {
    Texture2D terrainTex = PerlinNoise.Instance.GenerateTexture(mapSize, 3);
    Texture2D treeTex = PerlinNoise.Instance.GenerateTexture(mapSize, 10);
    gameTiles = new Dictionary<Vector2Int, GameTile>();

    for(int x = 0; x < mapSize; x++) {
      for(int y = 0; y < mapSize; y++) {
        float terrainPixelValue = terrainTex.GetPixel(x, y).r;
        float treePixelValue = treeTex.GetPixel(x, y).r;

        TerrainType terrainType = terrainPixelValue < 0.3f ? TerrainType.Water : TerrainType.Grass;
        if(x == 0 || y == 0 || x == mapSize - 1 || y == mapSize - 1)
          terrainType = TerrainType.Barrier;

        Vector2Int coords = new Vector2Int(x, y);

        GameTile newGameTile = new GameTile((Vector2Int) coords, terrainType);

        if(treePixelValue > 0.67 && terrainType == TerrainType.Grass)
          newGameTile.structureType = StructureType.Tree;

        gameTiles.Add((Vector2Int) coords, newGameTile);
      }
    }

    RenderMap();
    navMesh.Bake();
    GameManager.Instance.player.Spawn();
    GameManager.Instance.UpdateGameState(GameState.Play);
  }

  public void RenderMap() {
    foreach(var kvp in gameTiles) {
      GameTile gameTile = kvp.Value;
      Vector3Int coords3D = (Vector3Int) gameTile.coords;

      Tile terrainTileType = grassTile;
      switch(gameTile.terrainType) {
        case TerrainType.Barrier:
          terrainTileType = barrierTile;
          break;
        case TerrainType.Grass:
          terrainTileType = grassTile;
          break;
        case TerrainType.Water:
          terrainTileType = waterTile;
          break;
      }

      Tile structureTileType = null;
      switch(gameTile.structureType) {
        case StructureType.Tree:
          structureTileType = treeTile;
          break;
        case StructureType.Crate:
          structureTileType = crateTile;
          break;
      }

      terrainTilemap.SetTile(coords3D, terrainTileType);
      if(structureTileType != null)
        structureTilemap.SetTile(coords3D, structureTileType);
    }
  }

  public GameTile GetTile(Vector2Int coords) {
    if(gameTiles.TryGetValue(coords, out GameTile gameTile))
      return gameTile;
    else return null;
  }

  public void SetStructureAt(Vector2Int coords, StructureType structureType) {
    gameTiles[coords].structureType = structureType;
    RenderMap();
    navMesh.Rebake();
  }
}

public enum TerrainType {
  Barrier,
  Grass,
  Water,
}

public enum StructureType {
  None,
  Tree,
  Crate,
}
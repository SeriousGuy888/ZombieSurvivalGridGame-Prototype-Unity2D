using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {
  public static MapManager Instance;
  private void Awake() {
    Instance = this;
  }

  public int mapSize = 32;
  public Tilemap terrainTilemap;
  public Dictionary<Vector2Int, GameTile> gameTiles;
  [SerializeField] private Tile barrierTile, grassTile, waterTile;

  public void BuildMap() {
    Texture2D tex = PerlinNoise.Instance.GenerateTexture(mapSize, mapSize);
    gameTiles = new Dictionary<Vector2Int, GameTile>();

    for(int x = 0; x < mapSize; x++) {
      for(int y = 0; y < mapSize; y++) {
        float pixelVal = tex.GetPixel(x, y).r;

        TerrainType terrainType = pixelVal < 0.3f ? TerrainType.Water : TerrainType.Grass;
        if(x == 0 || y == 0 || x == mapSize - 1 || y == mapSize - 1)
          terrainType = TerrainType.Barrier;

        Vector2Int coords = new Vector2Int(x, y);
        GameTile newGameTile = new GameTile((Vector2Int) coords, terrainType);
        gameTiles.Add((Vector2Int) coords, newGameTile);
      }
    }

    RenderMap();
    GameManager.Instance.player.Spawn();
    GameManager.Instance.UpdateGameState(GameState.Play);
  }

  public void RenderMap() {
    foreach(var kvp in gameTiles) {
      GameTile gameTile = kvp.Value;
      Vector3Int coords3D = (Vector3Int) gameTile.coords;

      Tile tileType = grassTile;
      switch(gameTile.terrainType) {
        case TerrainType.Barrier:
          tileType = barrierTile;
          break;
        case TerrainType.Grass:
          tileType = grassTile;
          break;
        case TerrainType.Water:
          tileType = waterTile;
          break;
      }
      terrainTilemap.SetTile(coords3D, tileType);
    }
  }

  public GameTile GetTile(Vector2Int coords) {
    if(gameTiles.TryGetValue(coords, out GameTile gameTile))
      return gameTile;
    else return null;
  }
}

public enum TerrainType {
  Barrier,
  Grass,
  Water,
}
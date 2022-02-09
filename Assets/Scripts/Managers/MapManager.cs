using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
  public static MapManager Instance;

  public GameObject navMeshParent;

  public int width, height;
  [SerializeField] private float terrainNoiseScale;
  [SerializeField] private float waterThreshold;
  [SerializeField] private BaseTile grassTile, waterTile, barrierTile;
  [SerializeField] private BaseNonCreature treePrefab;

  private Dictionary<Vector2Int, BaseTile> allTiles;

  void Awake() {
    Instance = this;
  }

  public Vector2Int GetSpawnPoint => new Vector2Int(width / 2, height / 2);

  public Dictionary<Vector2Int, BaseTile> GetAllTiles() => allTiles;
  public BaseTile GetTile(Vector2Int coords) {
    if(allTiles.TryGetValue(coords, out BaseTile tile))
      return tile;
    return null;
  }


  public void GenerateMap() {
    GenerateGrid();
    PlaceTrees();
    GameManager.Instance.UpdateGameState(GameState.SpawnPlayer);
  }

  private void GenerateGrid() {
    allTiles = new Dictionary<Vector2Int, BaseTile>();
    Texture2D tex = GeneratePerlinNoiseTexture(terrainNoiseScale);

    for(int x = 0; x < width; x++) {
      for(int y = 0; y < height; y++) {
        float pixelVal = tex.GetPixel(x, y).r;
        BaseTile tileType;
        if(x == 0 || y == 0 || x == width - 1 || y == height - 1) { // spawn barrier tiles on edge
          tileType = barrierTile;
        } else { // everywhere else, spawn tiles based on the perlin noise texture
          tileType = pixelVal > waterThreshold ? grassTile : waterTile;
        }

        
        BaseTile spawnedTile = Instantiate(tileType, new Vector2(x, y), Quaternion.identity, transform);
        spawnedTile.name = $"Tile {x},{y}";
        spawnedTile.transform.SetParent(navMeshParent.transform);

        spawnedTile.Init(x, y);
        allTiles[new Vector2Int(x, y)] = spawnedTile;
      }
    }

    navMeshParent.GetComponent<NavMesh>().Bake();
  }

  private void PlaceTrees() {
    Texture2D tex = GeneratePerlinNoiseTexture(10);
    foreach(var kvp in allTiles) {
      var tile = kvp.Value;

      float pixelVal = tex.GetPixel(tile.coords.x, tile.coords.y).r;
      
      if(pixelVal > 0.67 && tile is GrassTile)
        Instantiate(treePrefab, tile.transform.position, Quaternion.identity);
    }
  }


  private Texture2D GeneratePerlinNoiseTexture(float noiseScale) {
    Texture2D texture = new Texture2D(width, height);
    float noiseOffsetX = Random.Range(0f, 666f);
    float noiseOffsetY = Random.Range(0f, 666f);

    for(int x = 0; x < width; x++) {
      for(int y = 0; y < height; y++) {
        Color col = CalcCol(x, y, noiseOffsetX, noiseOffsetY, noiseScale);
        texture.SetPixel(x, y, col);
      }
    }

    texture.Apply();
    return texture;
  }

  private Color CalcCol(int x, int y, float noiseOffsetX, float noiseOffsetY, float noiseScale) {
    float xFloat = (float) x / width * noiseScale + noiseOffsetX;
    float yFloat = (float) y / width * noiseScale + noiseOffsetY;

    float sample = Mathf.PerlinNoise(xFloat, yFloat);

    // make land less common towards the edges of the map
    // Vector2 middle = new Vector2(width / 2, height / 2);
    // Vector2 currPoint = new Vector2(x, y);
    // float diff = Vector2.Distance(middle, currPoint) / (width / 2);
    // sample -= (diff * 0.4f);


    return new Color(sample, 0, 0);
  }
}
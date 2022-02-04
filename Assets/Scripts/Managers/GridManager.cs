using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
  public static GridManager Instance;

  public int width, height;
  [SerializeField] private float noiseScale;
  [SerializeField] private float waterThreshold;
  [SerializeField] private BaseTile grassTile, waterTile;

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

  public void GenerateGrid() {
    allTiles = new Dictionary<Vector2Int, BaseTile>();
    Texture2D tex = GeneratePerlinNoiseTexture();

    for(int x = 0; x < width; x++) {
      for(int y = 0; y < height; y++) {
        float pixelVal = tex.GetPixel(x, y).r;
        BaseTile whichTile = pixelVal > waterThreshold ? grassTile : waterTile;

        BaseTile spawnedTile = Instantiate(whichTile, new Vector2(x, y), Quaternion.identity, transform);
        spawnedTile.name = $"Tile {x},{y}";

        spawnedTile.Init(x, y);
        allTiles[new Vector2Int(x, y)] = spawnedTile;
      }
    }

    GameManager.Instance.UpdateGameState(GameState.SpawnPlayer);
  }


  private Texture2D GeneratePerlinNoiseTexture() {
    Texture2D texture = new Texture2D(width, height);
    float noiseOffsetX = Random.Range(0f, 666f);
    float noiseOffsetY = Random.Range(0f, 666f);

    for(int x = 0; x < width; x++) {
      for(int y = 0; y < height; y++) {
        Color col = CalcCol(x, y, noiseOffsetX, noiseOffsetY);
        texture.SetPixel(x, y, col);
      }
    }

    texture.Apply();
    return texture;
  }

  private Color CalcCol(int x, int y, float noiseOffsetX, float noiseOffsetY) {
    float xFloat = (float) x / width * noiseScale + noiseOffsetX;
    float yFloat = (float) y / width * noiseScale + noiseOffsetY;

    float sample = Mathf.PerlinNoise(xFloat, yFloat);

    // make land less common towards the edges of the map
    Vector2 middle = new Vector2(width / 2, height / 2);
    Vector2 currPoint = new Vector2(x, y);
    float diff = Vector2.Distance(middle, currPoint) / (width / 2);
    sample -= (diff * 0.4f);


    return new Color(sample, 0, 0);
  }
}
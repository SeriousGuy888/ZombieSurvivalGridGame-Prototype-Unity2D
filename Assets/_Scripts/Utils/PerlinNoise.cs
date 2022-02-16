using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour {
  public static PerlinNoise Instance;
  private void Awake() {
    Instance = this;
  }

  [SerializeField] private int noiseScale;


  public Texture2D GenerateTexture(int width, int height) {
    Texture2D texture = new Texture2D(width, height);
    float noiseOffsetX = Random.Range(0f, 666f);
    float noiseOffsetY = Random.Range(0f, 666f);

    for(int x = 0; x < width; x++) {
      for(int y = 0; y < height; y++) {
        Color col = CalcCol(
          x,
          y,
          width,
          height,
          noiseOffsetX,
          noiseOffsetY,
          noiseScale);
        texture.SetPixel(x, y, col);
      }
    }

    texture.Apply();
    return texture;
  }

  private Color CalcCol(
      int x,
      int y,
      int width,
      int height,
      float noiseOffsetX,
      float noiseOffsetY,
      float noiseScale) {
    float xFloat = (float) x / width * noiseScale + noiseOffsetX;
    float yFloat = (float) y / width * noiseScale + noiseOffsetY;

    float sample = Mathf.PerlinNoise(xFloat, yFloat);

    return new Color(sample, 0, 0);
  }
}
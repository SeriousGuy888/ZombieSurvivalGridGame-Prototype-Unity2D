using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour {
  public static PerlinNoise Instance;
  private void Awake() {
    Instance = this;
  }

  public Texture2D GenerateTexture(int sideLength, float noiseScale) {
    Texture2D texture = new Texture2D(sideLength, sideLength);
    float noiseOffsetX = Random.Range(0f, 666f);
    float noiseOffsetY = Random.Range(0f, 666f);

    for(int x = 0; x < sideLength; x++) {
      for(int y = 0; y < sideLength; y++) {
        Color col = CalcCol(
          x,
          y,
          sideLength,
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
      int sideLength,
      float noiseOffsetX,
      float noiseOffsetY,
      float noiseScale) {
    float xFloat = (float) x / sideLength * noiseScale + noiseOffsetX;
    float yFloat = (float) y / sideLength * noiseScale + noiseOffsetY;

    float sample = Mathf.PerlinNoise(xFloat, yFloat);

    return new Color(sample, 0, 0);
  }
}